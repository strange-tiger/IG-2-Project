//#define _DEV_MODE_
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System.IO;
using MySql.Data.MySqlClient;
using System;

namespace Asset.MySql
{
    public enum ETutorialCompleteState
    {
        NONE = 0b_0000,
        STARTROOM = 0b_1000,
        LOBBYONE = 0b_0100,
        LOBBYTWO = 0b_0010,
        ARENA = 0b_0001,
        Max
    }

    public static class MySqlStatement
    {
        private const string INSERT_ACCOUNT = "INSERT INTO AccountDB (ID,Password,Nickname,Question,Answer) VALUES ";
        private const string INSERT_CHARACTER = "INSERT INTO CharacterDB (Nickname,Gender) VALUES ";
        private const string INSERT_RELATIONSHIP = "INSERT INTO RelationshipDB (UserA,UserB,State) VALUES ";
        private const string INSERT_BETTING = "INSERT INTO BettingDB (Nickname,BettingGold,BettingChampionNumber,HaveGold) VALUES ";
        private const string INSERT_BETTINGAMOUNT = "INSERT INTO BettingDB (Nickname,BettingGold,BettingChampionNumber,HaveGold) VALUES ";
        private const string INSERT_PETINVENTORY = "INSERT INTO PetInventoryDB (Nickname) VALUES ";
        private const string INSERT_ROOMLIST = "INSERT INTO RoomListDB (UserId, Password, DisplayName, RoomNumber) VALUES ";
        public static readonly string[] INSERT =
        {
            INSERT_ACCOUNT,
            INSERT_BETTINGAMOUNT,
            INSERT_BETTING,
            INSERT_CHARACTER,
            INSERT_PETINVENTORY,
            INSERT_RELATIONSHIP,
            INSERT_ROOMLIST,
        };
        public const string SET_ENUM = "SHOW TABLES;\nDESC ";
        public const string SELECT = "SELECT * from ";
        public const string UPDATE_RELATIONSHIP = "UPDATE RelationshipDB SET State = ";
        public const string UPDATE_COMPLETETUTORIAL = "UPDATE CharacterDB SET Tutorial = ";
    }

    public class MySqlSetting
    {
        private static bool _hasInit = false;
        private static string _connectionString;
        [Obsolete]
        private static string _insertSocialRequestString;

