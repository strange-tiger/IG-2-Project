using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEditor;
using System.IO;
using MySql.Data.MySqlClient;


namespace Asset.MySql
{
    public enum ETableType
    {
        AccountDB,
        CharacterDB,
        RelationshipDB,
        Max
    };

    public enum EAccountColumns
    {
        Email,
        Password,
        Nickname,
        Qustion,
        Answer,
        Max
    }

    public enum ECharacterColumns
    {
        Nickname,
        Gender,
        Tutorial,
        OnOff,
        Max
    }

    public enum ESocialStatus
    {
        None,
        Request,
        Friend,
        Block,
        Denied,
        Max
    }

    public class MySqlSetting
    {
        private static bool hasInit = false;

        private static string _connectionString;
        private static string[] _insertStrings = new string[(int)ETableType.Max];
        private static string _insertSocialStateString;
        private static string _insertSocialRequestString;
        private static string _selectAccountString;
        private static string _selectSocialStateString;
        private static string _updateSocialStateString;

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
            _insertSocialStateString = Resources.Load<TextAsset>("InsertSocial").text;
            _insertSocialRequestString = Resources.Load<TextAsset>("InsertRequest").text;
            _selectAccountString = Resources.Load<TextAsset>("Select").text;
            _updateSocialStateString = Resources.Load<TextAsset>("UpdateRelationship").text;

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


                    _mysqlConnection.Open();
                    _insertAccountCommand.ExecuteNonQuery();
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

