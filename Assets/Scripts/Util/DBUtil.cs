using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEditor;
using System.IO;
using MySql.Data.MySqlClient;
using System;

namespace Asset.MySql
{
    public enum ESocialStatus
    {
        None,
        Request,
        Friend,
        Block,
        Denied,
        Max
    }

    public static class MySqlStatement
    {
        private const string INSERT_ACCOUNT = "INSERT INTO AccountDB (Email,Password,Nickname) VALUES ";
        private const string INSERT_CHARACTER = "INSERT INTO CharacterDB (Nickname,Gender,AvatarData) VALUES ";
        private const string INSERT_RELATIONSHIP = "INSERT INTO RelationshipDB (UserA,UserB,State) VALUES ";
        public static readonly string[] INSERT =
        {
            INSERT_ACCOUNT,
            INSERT_CHARACTER,
            INSERT_RELATIONSHIP
        };
        public const string SET_ENUM = "SHOW TABLES;\nDESC ";
        public const string SELECT = "SELECT * from ";
        public const string UPDATE_RELATIONSHIP = "UPDATE RelationshipDB SET State = ";
    }

    public class MySqlSetting
    {
        private static bool _hasInit = false;
        private static string _selectJsonstring;
        private static string _initInsertAvatarstring;
        private static string _resultAvatarData;
        private static string _connectionString;
        [Obsolete]
        private static string _insertSocialRequestString;

        /// <summary>
        ///  MySql 세팅 초기화
        /// </summary>
        public static void Init()
        {
            if (_hasInit)
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
            _initInsertAvatarstring = Resources.Load<TextAsset>("initAvatar").text;
            // _insertSocialRequestString = Resources.Load<TextAsset>("InsertRequest").text;

            SetEnum();
            Debug.Log("Enum Setting 끝");
        }