        /// <summary>
        ///  MySql ?????? ?????????
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
        /// MySql ????????? ?????????
        /// </summary>
        /// <param name="isNeedReset"> ???????????? ???????????? true, ????????? false</param>
        public static void Init(bool isNeedReset)
        {
            if (!isNeedReset)
            {
                return;
            }

            _connectionString = Resources.Load<TextAsset>("DBText/Connection").text;
            // _insertSocialRequestString = Resources.Load<TextAsset>("InsertRequest").text;

#if _DEV_MODE_
            SetEnum();
#endif
            Debug.Log("Enum Setting ???");
        }

#if _DEV_MODE_
        [MenuItem("Tools/GenerateEnum")]
        private static void SetEnum()
        {
            string tableTypeString = MySqlStatement.SET_ENUM.Split('\n')[0];
            string columnTypeString = MySqlStatement.SET_ENUM.Split('\n')[1];

            List<string> tableNames = new List<string>();
            Dictionary<string, List<string>> columNames = new Dictionary<string, List<string>>();

            try
            {
                // DB?????? ???????????? ????????? ????????????
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    _sqlConnection.Open();

                    // ????????? ??? ????????????
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

                    // ????????? ?????? ?????? Column ??? ????????????
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

                // ?????? ????????? ?????? ?????? ????????????
                using (StreamWriter streamWriter = new StreamWriter("./Assets/_Scripts/Util/MySqlEnum.cs"))
                {
                    // ?????????
                    streamWriter.WriteLine("namespace Asset {");

                    // enum ????????????
                    //  1. ????????? ?????? 
                    streamWriter.WriteLine("\tpublic enum ETableType {");
                    foreach (string table in tableNames)
                    {
                        streamWriter.WriteLine($"\t\t{table},");
                    }
                    streamWriter.WriteLine($"\t\tMax,");
                    streamWriter.WriteLine("\t}");

                    //  2. ???????????? ?????? ??????
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

                    // ?????????
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
#endif

        #region Add
        /// <summary>
        /// ?????? ????????????
        /// </summary>
        /// <param name="Email">?????? Email</param>
        /// <param name="Password">?????? PW</param>
        /// <param name="Nickname">?????? Nickname</param>
        /// <returns>??????????????? ????????? ????????? ?????? true, ????????? false
        /// (??????????????? email Nickname??? ?????? ?????? false ??????)</returns>
        public static bool AddNewAccount(string ID, string Password, string Nickname, int QuestionNum, string Answer)
        {
            try
            {
                if (HasValue(EaccountdbColumns.ID, ID))
                {
                    throw new System.Exception("ID ?????????");
                }
                if (HasValue(EaccountdbColumns.Nickname, Nickname))
                {
                    throw new System.Exception("Nickname ?????????");
                }

                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    string _insertAccountString = GetInsertString(ETableType.accountdb, ID, Password, Nickname, QuestionNum.ToString(), Answer);
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

        /// <summary>
        /// ???????????? ????????? ?????? CharacterDB??? ?????????.
        /// </summary>
        /// <param name="nickname"> ???????????? ?????? ????????? ?????????</param>
        /// <param name="gender">???????????? ??????</param>
        /// <returns> CharacterDB??? ?????? ???????????? true, ????????? false </returns>
        public static bool AddNewCharacter(string nickname, string gender)
        {
            try
            {
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    string _insertCharacterString = GetInsertString(ETableType.characterdb, nickname, gender);

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

        /// <summary>
        /// ????????? ???????????? ???????????? PetInventory??? Row??? ?????????.
        /// </summary>
        /// <param name="nickname"> ????????? ????????? </param>
        /// <returns> ????????? ???????????? true, ????????? false</returns>
        public static bool AddNewPetInventory(string nickname)
        {
            try
            {
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    string _insertPetInventoryString = GetInsertString(ETableType.petinventorydb, nickname);
                    MySqlCommand _insertPetInventoryCommand = new MySqlCommand(_insertPetInventoryString, _mysqlConnection);

                    _mysqlConnection.Open();
                    _insertPetInventoryCommand.ExecuteNonQuery();
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

        /// <summary>
        /// roomlistdb??? ????????? ??? ??????
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="displayName"></param>
        /// <param name="roomNumber"></param>
        /// <returns>?????? ?????? true, ?????? false ??????</returns>
        public static bool AddNewRoomInfo(string userId, string password, string displayName, int roomNumber)
        {
            try
            {
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    string _insertRoomInfoString = GetInsertString(ETableType.roomlistdb, userId, password, displayName, roomNumber.ToString());

                    MySqlCommand _insertRoomInfoCommand = new MySqlCommand(_insertRoomInfoString, _mysqlConnection);

                    _mysqlConnection.Open();
                    _insertRoomInfoCommand.ExecuteNonQuery();
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
        /// ??? ????????? ?????? ????????? RequestDB??? ???????????? ??? ??????.
        /// </summary>
        /// <param name="userA"></param>
        /// <param name="userB"></param>
        /// <returns> ???????????? true ????????? false </returns>
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
        /// ?????? ????????? RequestDB??? ?????????.
        /// </summary>
        /// <param name="requester">?????????</param>
        /// <param name="respondent">?????????</param>
        /// <returns>???????????? true, ???????????? false ??????</returns>
        public static bool RequestSocialInteraction(string requester, string respondent)
        {
            if (CheckRequest(requester, respondent))
            {
                Debug.LogError("?????? ????????? ???????????????.");
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

        #region Tutorial

        /// <summary>
        /// ????????? ???????????? ?????? ????????? 4?????? ???????????? ????????????. ____ => ????????? ???, ?????? 1, ?????? 2, ????????? ????????? ?????????.
        /// </summary>
        /// <param name="myNickname">????????? ?????????</param>
        /// <param name="checkState">????????? ??????????????? ??????</param>
        /// <returns>??????????????? ??????????????? true, ????????? false </returns>
        public static bool CheckCompleteTutorial(string myNickname, ETutorialCompleteState checkState)
        {
            int state = CheckCompleteTutorial(myNickname);

            state = state & (int)checkState;

            return state == (int)checkState;
        }

        private static int CheckCompleteTutorial(string myNickname)
        {
            try
            {
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    int state = (int)ETutorialCompleteState.NONE;

                    string selcetSocialRequestString = SelectDBHelper(ETableType.characterdb) + $" where Nickname = '{myNickname}';";

                    MySqlCommand command = new MySqlCommand(selcetSocialRequestString, _mysqlConnection);

                    _mysqlConnection.Open();

                    MySqlDataReader reader = command.ExecuteReader();


                    if (reader.Read())
                    {
                        state = reader.GetInt32("Tutorial");
                    }

                    _mysqlConnection.Close();


                    return state;
                }
            }
            catch (System.Exception error)
            {
                Debug.LogError("??????!! CheckRelationship?????? ????????? \n" + error.Message);
                return -1;
            }
        }

        /// <summary>
        /// ??????????????? ????????????, ??????????????? DB??? ????????????.
        /// </summary>
        /// <param name="myNickname">????????? ?????????</param>
        /// <param name="state">?????? ????????? ??????????????? ??????</param>
        /// <returns>??????????????? ???????????? true, ????????? false </returns>
        public static bool CompleteTutorial(string myNickname, ETutorialCompleteState state)
        {
            int updateState = (int)state | CheckCompleteTutorial(myNickname);

            try
            {
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {

                    string updateCompleteTutorialString = MySqlStatement.UPDATE_COMPLETETUTORIAL + $"'{updateState}' where Nickname = '{myNickname}';";

                    MySqlCommand updateCompleteTutorialCommand = new MySqlCommand(updateCompleteTutorialString, _mysqlConnection);
                    _mysqlConnection.Open();
                    updateCompleteTutorialCommand.ExecuteNonQuery();
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
         * ?????? A??? ?????? B??? ????????? ?????????, 
         * A??? B??? ?????? ????????? Relationship Bit ????????? RelationshipDB??? ????????????.
         * 
         * Relationship Bit??? 4????????? ????????????.
         * 
         * _ _ _ _ : [A ??? B ?????? ?????? ??????] [A ??? B ?????? ??????] [A ??? B ?????? ?????? ??????] [A ??? B ?????? ??????]
         * 
        */



        /// <summary>
        /// ???????????? ????????? ???????????? ??? ?????????.
        /// </summary>
        /// <param name="userA"> UserA Column??? ???????????? ?????? ?????????. </param>
        /// <param name="userB"> UserB Column??? ???????????? ?????? ?????????. </param>
        /// <returns> ???????????? true, ???????????? ???????????? false ??????. </returns>
        private static bool IsThereRelationship(string userA, string userB)
        {
            using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
            {
                string selcetSocialRequestString = SelectDBHelper(ETableType.relationshipdb) + $" where UserA = '{userA}' and UserB = '{userB}' " +
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
        /// ?????? UserA??????, UserB?????? ?????????.
        /// </summary>
        /// <param name="myNickname"> ?????? ?????????</param>
        /// <param name="targetNickname"> ????????? ????????? </param>
        /// <param name="isLeft">?????? UserA?????? True, UserB?????? false</param>
        /// <returns>Row??? ???????????? True, ???????????? ????????? false </returns>
        public static bool CheckMyPositionInRelationShip(string myNickname, string targetNickname, out bool isLeft)
        {
            try
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
            catch (System.Exception error)
            {
                Debug.LogError("??????!! CheckMyPositionInRelationShip?????? ????????? \n" + error.Message);
                isLeft = false;
                return false;
            }

        }

        /// <summary>
        /// ?????? ?????? ???????????? ????????? ????????????, ?????? ?????? ?????????.
        /// </summary>
        /// <param name="myNickname"> ?????? ????????? </param>
        /// <param name="targetNickname"> ?????? ????????? ????????? </param>
        /// <param name="isLeft"> ?????? UserA Column????????? true, UserB Column????????? false. </param>
        /// <returns> ?????? ?????? ???????????? State??? int??? ?????????. ????????? ???????????? ???????????? -1 ??????. </returns>
        public static int CheckRelationship(string myNickname, string targetNickname, out bool isLeft)
        {
            if (CheckMyPositionInRelationShip(myNickname, targetNickname, out isLeft) == false)
            {
                return -1;
            }

            CheckMyPositionInRelationShip(myNickname, targetNickname, out isLeft);

            try
            {
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
            catch (System.Exception error)
            {
                Debug.LogError("??????!! CheckRelationship?????? ????????? \n" + error.Message);
                return -1;
            }
        }

        /// <summary>
        /// ????????? State??? ??????, RelationshipDB??? State??? ???????????? ???. DB??? ????????? ?????????, ?????? ?????????.
        /// </summary>
        /// <param name="userA"> UserA column??? ???????????? ?????????. </param>
        /// <param name="userB"> UserB column??? ???????????? ?????????. </param>
        /// <param name="state">??????????????? State </param>
        /// <returns>???????????? true, ???????????? false??? ?????????. </returns>
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
            else
            {
                try
                {
                    using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                    {

                        string updateSocialStateString = MySqlStatement.UPDATE_RELATIONSHIP + $"'{state}' where UserA = '{userA}' and UserB = '{userB}' " +
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

        }

        /// <summary>
        /// ?????? ?????? ????????? ???????????? ?????? State??? ?????????.
        /// </summary>
        /// <param name="myNickname"> ?????? ????????? </param>
        /// <param name="targetNickname"> ?????? ????????? ????????? </param>
        /// <returns>???????????? true, ???????????? false??? ?????????. </returns>
        public static bool UpdateRelationshipToBlock(string myNickname, string targetNickname)
        {
            try
            {
                int state = CheckRelationship(myNickname, targetNickname, out bool isLeft);


                if (state != -1)
                {
                    state = UpdateRelationshipToBlockHelper(isLeft, state);

                }
                else
                {
                    state = _BLOCK_LEFT_BIT;
                }

                UpdateRelationship(myNickname, targetNickname, state);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ?????? ?????? ????????? ???????????? ?????? ?????? State??? ?????????.
        /// </summary>
        /// <param name="myNickname"> ?????? ????????? </param>
        /// <param name="targetNickname"> ?????? ????????? ????????? </param>
        /// <returns>???????????? true, ???????????? false??? ?????????. </returns>
        public static bool UpdateRelationshipToUnblock(string myNickname, string targetNickname)
        {
            try
            {
                bool isLeft;

                int state = CheckRelationship(myNickname, targetNickname, out isLeft);

                state = UpdateRelationshipToResetHelper(isLeft, state);

                if (state == 0)
                {
                    if (isLeft)
                    {
                        DeleteRowByComparator
                        (
                            ErelationshipdbColumns.UserA,
                            myNickname,
                            ErelationshipdbColumns.UserB,
                            targetNickname
                        );
                    }
                    else
                    {
                        DeleteRowByComparator
                        (
                            ErelationshipdbColumns.UserA,
                            targetNickname,
                            ErelationshipdbColumns.UserB,
                            myNickname
                        );
                    }
                }
                else
                {
                    UpdateRelationship(myNickname, targetNickname, state);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ?????? ???????????? ?????? ????????? ??????.
        /// ???????????? ????????? ???????????? ????????????.
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
                if (state != -1)
                {
                    state = UpdateRelationshipToRequestHelper(isLeft, state);
                }
                else
                {
                    state = _REQUEST_LEFT_BIT;
                }

                UpdateRelationship(myNickname, targetNickname, state);

                return true;
            }
            catch (System.Exception error)
            {
                Debug.LogError("??????: Fail To Request Social Interaction + \n" + error.Message);
                return false;
            }
        }

        /// <summary>
        /// ??????????????? ??? ?????? ????????? ????????????.
        /// ?????? ????????? ???????????? ???????????????, Unblock????????? ??????????????? ????????????.
        /// </summary>
        /// <param name="myNickname"></param>
        /// <param name="targetNickname"></param>
        /// <returns></returns>
        public static bool UpdateRelationshipToUnrequest(string myNickname, string targetNickname)
        {
            return UpdateRelationshipToUnblock(myNickname, targetNickname);
        }


        /// <summary>
        /// ?????? ????????? ????????? ????????? ??????????????????.
        /// ???????????? ????????? ???????????? ????????????.
        /// </summary>
        /// <param name="myNickname"></param>
        /// <param name="targetNickname"></param>
        /// 
        /// 
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
                Debug.LogError("??????: Fail To Be Friend");
                return false;
            }
        }

        /// <summary>
        /// ?????? ????????? ?????? ????????? ?????????.
        /// ???????????? ????????? ???????????? ????????????.
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
                Debug.LogError("??????: Fail To Undo Friend");
                return false;
            }
        }

        private static int UpdateRelationshipToResetHelper(bool isLeft, int state)
        {
            int resetBit = isLeft ? _RESET_LEFT_BIT : _RESET_RIGHT_BIT;
            return state & resetBit;
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
        /// DataSet??? ???????????? ?????????.
        /// </summary>
        /// <param name="selectString"> ????????? ???????????? ????????? ?????????</param>
        /// <param name="nickname">???????????? ????????? ?????????</param>
        /// <returns>???????????? ????????? DataSet??? ??????</returns>
        private static DataSet GetUserData(string selectString)
        {
            DataSet _dataSet = new DataSet();
            using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
            {
                if (_sqlConnection.State == ConnectionState.Closed)
                {
                    _sqlConnection.Open();
                }

                MySqlDataAdapter _dataAdapter = new MySqlDataAdapter(selectString, _sqlConnection);

                _dataAdapter.Fill(_dataSet);
            }
            return _dataSet;
        }

        #region RelationshipList        
        /// <summary>
        /// ????????? ???????????? ?????? ?????? State??? ???????????? ?????????.
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> GetRelationList(string nickname)
        {
            List<Dictionary<string, string>> resultList = new List<Dictionary<string, string>>();

            // UserA ????????? ?????? State ?????? ??? ????????? ??????
            GetRelationListHelper
            (
                ErelationshipdbColumns.UserA,
                ErelationshipdbColumns.UserB,
                nickname,
                ref resultList
            );

            // UserB ????????? ?????? State ?????? ??? ????????? ??????
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

        #endregion

        #region Betting

        /// <summary>
        /// BettingDB??? ???????????? ????????????, ????????? ???????????? ????????? ????????? ??????????????? ?????? ????????? ????????????. ??????, ?????? ??????????????? ???????????? ????????? ???????????? ?????? ?????? ?????????????????????.
        /// </summary>
        /// <param name="nickname">????????? ????????? ?????????</param>
        /// <param name="betGold">?????? ??????</param>
        /// <param name="championNum"> ????????? ???????????? ????????? ZeroBase</param>
        /// <returns></returns>
        public static bool InsertBetting(string nickname, int betGold, int championNum)
        {

            try
            {
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {

                    int haveGold;


                    haveGold = int.Parse(GetValueByBase(EcharacterdbColumns.Nickname, nickname, EcharacterdbColumns.Gold)) - (int)betGold;


                    string insertBettingString = GetInsertString(ETableType.bettingdb, nickname, betGold.ToString(), championNum.ToString(), haveGold.ToString());

                    MySqlCommand insertBettingCommand = new MySqlCommand(insertBettingString, _mysqlConnection);

                    _mysqlConnection.Open();

                    insertBettingCommand.ExecuteNonQuery();

                    _mysqlConnection.Close();

                }

                return true;
            }
            catch (System.Exception error)
            {
                Debug.LogError("?????????" + error.Message);

                return false;
            }
        }

        /// <summary>
        /// ????????? ??? ???, ????????? CharacterDB ?????? ????????? ???????????? ??????.
        /// </summary>
        /// <param name="nickname">????????? ?????????</param>
        /// <param name="betGold"> ????????? ??? ????????? ???</param>
        /// <returns>???????????? true, ????????? false </returns>
        public static bool UpdateGoldAfterBetting(string nickname, int betGold)
        {

            try
            {
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    int haveGold;

                    haveGold = int.Parse(GetValueByBase(EcharacterdbColumns.Nickname, nickname, EcharacterdbColumns.Gold)) - (int)betGold;

                    string updateCharacterGoldString = $"Update {ETableType.characterdb} set Gold = '{haveGold}' where Nickname = '{nickname}'";

                    MySqlCommand updateCharacterGoldCommand = new MySqlCommand(updateCharacterGoldString, _mysqlConnection);

                    _mysqlConnection.Open();

                    updateCharacterGoldCommand.ExecuteNonQuery();

                    _mysqlConnection.Close();

                }

                return true;
            }
            catch (System.Exception error)
            {
                Debug.LogError("?????????" + error.Message);

                return false;
            }
        }

        /// <summary>
        /// BettingDB?????? ????????? ????????? ????????? ?????????, ????????? ??????????????? ?????? CharacterDB??? ??????????????????.
        /// </summary>
        /// <param name="nickname">????????? ????????? ?????????</param>
        /// <returns>????????? ??????????????? ????????????, BettingDB?????? ????????? ????????? ????????? -1??? ????????????.</returns>
        public static int CancelBetting(string nickname)
        {
            try
            {
                int result = int.Parse(GetValueByBase(EbettingdbColumns.Nickname, nickname, EbettingdbColumns.BettingGold));

                int updateGold = int.Parse(GetValueByBase(EbettingdbColumns.Nickname, nickname, EbettingdbColumns.HaveGold)) + result;

                DeleteRowByComparator(EbettingdbColumns.Nickname, nickname);

                UpdateValueByBase(EcharacterdbColumns.Nickname, nickname, EcharacterdbColumns.Gold, updateGold.ToString());

                return result;
            }
            catch
            {
                return -1;
            }
        }


        public static UnityEvent OnBettingDraw = new UnityEvent();

        private static Dictionary<string, int> winnerListDictionary = new Dictionary<string, int>();

        /// <summary>
        /// DataSet??? BettingDB??? ????????? ????????????, ???????????? ???????????? CharacterDB??? ????????? ????????????, BettingDB??? ????????????. ???????????? ??????, ????????? ?????? ???????????? ?????? ???????????? BettingUI??? ????????????.
        /// ?????????????????? ???????????? ????????? ???????????? DistributeUI??? ?????????.
        /// </summary>
        /// <param name="winChampionNumber"> ????????? ???????????? ????????? </param>
        /// <param name="betAmount"> ??? ?????? ?????? </param>
        /// <param name="championBetAmount"> ????????? ??????????????? ????????? ??? ??????</param>
        /// <param name="isDraw"> ????????? ?????? </param>
        /// <returns> ????????? ????????? ????????? Dictionary??? ??????</returns>
        public static Dictionary<string, int> DistributeBet(int winChampionNumber, int betAmount, int championBetAmount, bool isDraw)
        {
            try
            {
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    winnerListDictionary.Clear();

                    string selectAllBettingData = SelectDBHelper(ETableType.bettingdb) + $" where BettingChampionNumber = '{winChampionNumber}'";

                    string selectDrawBettingData = SelectDBHelper(ETableType.bettingdb);


                    if (isDraw)
                    {
                        DataSet bettingDBdata = GetUserData(selectDrawBettingData);

                        foreach (DataRow _dataRow in bettingDBdata.Tables[0].Rows)
                        {
                            
                            int betGold = (int)double.Parse(_dataRow[EbettingdbColumns.BettingGold.ToString()].ToString());

                            int haveGold = int.Parse(_dataRow["HaveGold"].ToString()) + betGold;

                            string updateString = $"Update {ETableType.characterdb} SET Gold = '{haveGold}' WHERE Nickname = '{_dataRow[EbettingdbColumns.Nickname.ToString()]}';";

                            MySqlCommand command = new MySqlCommand(updateString, _mysqlConnection);

                            _mysqlConnection.Open();

                            command.ExecuteNonQuery();

                            _mysqlConnection.Close();

                        }
                        OnBettingDraw.Invoke();
                    }
                    else
                    {
                        DataSet bettingDBdata = GetUserData(selectAllBettingData);

                        foreach (DataRow _dataRow in bettingDBdata.Tables[0].Rows)
                        {

                            int betGold = Convert.ToInt32(Math.Round(((Convert.ToDouble(betAmount) * (double.Parse(_dataRow[EbettingdbColumns.BettingGold.ToString()].ToString()) / Convert.ToDouble(championBetAmount)))
                                )));

                            winnerListDictionary.Add(_dataRow[EbettingdbColumns.Nickname.ToString()].ToString(), betGold);

                            int haveGold = int.Parse(_dataRow["HaveGold"].ToString()) + betGold;

                            string updateString = $"Update {ETableType.characterdb} SET Gold = '{haveGold}' WHERE Nickname = '{_dataRow[EbettingdbColumns.Nickname.ToString()]}';";

                            MySqlCommand command = new MySqlCommand(updateString, _mysqlConnection);

                            _mysqlConnection.Open();

                            command.ExecuteNonQuery();

                            _mysqlConnection.Close();

                        }

                    }
                }

                return winnerListDictionary;
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return winnerListDictionary;

            }
        }

        /// <summary>
        /// BettingDB??? ????????????. 
        /// </summary>
        /// <returns></returns>
        public static bool ResetBettingDB()
        {
            try
            {
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    string dropBettingDBString = $"Delete from {ETableType.bettingdb};";

                    MySqlCommand dropBettingDBCommand = new MySqlCommand(dropBettingDBString, _mysqlConnection);

                    if (_mysqlConnection.State == ConnectionState.Closed)
                    {
                        _mysqlConnection.Open();
                        dropBettingDBCommand.ExecuteNonQuery();
                        _mysqlConnection.Close();
                    }
                    else
                    {
                        dropBettingDBCommand.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                return false;
            }
        }

        /// <summary>
        /// ??? ?????? ????????? ???????????? ?????? BettingAmountDB??? ?????????.
        /// </summary>
        /// <returns> ??? ???????????? ?????? ??? ??????, ?????? ?????? ????????? List??? ?????? </returns>
        public static List<int> CheckBettingAmount()
        {

            using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
            {
                string selectBettingAmountString = $"Select * from {ETableType.bettingamountdb};";

                DataSet bettingAmount = GetUserData(selectBettingAmountString);

                List<int> resultList = new List<int>();

                foreach (DataRow _dataRow in bettingAmount.Tables[0].Rows)
                {
                    resultList.Add(int.Parse(_dataRow["Amount"].ToString()));
                    resultList.Add(int.Parse(_dataRow["OneAmount"].ToString()));
                    resultList.Add(int.Parse(_dataRow["TwoAmount"].ToString()));
                    resultList.Add(int.Parse(_dataRow["ThreeAmount"].ToString()));
                    resultList.Add(int.Parse(_dataRow["FourAmount"].ToString()));
                }

                return resultList;
            }

        }

        /// <summary>
        /// ????????? ????????????, ??? ???????????? ??? ?????? ??????, ????????? ???????????? ?????? ??? ????????? ???????????????.
        /// ?????? ??? ?????? ?????????, ????????? ????????? BettingManager?????? ???????????? ???????????????.
        /// </summary>
        /// <param name="index">????????? ???????????? ?????????</param>
        /// <param name="amount"> ??????????????? ????????? ????????? ?????? ??????????????? ?????? ??????</param>
        /// <param name="championAmount">??? ????????? ????????? ??????????????? ?????? ??????</param>
        /// <returns></returns>
        public static bool UpdateBettingAmountDB(int index, int amount, int championAmount)
        {
            try
            {
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    string updateBettingAmountString = $"Update {ETableType.bettingamountdb} set Amount = '{amount}',{(ChampionNumber)Enum.Parse(typeof(ChampionNumber), index.ToString())} = '{championAmount}' ;";


                    MySqlCommand updateBettingAmountCommand = new MySqlCommand(updateBettingAmountString, _mysqlConnection);


                    _mysqlConnection.Open();
                    updateBettingAmountCommand.ExecuteNonQuery();
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





        #endregion

        #region GoldSystem

        /// <summary>
        /// ?????? ?????? ????????? ?????????.
        /// </summary>
        /// <param name="nickname"> ????????? ???????????? ????????? ?????????</param>
        /// <returns> ?????? ????????? ?????????. </returns>
        public static int CheckHaveGold(string nickname)
        {
            return int.Parse(GetValueByBase(EcharacterdbColumns.Nickname, nickname, EcharacterdbColumns.Gold));
        }

        /// <summary>
        /// ????????? ????????? ?????? ???????????? ?????? ?????? ???????????????.
        /// </summary>
        /// <param name="nickname">????????? ?????????</param>
        /// <param name="useGold">????????? ????????? ???</param>
        /// <returns>????????? ???????????? ?????? ????????? ????????? False??? ??????.</returns>
        public static bool UseGold(string nickname, int useGold)
        {
            try
            {
                int haveGold = CheckHaveGold(nickname);

                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    if (useGold > haveGold)
                    {
                        Debug.LogError("?????? ?????????.");
                        return false;
                    }

                    UpdateValueByBase(EcharacterdbColumns.Nickname, nickname, EcharacterdbColumns.Gold, (haveGold - useGold));
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ????????? ????????? ?????? ????????? ?????????.
        /// </summary>
        /// <param name="nickname"> ????????? ??? ????????? ?????????. </param>
        /// <param name="earnGold"> ??? ????????? ??? </param>
        /// <returns> ?????? ????????? ????????? false??? ??????. </returns>
        public static bool EarnGold(string nickname, int earnGold)
        {
            int maxGold = 99999999;

            try
            {
                int haveGold = CheckHaveGold(nickname);

                int updateGold = haveGold + earnGold;

                if (maxGold < haveGold)
                {
                    return false;
                }

                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    if (maxGold < updateGold)
                    {
                        updateGold = maxGold;
                    }
                    UpdateValueByBase(EcharacterdbColumns.Nickname, nickname, EcharacterdbColumns.Gold, updateGold);

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region PetInventoryList        

        /// <summary>
        /// ????????? ???????????? PetData??? ????????? ??????????????? ??????????????? ??????, DB??? ?????? ????????? ?????? ??????????????? ??????????????? ???????????? ????????????.
        /// </summary>
        /// <param name="nickname">????????? ?????????</param>
        /// <param name="petData">PetData??? ????????? ??????????????? ????????????</param>
        /// <returns>DB??? ????????? ????????? PetData ??????????????? ??????????????? ??????</returns>
        public static PetData GetPetInventoryData(string nickname, PetData petData)
        {
            string selcetPetInventoryString = $"SELECT * from PetInventoryDB " +
                $"WHERE {EpetinventorydbColumns.Nickname} = '{nickname}'; ";

            using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
            {
                MySqlCommand command = new MySqlCommand(selcetPetInventoryString, _mysqlConnection);

                _mysqlConnection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string[] petStatusArray = reader["PetStatus"].ToString().Split(',');
                    string[] petLevelArray = reader["PetLevel"].ToString().Split(',');
                    string[] petExpArray = reader["PetExp"].ToString().Split(',');
                    string[] petAssetArray = reader["PetAsset"].ToString().Split(',');
                    string[] petSizeArray = reader["PetSize"].ToString().Split(',');

                    for (int i = 0; i < petStatusArray.Length; ++i)
                    {
                        petData.Status[i] = (EPetStatus)Enum.Parse(typeof(EPetStatus), petStatusArray[i]);
                        petData.Level[i] = int.Parse(petLevelArray[i]);
                        petData.Exp[i] = int.Parse(petExpArray[i]);
                        petData.ChildIndex[i] = int.Parse(petAssetArray[i]);
                        petData.Size[i] = float.Parse(petSizeArray[i]);
                    }
                }

                _mysqlConnection.Close();

                return petData;
            }

        }

        /// <summary>
        /// PetData??? ??????????????? ?????????, DB??? ???????????? ??????.
        /// </summary>
        /// <param name="nickname">????????? ?????????</param>
        /// <param name="petData">??????????????? PetData</param>
        /// <returns></returns>
        public static bool UpdatePetInventoryData(string nickname, PetData petData)
        {

            string petStatusString = petData.Status[0].ToString();
            string petLevelString = petData.Level[0].ToString();
            string petExpString = petData.Exp[0].ToString();
            string petAssetString = petData.ChildIndex[0].ToString();
            string petSizeString = petData.Size[0].ToString();

            for (int i = 1; i < petData.Status.Length; ++i)
            {
                petStatusString += ',' + petData.Status[i].ToString();
                petLevelString += ',' + petData.Level[i].ToString();
                petExpString += ',' + petData.Exp[i].ToString();
                petAssetString += ',' + petData.ChildIndex[i].ToString();
                petSizeString += ',' + petData.Size[i].ToString();
            }

            string updateString = $"Update PetInventoryDB set {EpetinventorydbColumns.PetStatus} = '{petStatusString}',{EpetinventorydbColumns.PetLevel} = '{petLevelString}',{EpetinventorydbColumns.PetExp} = '{petExpString}',{EpetinventorydbColumns.PetAsset} = '{petAssetString}',{EpetinventorydbColumns.PetSize} = '{petSizeString}' where {EpetinventorydbColumns.Nickname} = '{nickname}';";

            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {

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



        /// <summary>
        /// ??????????????? AccountDB - IsOline Column??? ???????????? ???/???????????? ????????? ?????????.
        /// </summary>
        /// <param name="nickname">????????? ?????????</param>
        /// <returns>??????????????? true, ????????? false</returns>
        public static bool IsPlayerOnline(string nickname)
        {
            try
            {
                using (MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString))
                {
                    bool isOnOff = false;

                    string selcetOnOffString = SelectDBHelper(ETableType.accountdb) + $" where Nickname = '{nickname}';";

                    MySqlCommand command = new MySqlCommand(selcetOnOffString, _mysqlConnection);

                    _mysqlConnection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        if (reader["IsOnline"].ToString() == "true")
                        {
                            isOnOff = true;
                        }
                        else
                        {
                            isOnOff = false;
                        }
                    }

                    _mysqlConnection.Close();

                    return isOnOff;
                }
            }
            catch (System.Exception error)
            {
                Debug.LogError("?????????: " + error);
                return false;
            }

        }

        /// <summary>
        /// roomlistdb??? ???????????? List<Dictionary>??? ????????? ??????, roomList ??????
        /// </summary>
        /// <returns></returns>
        public static List<Dictionary<string, string>> GetRoomList()
        {
            List<Dictionary<string, string>> resultList = new List<Dictionary<string, string>>();

            string selectString = MySqlStatement.SELECT + $"{ETableType.roomlistdb};";

            DataSet roomData = GetUserData(selectString);
            foreach (DataRow _dataRow in roomData.Tables[0].Rows)
            {
                Dictionary<string, string> dictionaryList = new Dictionary<string, string>();
                dictionaryList.Add("UserID", _dataRow[EroomlistdbColumns.UserID.ToString()].ToString());
                dictionaryList.Add("Password", _dataRow[EroomlistdbColumns.Password.ToString()].ToString());
                dictionaryList.Add("DisplayName", _dataRow[EroomlistdbColumns.DisplayName.ToString()].ToString());
                dictionaryList.Add("RoomNumber", _dataRow[EroomlistdbColumns.RoomNumber.ToString()].ToString());

                resultList.Add(dictionaryList);
            }

            return resultList;
        }

        /// <summary>
        /// ?????? ?????? DB??? ????????? ????????????.
        /// </summary>
        /// <param name="columnType">Account ??????????????? ???????????? ?????? colum ???</param>
        /// <param name="value">????????? ???</param>
        /// <returns>?????? ????????? true, ????????? false??? ????????????.</returns>
        public static bool HasValue(EaccountdbColumns columnType, string value)
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    bool result = false;

                    string selectString = SelectDBHelper(ETableType.accountdb) + $" WHERE {columnType} = '{value}';";

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
                Debug.LogError("?????????: Doublecheck");
                return false;
            }

        }

        private static string SelectDBHelper(ETableType db)
        {

            return MySqlStatement.SELECT + db.ToString();
        }


        /// <summary>
        /// ?????? ?????? BettingDB??? ????????? ????????????.
        /// </summary>
        /// <param name="columnType">Account ??????????????? ???????????? ?????? colum ???</param>
        /// <param name="value">????????? ???</param>
        /// <returns>?????? ????????? true, ????????? false??? ????????????.</returns>
        public static bool HasValue(EbettingdbColumns columnType, string value)
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    bool result = false;

                    string selectString = SelectDBHelper(ETableType.bettingdb) + $" WHERE {columnType} = '{value}';";

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
                Debug.LogError("?????????: Doublecheck");
                return false;
            }

        }

        #region ValueByBase

        /// <summary>
        /// CharacterDB Table?????? baseType??? baseValue??? ???????????? checkType??? checkValue??? ??????????????? ?????????
        /// </summary>
        /// <param name="baseType">?????? ????????? Column ??????</param>
        /// <param name="baseValue">?????? ????????? ???</param>
        /// <param name="checkType">????????? ????????? Column ??????</param>
        /// <param name="checkValue">????????? ???</param>
        /// <returns>???????????? true??? ??????, ???????????? ????????? ?????? ?????? false ??????</returns>
        public static bool CheckValueByBase(EcharacterdbColumns baseType, string baseValue,
            EcharacterdbColumns checkType, string checkValue)
        {
            return CheckValueByBase(ETableType.characterdb, baseType, baseValue, checkType, checkValue);
        }
        /// <summary>
        /// AccountDB Table?????? baseType??? baseValue??? ???????????? checkType??? checkValue??? ??????????????? ?????????
        /// ex. ID(baseType)??? aaa(baseValue)??? ???????????? Password(checkType)??? 123(checkValue)?????? ?????????
        /// </summary>
        /// <param name="baseType">?????? ????????? Column ??????</param>
        /// <param name="baseValue">?????? ????????? ???</param>
        /// <param name="checkType">????????? ????????? Column ??????</param>
        /// <param name="checkValue">????????? ???</param>
        /// <returns>???????????? true??? ??????, ???????????? ????????? ?????? ?????? false ??????</returns>
        public static bool CheckValueByBase(EaccountdbColumns baseType, string baseValue,
            EaccountdbColumns checkType, string checkValue)
        {
            return CheckValueByBase(ETableType.accountdb, baseType, baseValue, checkType, checkValue);
        }
        private static bool CheckValueByBase<T>(ETableType targetTable, T baseType, string baseValue,
            T checkType, string checkValue) where T : System.Enum
        {
            string checkTargetValue = GetValueByBase(targetTable, baseType, baseValue, checkType);
            Debug.Log(checkTargetValue);
            Debug.Log(checkValue);
            if (checkTargetValue != null)
            {
                return checkTargetValue == checkValue;
            }
            else
            {
                return false;
            }
        }


        public static string GetValueByBase(EbettingdbColumns baseType, string baseValue, EbettingdbColumns targetType)
        {
            return GetValueByBase(ETableType.bettingdb, baseType, baseValue, targetType);
        }

        /// <summary>
        /// CharacterDB ??????????????? baseType??? baseValue??? ???????????? targetType??? ???????????? ?????????
        /// </summary>
        /// <param name="baseType">????????? ?????? ?????? Column???</param>
        /// <param name="baseValue">????????? ?????? ?????????</param>
        /// <param name="targetType">???????????? ?????? ????????? Column???</param>
        /// <returns>?????? ???????????? ??????. ?????? ??? null ??????</returns>
        public static string GetValueByBase(EcharacterdbColumns baseType, string baseValue, EcharacterdbColumns targetType)
        {
            return GetValueByBase(ETableType.characterdb, baseType, baseValue, targetType);
        }

        /// <summary>
        /// AccountDB ??????????????? baseType??? baseValue??? ???????????? targetType??? ???????????? ?????????
        /// </summary>
        /// <param name="baseType">????????? ?????? ?????? Column???</param>
        /// <param name="baseValue">????????? ?????? ?????????</param>
        /// <param name="targetType">???????????? ?????? ????????? Column???</param>
        /// <returns>?????? ???????????? ??????. ?????? ??? null ??????</returns>
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
                        throw new System.Exception("base ?????? ??????");
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
        /// AccountDB Table?????? baseType??? baseValue??? ???????????? TargetType??? TargetValue??? ?????????
        /// </summary>
        /// <param name="baseType">?????? ?????? Column???</param>
        /// <param name="baseValue">?????? ?????? ?????????</param>
        /// <param name="targetType">????????? ?????? Column???</param>
        /// <param name="targetValue">????????? ???</param>
        /// <returns>??????????????? ?????????????????? true, ????????? false??? ??????</returns>
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





        #region DeleteRowByComparator-BettingDB

        public static bool DeleteRowByComparator
         (EbettingdbColumns type, string condition, string logicOperator = "and")
        {

            return DeleteRowByComparator
            (
                Asset.ETableType.bettingdb,
                logicOperator,
                new Comparator<EbettingdbColumns>()
                {
                    Column = type,
                    Value = condition
                }

            );
        }

        #endregion

        /// <summary>
        /// roomlistdb??? Row??? ??????
        /// </summary>
        /// <param name="type"></param>
        /// <param name="condition"></param>
        /// <param name="logicOperator"></param>
        /// <returns></returns>
        public static bool DeleteRowByComparator
        (EroomlistdbColumns type, string condition,
        string logicOperator = "and")

        {

            return DeleteRowByComparator
            (
                Asset.ETableType.roomlistdb,
                logicOperator,
                new Comparator<EroomlistdbColumns>()

                {
                    Column = type,
                    Value = condition
                }

            );
        }

        public static bool DeleteRowByComparator<T>
        (Asset.ETableType targetTable, string logicOperator,
        params Comparator<T>[] comparators)
            where T : System.Enum
        {
            try
            {
                using (MySqlConnection _sqlConnection = new MySqlConnection(_connectionString))
                {
                    string deleteString = $"DELETE FROM {targetTable} where ";

                    for (int i = 0; i < comparators.Length - 1; ++i)
                    {
                        deleteString += $"{comparators[i].Column} = '{comparators[i].Value}' {logicOperator} ";
                    }

                    deleteString += $"{comparators[comparators.Length - 1].Column} = '{comparators[comparators.Length - 1].Value}';";

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