        public static bool AddNewCharacter(string nickname, string gender)
        {
            try
            {

                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    string _insertCharacterString = GetInsertString(ETableType.CharacterDB, nickname, gender);

                    MySqlCommand _insertCharacterCommand = new MySqlCommand(_insertCharacterString, _mysqlConnection);

                    _mysqlConnection.Open();
                    _insertCharacterCommand.ExecuteNonQuery();
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
        /// 두 사용자 간의 요청이 RequestDB에 존재하는 지 확인.
        /// </summary>
        /// <param name="userA"></param>
        /// <param name="userB"></param>
        /// <returns> 존재하면 true 아니면 false </returns>
        public static bool CheckRequest(string userA, string userB)
        {

            using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
            {
                string selcetSocialRequestString = $"select * from RequestDB where Requester = '{userA}' and Repondent = '{userB}' or Requester = '{userB}' and Repondent = '{userA};";

                MySqlCommand selectSocialRequestCommand = new MySqlCommand(selcetSocialRequestString, _mysqlConnection);

                _mysqlConnection.Open();

                MySqlDataReader selectSocialStatusData = selectSocialRequestCommand.ExecuteReader();
                if (selectSocialStatusData.Read())
                {
                    _mysqlConnection.Close();

                    return true;
                }

                _mysqlConnection.Close();
                return false;
            }



        }

        /// <summary>
        /// 친구 요청을 RequestDB에 저장함.
        /// </summary>
        /// <param name="requester">신청인</param>
        /// <param name="respondent">대상자</param>
        /// <returns>성공하면 true, 실패하면 false 반환</returns>
        public static bool RequestSocialInteraction(string requester, string respondent)
        {
            if (CheckRequest(requester, respondent))
            {
                Debug.LogError("이미 요청이 존재합니다.");
                return false;
            }

            try
            {
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    string insertSocialRequestString = _insertSocialRequestString + $"('{requester}','{respondent}');";

                    MySqlCommand insertSocialRequestCommand = new MySqlCommand(insertSocialRequestString, _mysqlConnection);

                    _mysqlConnection.Open();
                    insertSocialRequestCommand.ExecuteNonQuery();
                    _mysqlConnection.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }



        /// <summary>
        /// 유저간의 관계가 존재하는 지 확인함.
        /// </summary>
        /// <param name="userA"> UserA Column에 들어가는 유저 닉네임. </param>
        /// <param name="userB"> UserB Column에 들어가는 유저 닉네임. </param>
        /// <returns> 존재하면 true, 존재하지 않는다면 false 반환. </returns>
        private bool IsThereRelationship(string userA, string userB)
        {
            using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
            {
                string selcetSocialRequestString = _selectSocialStateString + $"where UserA = '{userA}' and UserB = '{userB}' " +
                    $"or UserA = '{userB}' and UserB = '{userA}';";

                MySqlCommand command = new MySqlCommand(selcetSocialRequestString, _mysqlConnection);

                _mysqlConnection.Open();

                MySqlDataReader reader = command.ExecuteReader();


                if (!reader.Read())
                {
                    _mysqlConnection.Close();
                    return false;
                }

                _mysqlConnection.Close();
                return true;

            }
        }

        /// <summary>
        /// 내가 UserA인지, UserB인지 판단함.
        /// </summary>
        /// <param name="myNickname"> 나의 닉네임</param>
        /// <param name="targetNickname"> 대상의 닉네임 </param>
        /// <param name="isLeft">내가 UserA라면 True, UserB라면 false</param>
        /// <returns>Row가 존재하면 True, 존재하지 않으면 false </returns>
        public bool CheckMyPositionInRelationShip(string myNickname, string targetNickname, out bool isLeft)
        {

            using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
            {
                string selcetSocialRequestString = _selectSocialStateString + $"where UserA = '{myNickname}' and UserB = '{targetNickname}' " +
                    $"or UserA = '{targetNickname}' and UserB = '{myNickname}';";

                MySqlCommand command = new MySqlCommand(selcetSocialRequestString, _mysqlConnection);

                _mysqlConnection.Open();

                MySqlDataReader reader = command.ExecuteReader();


                if (reader.Read())
                {
                    isLeft = false;

                    if (reader["UserA"].ToString() == myNickname)
                    {
                        isLeft = true;
                    }
                    else if (reader["UserB"].ToString() == myNickname)
                    {
                        isLeft = false;
                    }

                    _mysqlConnection.Close();
                    return true;
                }

                isLeft = false;
                return false;
            }

        }

        /// <summary>
        /// 나와 대상 유저간의 관계를 확인하고, 관계 값을 반환함.
        /// </summary>
        /// <param name="myNickname"> 나의 닉네임 </param>
        /// <param name="targetNickname"> 대상 유저의 닉네임 </param>
        /// <param name="isLeft"> 내가 UserA Column이라면 true, UserB Column이라면 false. </param>
        /// <returns> 나와 대상 유저간의 State를 int로 반환함. 관계가 존재하지 않는다면 -1 반환. </returns>
        public int CheckRelationship(string myNickname, string targetNickname, out bool isLeft)
        {
            if (CheckMyPositionInRelationShip(myNickname, targetNickname, out isLeft) == false)
            {
                return -1;
            }

            CheckMyPositionInRelationShip(myNickname, targetNickname, out isLeft);

            using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
            {
                int state = _initBit >> 1;

                string selcetSocialRequestString = _selectSocialStateString + $"where UserA = '{myNickname}' and UserB = '{targetNickname}' " +
                    $"or UserA = '{targetNickname}' and UserB = '{myNickname}';";

                MySqlCommand command = new MySqlCommand(selcetSocialRequestString, _mysqlConnection);

                _mysqlConnection.Open();

                MySqlDataReader reader = command.ExecuteReader();


                if (reader.Read())
                {
                    state = reader.GetInt32("State");
                    
                }
                _mysqlConnection.Close();

                return state;
            }
        }
        
        private const int _initBit = 0b_0001;
        /// <summary>
        /// 연산된 State를 받아, RelationshipDB의 State를 업데이트 함. DB에 관계가 없다면, 새로 추가함.
        /// </summary>
        /// <param name="userA"> UserA column에 들어가는 닉네임. </param>
        /// <param name="userB"> UserB column에 들어가는 닉네임. </param>
        /// <param name="state">업데이트할 State </param>
        /// <returns>성공하면 true, 실패하면 false를 반환함. </returns>
        public bool UpdateRelationship(string userA, string userB, int state)
        {
            if(IsThereRelationship(userA,userB) == false)
            {
                try
                {
                    using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                    {
                        string insertSocialStateString = _insertSocialStateString + $"('{userA}','{userB}',{state});";

                        MySqlCommand insertSocialStateCommand = new MySqlCommand(insertSocialStateString, _mysqlConnection);

                        _mysqlConnection.Open();
                        insertSocialStateCommand.ExecuteNonQuery();
                        _mysqlConnection.Close();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            try
            {
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                
                    string updateSocialStateString =  _updateSocialStateString += $"{state} where UserA = '{userA}' and UserB = '{userB}' " +
                        $"or UserA = '{userB}' and UserB = '{userA}';";
 
                    MySqlCommand updateSocialStateCommand = new MySqlCommand(updateSocialStateString, _mysqlConnection);
                    _mysqlConnection.Open();
                    updateSocialStateCommand.ExecuteNonQuery();
                    _mysqlConnection.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 나와 대상 유저의 관계에서 차단 State를 연산함.
        /// </summary>
        /// <param name="myNickname"> 나의 닉네임 </param>
        /// <param name="targetNickname"> 대상 유저의 닉네임 </param>
        /// <returns>성공하면 true, 실패하면 false를 반환함. </returns>
        public bool UpdateRelationshipToBlock(string myNickname,string targetNickname)
        {
            try
            {
                int state = CheckRelationship(myNickname, targetNickname, out bool isLeft);

                int changeState = UpdateRelationshipToBlockHelper(isLeft, _initBit);

                changeState = state | changeState;

                UpdateRelationship(myNickname, targetNickname, changeState);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 나와 대상 유저의 관계에서 차단 해제 State를 연산함.
        /// </summary>
        /// <param name="myNickname"> 나의 닉네임 </param>
        /// <param name="targetNickname"> 대상 유저의 닉네임 </param>
        /// <returns>성공하면 true, 실패하면 false를 반환함. </returns>
        public bool UpdateRalationshipToUnblock(string myNickname, string targetNickname)
        {
            try
            {
                int state = CheckRelationship(myNickname, targetNickname, out bool isLeft);

                int changeState = UpdateRelationshipToBlockHelper(isLeft, _initBit);

                changeState = state & ~changeState;

                UpdateRelationship(myNickname, targetNickname, changeState);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private int UpdateRelationshipToBlockHelper(bool isLeft, int changeState)
        {
            changeState = changeState | changeState << 1;

            return UpdateRelationshipToRequestHelper(isLeft, changeState);
        }

        /// <summary>
        /// 내가 상대에게 친구 요청을 한다.
        /// 사전조건 판단은 외부에서 수행한다.
        /// </summary>
        /// <param name="myNickname"></param>
        /// <param name="targetNickname"></param>
        /// <returns></returns>
        public bool UpdateRelationshipToRequest(string myNickname, string targetNickname)
        {
            try
            {
                bool isLeft;
                int state = CheckRelationship(myNickname, targetNickname, out isLeft);

                int changeState = UpdateRelationshipToRequestHelper(isLeft, _initBit);

                state = state | changeState;
                
                UpdateRelationship(myNickname, targetNickname, state);

                return true;
            }
            catch
            {
                Debug.LogError("오류: Fail To Request Social Interaction");
                return false;
            }
        }

        /// <summary>
        /// 나와 상대의 관계를 친구로 업데이트한다.
        /// 사전조건 판단은 외부에서 수행한다.
        /// </summary>
        /// <param name="myNickname"></param>
        /// <param name="targetNickname"></param>
        /// <returns></returns>
        public bool UpdateRelationshipToFriend(string myNickname, string targetNickname)
        {
            int changeState = _initBit >> 1;

            try
            {
                UpdateRelationship(myNickname, targetNickname, changeState);
                return true;
            }
            catch
            {
                Debug.LogError("오류: Fail To Be Friend");
                return false;
            }
        }

        /// <summary>
        /// 나와 상대의 친구 관계를 끊는다.
        /// 사전조건 판단은 외부에서 수행한다.
        /// </summary>
        /// <param name="myNickname"></param>
        /// <param name="targetNickname"></param>
        /// <returns></returns>
        public bool UpdateRelationshipToUnFriend(string myNickname, string targetNickname)
        {
            try
            {
                DeleteRowByComparator
                (
                    ErelationshipdbColumns.UserA,
                    myNickname,
                    ErelationshipdbColumns.UserB,
                    targetNickname
                );

                return true;
            }
            catch
            {
                Debug.LogError("오류: Fail To Undo Friend");
                return false;
            }
        }

        private int UpdateRelationshipToRequestHelper(bool isLeft, int changeState)
        {
            if (isLeft)
            {
                return changeState << 2;
            }
            return changeState;
        }


        /// <summary>
        /// DataSet에 데이터를 저장함.
        /// </summary>
        /// <param name="selectString"> 저장할 데이터를 가져올 명령어</param>
        /// <param name="nickname">명령어에 들어갈 닉네임</param>
        /// <returns>데이터를 저장한 DataSet을 반환</returns>
        private static DataSet GetUserData(string selectString)
        {
            DataSet _dataSet = new DataSet();

            using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
            {
                _sqlConnection.Open();

                MySqlDataAdapter _dataAdapter = new MySqlDataAdapter(selectString, _sqlConnection);

                _dataAdapter.Fill(_dataSet);
            }
            return _dataSet;
        }

        /// <summary>
        /// 특정 유저의 특정 상태를 리스트로 만들어서 반환.
        /// </summary>
        /// <param name="nickname">리스트를 만들 유저</param>
        /// <param name="status"> 리스트를 만들 상태</param>
        /// <returns>Block이 아니면, 요청자와 대상자 Column을 모두 검사하여 반환, Block이면 요청자만 검사하여 반환.</returns>
        public static List<Dictionary<string, string>> CheckStatusList(string nickname, ESocialStatus status)
        {

            string selcetRequesterString = $"SELECT CharacterDB.OnOff, RelationshipDB.Respondent FROM RelationshipDB " +
               $"INNER JOIN CharacterDB ON CharacterDB.Nickname = RelationshipDB.Respondent where Requester = '{nickname}' and Status = '{(int)status}'; ";



            DataSet requesterData = GetUserData(selcetRequesterString);

            List<Dictionary<string, string>> resultList = new List<Dictionary<string, string>>();

            foreach (DataRow _dataRow in requesterData.Tables[0].Rows)
            {
                Dictionary<string, string> dictionaryList = new Dictionary<string, string>();
                dictionaryList.Add(_dataRow["Respondent"].ToString(), _dataRow["OnOff"].ToString());
                resultList.Add(dictionaryList);
            }

            if (status != ESocialStatus.Block)
            {
                string selcetRespondentString = $"SELECT CharacterDB.OnOff, RelationshipDB.Requester FROM RelationshipDB " +
                    $"INNER JOIN CharacterDB ON CharacterDB.Nickname = RelationshipDB.Requester where Respondent = '{nickname}' and Status = '{(int)status}'; ";

                DataSet respondentData = GetUserData(selcetRespondentString);

                foreach (DataRow _dataRow in respondentData.Tables[0].Rows)
                {
                    Dictionary<string, string> dictionaryList = new Dictionary<string, string>();
                    dictionaryList.Add(_dataRow["Requester"].ToString(), _dataRow["OnOff"].ToString());
                    resultList.Add(dictionaryList);

                }
            }

            return resultList;

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
        /// CharacterDB Table에서 baseType의 baseValue를 기준으로 checkType의 checkValue가 일치하는지 확인함
        /// </summary>
        /// <param name="baseType">기준 데이터 Column 타입</param>
        /// <param name="baseValue">기준 데이터 값</param>
        /// <param name="checkType">확인할 데이터 Column 타입</param>
        /// <param name="checkValue">확인할 값</param>
        /// <returns>일치하면 true를 반환, 아니거나 오류가 있을 경우 false 반환</returns>
        public static bool CheckValueByBase(ECharacterColumns baseType, string baseValue,
            ECharacterColumns checkType, string checkValue)
        {
            return CheckValueByBase(ETableType.CharacterDB, baseType, baseValue, checkType, checkValue);
        }
        /// <summary>
        /// AccountDB Table에서 baseType의 baseValue를 기준으로 checkType의 checkValue가 일치하는지 확인함
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
        /// AccountDB 테이블에서 baseType의 baseValue를 기준으로 targetType의 데이터를 가져옴
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
        /// AccountDB Table에서 baseType의 baseValue를 기준으로 TargetType을 TargetValue로 변경함
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

        public static bool UpdateValueByBase(ECharacterColumns baseType, string baseValue,
            ECharacterColumns targetType, int targetValue)
        {
            return UpdateValueByBase(ETableType.CharacterDB, baseType, baseValue, targetType, targetValue);
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

        public class Comparator<T> where T : System.Enum
        {
            public T Column;
            public string Value;
        }

        #region DeleteRowByComparator-RelationshipDB

        public static bool DeleteRowByComparator
         (ErelationshipdbColumns type_1, string condition_1,
          ErelationshipdbColumns type_2, string condition_2,
          ErelationshipdbColumns type_3, string condition_3,
          string logicOperator = "and")
        {

            return DeleteRowByComparator
            (
                Asset.ETableType.relationshipdb,
                logicOperator,
                new Comparator<ErelationshipdbColumns>()
                {
                    Column = type_1,
                    Value = condition_1
                },
                new Comparator<ErelationshipdbColumns>()
                {
                    Column = type_2,
                    Value = condition_2
                },
                new Comparator<ErelationshipdbColumns>()
                {
                    Column = type_3,
                    Value = condition_3
                }
            );
        }

        public static bool DeleteRowByComparator
        (ErelationshipdbColumns type_1, string condition_1,
         ErelationshipdbColumns type_2, string condition_2,
         string logicOperator = "and")
        {
            if 
            (
                DeleteRowByComparator
                (
                    Asset.ETableType.relationshipdb,
                    logicOperator,
                    new Comparator<ErelationshipdbColumns>()
                    {
                        Column = type_1,
                        Value = condition_1
                    },
                    new Comparator<ErelationshipdbColumns>()
                    {
                        Column = type_2,
                        Value = condition_2
                    }
                )
            )
            {
                return true;
            }
            else
            {
                return DeleteRowByComparator
                (
                    Asset.ETableType.relationshipdb,
                    logicOperator,
                    new Comparator<ErelationshipdbColumns>()
                    {
                        Column = type_2,
                        Value = condition_2
                    },
                    new Comparator<ErelationshipdbColumns>()
                    {
                        Column = type_1,
                        Value = condition_1
                    }
                );
            }
        }

        public static bool DeleteRowByComparator
        (ErelationshipdbColumns type, string condition,
        string logicOperator = "and")
        {

            return DeleteRowByComparator
            (
                Asset.ETableType.relationshipdb,
                logicOperator,
                new Comparator<ErelationshipdbColumns>() 
                { 
                    Column = type, 
                    Value = condition 
                }
            );
        }

        #endregion

        public static bool DeleteRowByComparator<T>
        (Asset.ETableType targetTable, string logicOperator,
        params Comparator<T>[] comparators) 
            where T : System.Enum
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string deleteString = $"Delete From {targetTable} ";

                    for (int i = 0; i < comparators.Length - 1; ++i)
                    {
                        deleteString += $"where {comparators[i].Column} = '{comparators[i].Value}' {logicOperator} ";
                    }

                    deleteString += $"where {comparators[comparators.Length - 1].Column} = '{comparators[comparators.Length - 1].Value}';";

                    MySqlCommand command = new MySqlCommand(deleteString, _sqlConnection);

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

    }

}