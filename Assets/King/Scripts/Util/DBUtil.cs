using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEditor;
using System.IO;
using MySql.Data.MySqlClient;
using LitJson;


namespace Asset.MySql
{
    public enum ETableType
    {
        AccountDB,
        AccountInfoDB,
        Max
    };

    public enum EAccountColumns
    {
        Email,
        Password,
        Nickname,
        Max
    }

    public enum EAccountInfoColumns
    {
        Nickname,
        AccountData,
        Max
    }

    public class MySqlSetting
    {
        private static bool hasInit = false;

        private static string _connectionString;
        private static string[] _insertStrings = new string[(int)ETableType.Max];
        private static string _insertInitDataString;
        private static string _selectAccountString;
        private static string _selectJsonString;

        /// <summary>
        ///  MySql 세팅 초기화
        /// </summary>
        public static void Init()
        {
            if (hasInit)
            {
                return;
            }

            Init(true);
        }

        /// <summary>
        /// MySql 세팅을 초기화
        /// </summary>
        /// <param name="isNeedReset"> 초기화가 필요하면 true, 아니면 false</param>
        public static void Init(bool isNeedReset)
        {
            if (!isNeedReset)
            {
                return;
            }

            _connectionString = Resources.Load<TextAsset>("Connection").text;
            _insertStrings = Resources.Load<TextAsset>("Insert").text.Split('\n');
            _insertInitDataString = Resources.Load<TextAsset>("InitInsertAccountData").text;
            _selectAccountString = Resources.Load<TextAsset>("Select").text;

            SetEnum();
            Debug.Log("Enum Setting 끝");
        }

        [MenuItem("Tools/GenerateEnum")]
        private static void SetEnum()
        {
            string settingString = Resources.Load<TextAsset>("SetEnum").text;
            string tableTypeString = settingString.Split('\n')[0];
            string columnTypeString = settingString.Split('\n')[1];

            List<string> tableNames = new List<string>();
            Dictionary<string, List<string>> columNames = new Dictionary<string, List<string>>();

            try
            {
                // DB에서 테이블과 컬럼명 가져오기
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    _sqlConnection.Open();

                    // 테이블 명 가져오기
                    MySqlCommand tableTypeCommand = new MySqlCommand(tableTypeString, _sqlConnection);
                    MySqlDataReader tableTypeReader = tableTypeCommand.ExecuteReader();

                    while (true)
                    {
                        if (tableTypeReader.Read() == false)
                        {
                            break;
                        }

                        string tableName = tableTypeReader[0].ToString();
                        tableNames.Add(tableName);
                        columNames.Add(tableName, new List<string>());
                    }

                    tableTypeReader.Close();

                    // 테이블 명에 따라 Column 값 가져오기
                    foreach (string table in tableNames)
                    {
                        string columnSelectString = columnTypeString + table + ";";

                        MySqlCommand columnCommand = new MySqlCommand(columnSelectString, _sqlConnection);
                        MySqlDataReader columnTypeReader = columnCommand.ExecuteReader();

                        while (true)
                        {
                            if (columnTypeReader.Read() == false)
                            {
                                break;
                            }

                            string columnName = columnTypeReader["Field"].ToString();
                            columNames[table].Add(columnName);
                        }
                        columnTypeReader.Close();
                    }

                    _sqlConnection.Close();
                }

                // 해당 내용에 맞는 파일 생성하기
                using (StreamWriter streamWriter = new StreamWriter("./Assets/Scripts/Util/MySqlEnum.cs"))
                {
                    // 전처리
                    streamWriter.WriteLine("namespace Asset {");

                    // enum 생성하기
                    //  1. 테이블 타입 
                    streamWriter.WriteLine("\tpublic enum ETableType {");
                    foreach (string table in tableNames)
                    {
                        streamWriter.WriteLine($"\t\t{table},");
                    }
                    streamWriter.WriteLine("\t}");

                    //  2. 테이블의 컬럼 타입
                    foreach (string table in tableNames)
                    {
                        streamWriter.WriteLine($"\tpublic enum E{table}Columns {{");

                        foreach (string column in columNames[table])
                        {
                            streamWriter.WriteLine($"\t\t{column},");
                        }

                        streamWriter.WriteLine("\t}");
                    }

                    // 후처리
                    streamWriter.WriteLine("}");
                }
                AssetDatabase.Refresh();
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return;
            }
        }