        [MenuItem("Tools/GenerateEnum")]
        private static void SetEnum()
        {
            string tableTypeString = MySqlStatement.SET_ENUM.Split('\n')[0];
            string columnTypeString = MySqlStatement.SET_ENUM.Split('\n')[1];

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
                    streamWriter.WriteLine($"\t\tMax,");
                    streamWriter.WriteLine("\t}");

                    //  2. 테이블의 컬럼 타입
                    foreach (string table in tableNames)
                    {
                        streamWriter.WriteLine($"\tpublic enum E{table}Columns {{");

                        foreach (string column in columNames[table])
                        {
                            streamWriter.WriteLine($"\t\t{column},");
                        }
                        streamWriter.WriteLine($"\t\tMax,");
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

    #region Add
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
                if (HasValue(EaccountdbColumns.Email, Email))
                {
                    throw new System.Exception("Email 중복됨");
                }
                if (HasValue(EaccountdbColumns.Nickname, Nickname))
                {
                    throw new System.Exception("Nickname 중복됨");
                }


                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    string _insertAccountString = GetInsertString(ETableType.accountdb, Nickname, Password, Email);
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
                    string _insertCharacterString = GetInsertString(ETableType.characterdb, nickname, gender, _initInsertAvatarstring);

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
            string insertString = MySqlStatement.INSERT[(int)tableType] + '(';

            foreach (string value in values)
            {
                insertString += $"'{value}',";
            }

            insertString = insertString.TrimEnd(',') + ");";

            return insertString;
        }
        #endregion

        #region Request
        /// <summary>
        /// 두 사용자 간의 요청이 RequestDB에 존재하는 지 확인.
        /// </summary>
        /// <param name="userA"></param>
        /// <param name="userB"></param>
        /// <returns> 존재하면 true 아니면 false </returns>
        [Obsolete]
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

        [Obsolete]
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
        #endregion

    #region Relationship
        public const byte _FRIEND_BIT = 0b_0000;
        public const byte _REQUEST_LEFT_BIT = 0b_0100;
        public const byte _REQUEST_RIGHT_BIT = 0b_0001;
        public const byte _BLOCK_LEFT_BIT = 0b_1000;
        public const byte _BLOCK_RIGHT_BIT = 0b_0010;
        public const byte _RESET_RIGHT_BIT = 0b_1100;
        public const byte _RESET_LEFT_BIT = 0b_0011;
        public const byte _ERROR_BIT = 0b_1111;

        /*
         * Relationship Bit
         * 
         * 유저 A와 유저 B의 관계가 생기면, 
         * A와 B의 관계 상태가 Relationship Bit 형태로 RelationshipDB에 저장된다.
         * 
         * Relationship Bit는 4비트로 구성된다.
         * 
         * _ _ _ _ : [A → B 친구 요청 여부] [A → B 차단 여부] [A ← B 친구 요청 여부] [A ← B 차단 여부]
         * 
        */



        /// <summary>
        /// 유저간의 관계가 존재하는 지 확인함.
        /// </summary>
        /// <param name="userA"> UserA Column에 들어가는 유저 닉네임. </param>
        /// <param name="userB"> UserB Column에 들어가는 유저 닉네임. </param>
        /// <returns> 존재하면 true, 존재하지 않는다면 false 반환. </returns>
        private static bool IsThereRelationship(string userA, string userB)
        {
            using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
            {
                string selcetSocialRequestString = SelectDBHelper(ETableType.relationshipdb) + $"where UserA = '{userA}' and UserB = '{userB}' " +
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
        public static bool CheckMyPositionInRelationShip(string myNickname, string targetNickname, out bool isLeft)
        {

            using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
            {
                string selcetSocialRequestString = SelectDBHelper(ETableType.relationshipdb) + $" where UserA = '{myNickname}' and UserB = '{targetNickname}' " +
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
        public static int CheckRelationship(string myNickname, string targetNickname, out bool isLeft)
        {
            if (CheckMyPositionInRelationShip(myNickname, targetNickname, out isLeft) == false)
            {
                return -1;
            }

            CheckMyPositionInRelationShip(myNickname, targetNickname, out isLeft);

            using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
            {
                int state = _ERROR_BIT;

                string selcetSocialRequestString = SelectDBHelper(ETableType.relationshipdb) + $" where UserA = '{myNickname}' and UserB = '{targetNickname}' " +
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

        /// <summary>
        /// 연산된 State를 받아, RelationshipDB의 State를 업데이트 함. DB에 관계가 없다면, 새로 추가함.
        /// </summary>
        /// <param name="userA"> UserA column에 들어가는 닉네임. </param>
        /// <param name="userB"> UserB column에 들어가는 닉네임. </param>
        /// <param name="state">업데이트할 State </param>
        /// <returns>성공하면 true, 실패하면 false를 반환함. </returns>
        public static bool UpdateRelationship(string userA, string userB, int state)
        {
            if (IsThereRelationship(userA, userB) == false)
            {
                try
                {
                    using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                    {
                        string insertSocialStateString = GetInsertString(ETableType.relationshipdb, userA, userB, state.ToString());

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

                    string updateSocialStateString = MySqlStatement.UPDATE_RELATIONSHIP + $"{state} where UserA = '{userA}' and UserB = '{userB}' " +
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
        public static bool UpdateRelationshipToBlock(string myNickname, string targetNickname)
        {
            try
            {
                int state = CheckRelationship(myNickname, targetNickname, out bool isLeft);

                state = UpdateRelationshipToBlockHelper(isLeft, state);

                UpdateRelationship(myNickname, targetNickname, state);

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
        public static bool UpdateRelationshipToUnblock(string myNickname, string targetNickname)
        {
            try
            {
                int state = CheckRelationship(myNickname, targetNickname, out bool isLeft);

                state = UpdateRelationshipToResetHelper(isLeft, state);

                UpdateRelationship(myNickname, targetNickname, state);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 내가 상대에게 친구 요청을 한다.
        /// 사전조건 판단은 외부에서 수행한다.
        /// </summary>
        /// <param name="myNickname"></param>
        /// <param name="targetNickname"></param>
        /// <returns></returns>
        public static bool UpdateRelationshipToRequest(string myNickname, string targetNickname)
        {
            try
            {
                bool isLeft;
                int state = CheckRelationship(myNickname, targetNickname, out isLeft);

                state = UpdateRelationshipToRequestHelper(isLeft, state);

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
        /// 상대에게서 온 친구 요청을 삭제한다.
        /// 차단 해제와 동일하게 동작하므로, Unblock함수를 불러오게끔 만들었다.
        /// </summary>
        /// <param name="myNickname"></param>
        /// <param name="targetNickname"></param>
        /// <returns></returns>
        public static bool UpdateRelationshipToUnrequest(string myNickname, string targetNickname)
        {
            return UpdateRelationshipToUnblock(myNickname,targetNickname);
        }


        /// <summary>
        /// 나와 상대의 관계를 친구로 업데이트한다.
        /// 사전조건 판단은 외부에서 수행한다.
        /// </summary>
        /// <param name="myNickname"></param>
        /// <param name="targetNickname"></param>
        /// <returns></returns>
        public static bool UpdateRelationshipToFriend(string myNickname, string targetNickname)
        {
            try
            {
                UpdateRelationship(myNickname, targetNickname, _FRIEND_BIT);
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
        public static bool UpdateRelationshipToUnFriend(string myNickname, string targetNickname)
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

        private static int UpdateRelationshipToResetHelper(bool isLeft, int state)
        {
            int resetBit = isLeft ? _RESET_LEFT_BIT : _RESET_RIGHT_BIT;
            return state | resetBit;
        }
        
        private static int UpdateRelationshipToBlockHelper(bool isLeft, int state)
        {
            state = UpdateRelationshipToResetHelper(isLeft, state);

            int blockBit = isLeft ? _BLOCK_LEFT_BIT : _BLOCK_RIGHT_BIT;
            return state | blockBit;
        }

        private static int UpdateRelationshipToRequestHelper(bool isLeft, int state)
        {
            state = UpdateRelationshipToResetHelper(isLeft, state);

            int requestBit = isLeft ? _REQUEST_LEFT_BIT : _REQUEST_RIGHT_BIT;
            return state | requestBit;
        }
    #endregion

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
        /// 유저의 닉네임을 받아 특정 State의 리스트를 가져옴.
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> GetRelationList(string nickname)
        {
            List<Dictionary<string, string>> resultList = new List<Dictionary<string, string>>();

            // UserA 칼럼에 대한 State 검사 후 리스트 생성
            GetRelationListHelper
            (
                ErelationshipdbColumns.UserA,
                ErelationshipdbColumns.UserB,
                nickname,
                ref resultList
            ) ;

            // UserB 칼럼에 대한 State 검사 후 리스트 생성
            GetRelationListHelper
            (
                ErelationshipdbColumns.UserB,
                ErelationshipdbColumns.UserA,
                nickname,
                ref resultList
            );

            return resultList;
        }

        private static void GetRelationListHelper(ErelationshipdbColumns userA, ErelationshipdbColumns userB, string nickname, ref List<Dictionary<string, string>> resultList)
        {
            string selcetUserAString = $"SELECT RelationshipDB.{userB}, RelationshipDB.State " +
                $"FROM RelationshipDB WHERE {userA} = '{nickname}'; ";

            bool isLeft;

            if (userA == ErelationshipdbColumns.UserA)
            {
                isLeft = true;
            }
            else
            {
                isLeft = false;
            }

            DataSet userAData = GetUserData(selcetUserAString);

            foreach (DataRow _dataRow in userAData.Tables[0].Rows)
            {
                Dictionary<string, string> dictionaryList = new Dictionary<string, string>();
                dictionaryList.Add("Nickname", _dataRow[userB.ToString()].ToString());
                dictionaryList.Add("State", _dataRow[ErelationshipdbColumns.State.ToString()].ToString());
                dictionaryList.Add("IsLeft", isLeft.ToString());

                resultList.Add(dictionaryList);
            }
        }

        /// <summary>
        /// 해당 값이 DB에 있는지 확인한다.
        /// </summary>
        /// <param name="columnType">Account 태이블에서 비교하기 위한 colum 명</param>
        /// <param name="value">비교할 값</param>
        /// <returns>값이 있다면 true, 아니면 false를 반환한다.</returns>
        public static bool HasValue(EaccountdbColumns columnType, string value)
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    bool result = false;

                    string selectString = SelectDBHelper(ETableType.accountdb) + ETableType.accountdb + $" WHERE {columnType} = '{value}';";

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

        private static string SelectDBHelper(ETableType db)
        {
            return MySqlStatement.SELECT + db.ToString();
        }

    #region ValueByBase
        /// <summary>
        /// CharacterDB Table에서 baseType의 baseValue를 기준으로 checkType의 checkValue가 일치하는지 확인함
        /// </summary>
        /// <param name="baseType">기준 데이터 Column 타입</param>
        /// <param name="baseValue">기준 데이터 값</param>
        /// <param name="checkType">확인할 데이터 Column 타입</param>
        /// <param name="checkValue">확인할 값</param>
        /// <returns>일치하면 true를 반환, 아니거나 오류가 있을 경우 false 반환</returns>
        public static bool CheckValueByBase(EcharacterdbColumns baseType, string baseValue,
            EcharacterdbColumns checkType, string checkValue)
        {
            return CheckValueByBase(ETableType.characterdb, baseType, baseValue, checkType, checkValue);
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
        public static bool CheckValueByBase(EaccountdbColumns baseType, string baseValue,
            EaccountdbColumns checkType, string checkValue)
        {
            return CheckValueByBase(ETableType.accountdb, baseType, baseValue, checkType, checkValue);
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
        /// CharacterDB 테이블에서 baseType의 baseValue를 기준으로 targetType의 데이터를 가져옴
        /// </summary>
        /// <param name="baseType">기준이 되는 값의 Column명</param>
        /// <param name="baseValue">기준이 되는 데이터</param>
        /// <param name="targetType">가져오기 위한 데이터 Column명</param>
        /// <returns>해당 데이터를 반환. 오류 시 null 반환</returns>
        public static string GetValueByBase(EcharacterdbColumns baseType, string baseValue, EcharacterdbColumns targetType)
        {
            return GetValueByBase(ETableType.characterdb, baseType, baseValue, targetType);
        }

        /// <summary>
        /// AccountDB 테이블에서 baseType의 baseValue를 기준으로 targetType의 데이터를 가져옴
        /// </summary>
        /// <param name="baseType">기준이 되는 값의 Column명</param>
        /// <param name="baseValue">기준이 되는 데이터</param>
        /// <param name="targetType">가져오기 위한 데이터 Column명</param>
        /// <returns>해당 데이터를 반환. 오류 시 null 반환</returns>
        public static string GetValueByBase(EaccountdbColumns baseType, string baseValue, EaccountdbColumns targetType)
        {
            return GetValueByBase(ETableType.accountdb, baseType, baseValue, targetType);
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

        public static bool UpdateValueByBase(EcharacterdbColumns baseType, string baseValue,
           EcharacterdbColumns targetType, string targetValue)
        {
            return UpdateValueByBase(ETableType.characterdb, baseType, baseValue, targetType, targetValue);
        }
        /// <summary>
        /// AccountDB Table에서 baseType의 baseValue를 기준으로 TargetType을 TargetValue로 변경함
        /// </summary>
        /// <param name="baseType">기준 값의 Column명</param>
        /// <param name="baseValue">기준 값의 데이터</param>
        /// <param name="targetType">변경할 값의 Column명</param>
        /// <param name="targetValue">변경할 값</param>
        /// <returns>정상적으로 변경되었다면 true, 아니면 false를 반환</returns>
        public static bool UpdateValueByBase(EaccountdbColumns baseType, string baseValue,
            EaccountdbColumns targetType, string targetValue)
        {
            return UpdateValueByBase(ETableType.accountdb, baseType, baseValue, targetType, targetValue);
        }

        public static bool UpdateValueByBase(EaccountdbColumns baseType, string baseValue,
            EaccountdbColumns targetType, int targetValue)
        {
            return UpdateValueByBase(ETableType.accountdb, baseType, baseValue, targetType, targetValue);
        }

        public static bool UpdateValueByBase(EcharacterdbColumns baseType, string baseValue,
            EcharacterdbColumns targetType, int targetValue)
        {
            return UpdateValueByBase(ETableType.characterdb, baseType, baseValue, targetType, targetValue);
        }
        private static bool UpdateValueByBase<T>(ETableType targetTable,
            T baseType, string baseValue,
            T targetType, int targetValue) where T : System.Enum
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string updateString = $"Update {targetTable} set {targetType} = '{targetValue}' where {baseType} = '{baseValue}';";
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

        private static bool UpdateValueByBase<T1, T2>(ETableType targetTable,
            T1 baseType, string baseValue,
            T1 targetType, T2 targetValue) where T1 : System.Enum
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string updateString = $"Update {targetTable} SET {targetType} = '{targetValue}' WHERE {baseType} = '{baseValue}';";
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
        #endregion

        #region DeleteRowByComparator
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
          string logicOperator = "AND")
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
                    string deleteString = $"DELETE FROM {targetTable} ";

                    for (int i = 0; i < comparators.Length - 1; ++i)
                    {
                        deleteString += $"WHERE {comparators[i].Column} = '{comparators[i].Value}' {logicOperator} ";
                    }

                    deleteString += $"WHERE {comparators[comparators.Length - 1].Column} = '{comparators[comparators.Length - 1].Value}';";

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
    #endregion


        
    }

}