        /// <summary>
        /// 계정 추가하기
        /// </summary>
        /// <param name="Email">계정 Email</param>
        /// <param name="Password">계정 PW</param>
        /// <param name="Nickname">계정 Nickname</param>
        /// <returns>정상적으로 입력이 되었을 경우 true, 아니면 false
        /// (대표적으로 email Nickname이 겹칠 경우 false 반환)</returns>
        public static bool AddNewAccount(string Email, string Password, string Nickname)
        {
            try
            {
                if (HasValue(EAccountColumns.Email, Email))
                {
                    throw new System.Exception("Email 중복됨");
                }
                if (HasValue(EAccountColumns.Nickname, Nickname))
                {
                    throw new System.Exception("Nickname 중복됨");
                }


                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    string _insertAccountString = GetInsertString(ETableType.AccountDB, Nickname, Password, Email);
                    MySqlCommand _insertAccountCommand = new MySqlCommand(_insertAccountString, _mysqlConnection);

                    string _insertAccountInfoString = GetInsertString(ETableType.AccountInfoDB, Nickname);
                    MySqlCommand _insertAccountInfoCommand = new MySqlCommand(_insertAccountInfoString, _mysqlConnection);

                    MySqlCommand _insertInitDataCommand = new MySqlCommand(_insertInitDataString + $"where {Nickname};", _mysqlConnection);

                    _mysqlConnection.Open();
                    _insertAccountCommand.ExecuteNonQuery();
                    _insertAccountInfoCommand.ExecuteNonQuery();
                    _insertInitDataCommand.ExecuteNonQuery();
                    _mysqlConnection.Close();
                }

                return true;
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return false;
            }
        }
        private static string GetInsertString(ETableType tableType, params string[] values)
        {
            string insertString = _insertStrings[(int)tableType] + '(';

            foreach (string value in values)
            {
                insertString += $"'{value}',";
            }

            insertString = insertString.TrimEnd(',') + ");";

            return insertString;
        }

       


        /// <summary>
        /// 해당 값이 DB에 있는지 확인한다.
        /// </summary>
        /// <param name="columnType">Account 태이블에서 비교하기 위한 colum 명</param>
        /// <param name="value">비교할 값</param>
        /// <returns>값이 있다면 true, 아니면 false를 반환한다.</returns>
        public static bool HasValue(EAccountColumns columnType, string value)
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    bool result = false;

                    string selectString = _selectAccountString + $" WHERE {columnType} = '{value}';";

                    _sqlConnection.Open();

                    MySqlCommand _selectCommand = new MySqlCommand(selectString, _sqlConnection);
                    MySqlDataReader _selectData = _selectCommand.ExecuteReader();

                    result = _selectData.Read();

                    _sqlConnection.Close();

                    return result;
                }
            }
            catch
            {
                Debug.LogError("오류남: Doublecheck");
                return false;
            }

        }

        /// <summary>
        /// AccountInfo Table에서 baseType의 baseValue를 기준으로 checkType의 checkValue가 일치하는지 확인함
        /// </summary>
        /// <param name="baseType">기준 데이터 Column 타입</param>
        /// <param name="baseValue">기준 데이터 값</param>
        /// <param name="checkType">확인할 데이터 Column 타입</param>
        /// <param name="checkValue">확인할 값</param>
        /// <returns>일치하면 true를 반환, 아니거나 오류가 있을 경우 false 반환</returns>
        public static bool CheckValueByBase(EAccountInfoColumns baseType, string baseValue,
            EAccountInfoColumns checkType, string checkValue)
        {
            return CheckValueByBase(ETableType.AccountInfoDB, baseType, baseValue, checkType, checkValue);
        }
        /// <summary>
        /// Account Table에서 baseType의 baseValue를 기준으로 checkType의 checkValue가 일치하는지 확인함
        /// ex. ID(baseType)가 aaa(baseValue)인 데이터의 Password(checkType)이 123(checkValue)인지 확인함
        /// </summary>
        /// <param name="baseType">기준 데이터 Column 타입</param>
        /// <param name="baseValue">기준 데이터 값</param>
        /// <param name="checkType">확인할 데이터 Column 타입</param>
        /// <param name="checkValue">확인할 값</param>
        /// <returns>일치하면 true를 반환, 아니거나 오류가 있을 경우 false 반환</returns>
        public static bool CheckValueByBase(EAccountColumns baseType, string baseValue,
            EAccountColumns checkType, string checkValue)
        {
            return CheckValueByBase(ETableType.AccountDB, baseType, baseValue, checkType, checkValue);
        }
        private static bool CheckValueByBase<T>(ETableType targetTable, T baseType, string baseValue,
            T checkType, string checkValue) where T : System.Enum
        {
            string checkTargetValue = GetValueByBase(targetTable, baseType, baseValue, checkType);
            if (checkTargetValue != null)
            {
                return checkTargetValue == checkValue;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Account 테이블에서 baseType의 baseValue를 기준으로 targetType의 데이터를 가져옴
        /// </summary>
        /// <param name="baseType">기준이 되는 값의 Column명</param>
        /// <param name="baseValue">기준이 되는 데이터</param>
        /// <param name="targetType">가져오기 위한 데이터 Column명</param>
        /// <returns>해당 데이터를 반환. 오류 시 null 반환</returns>
        public static string GetValueByBase(EAccountColumns baseType, string baseValue, EAccountColumns targetType)
        {
            return GetValueByBase(ETableType.AccountDB, baseType, baseValue, targetType);
        }
       
        private static string GetValueByBase<T>(ETableType targetTable,
            T baseType, string baseValue,
            T targetType) where T : System.Enum
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string selectString = $"Select {targetType} from {targetTable} where {baseType} = '{baseValue}';";

                    _sqlConnection.Open();

                    MySqlCommand command = new MySqlCommand(selectString, _sqlConnection);
                    MySqlDataReader resultReader = command.ExecuteReader();

                    if (!resultReader.Read())
                    {
                        throw new System.Exception("base 값이 없음");
                    }

                    string result = resultReader[targetType.ToString()].ToString();

                    _sqlConnection.Close();

                    return result;
                }
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return null;
            }

        }

       
        /// <summary>
        /// Account Table에서 baseType의 baseValue를 기준으로 TargetType을 TargetValue로 변경함
        /// </summary>
        /// <param name="baseType">기준 값의 Column명</param>
        /// <param name="baseValue">기준 값의 데이터</param>
        /// <param name="targetType">변경할 값의 Column명</param>
        /// <param name="targetValue">변경할 값</param>
        /// <returns>정상적으로 변경되었다면 true, 아니면 false를 반환</returns>
        public static bool UpdateValueByBase(EAccountColumns baseType, string baseValue,
            EAccountColumns targetType, int targetValue)
        {
            return UpdateValueByBase(ETableType.AccountDB, baseType, baseValue, targetType, targetValue);
        }
        
        public static bool UpdateValueByBase(EAccountInfoColumns baseType, string baseValue,
            EAccountInfoColumns targetType, int targetValue)
        {
            return UpdateValueByBase(ETableType.AccountInfoDB, baseType, baseValue, targetType, targetValue);
        }
        private static bool UpdateValueByBase<T>(ETableType targetTable,
            T baseType, string baseValue,
            T targetType, int targetValue) where T : System.Enum
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string updateString = $"Update {targetTable} set {targetType} = {targetValue} where {baseType} = '{baseValue}';";
                    MySqlCommand command = new MySqlCommand(updateString, _sqlConnection);

                    _sqlConnection.Open();
                    command.ExecuteNonQuery();
                    _sqlConnection.Close();

                    return true;
                }
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return false;
            }


        }

        /// <summary>
        /// 유저의 캐릭터 데이터를 가진 Json을 불러옴.
        /// </summary>
        /// <param name="nickname"> 유저의 닉네임</param>
        /// <returns>JsonData로 반환.</returns>
        public static JsonData SelectCharacterData(string nickname)
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string selectString = $"Select * from AccountInfoDB where {nickname};";
                    MySqlCommand selectCommand = new MySqlCommand(selectString, _sqlConnection);
                    _sqlConnection.Open();
                    MySqlDataReader dataReader = selectCommand.ExecuteReader();
                    if(dataReader.Read())
                    {
                        _selectJsonString = dataReader["AccountData"].ToString();
                    }
                    JsonData resultData = JsonMapper.ToObject(_selectJsonString);
                    _sqlConnection.Close();

                    return resultData;
                }
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return false;
            }
        }

        /// <summary>
        /// 유저의 캐릭터 데이터 Json에서 특정 값을 바꿔줌
        /// </summary>
        /// <param name="nickname">유저의 닉네임</param>
        /// <param name="targetColumn">값을 바꿔줄 Column명</param>
        /// <param name="value">바꿔줄 값</param>
        /// <returns>성공하면 true, 실패하면 false</returns>
        public static bool ReplaceCharacterData(string nickname, string targetColumn, string value)
        {
            try
            {
                using(MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string replaceString = $"Update AccountInfoDB set AccountData = json_replace(AccountData,'$.{targetColumn}', {value}) where {nickname};";

                    MySqlCommand replaceCommand = new MySqlCommand(replaceString, _sqlConnection);
                    _sqlConnection.Open();
                    replaceCommand.ExecuteNonQuery();
                    _sqlConnection.Close();

                    return true;
                }
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return false;
            }
        }
        
    }

}