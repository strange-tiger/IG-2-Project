using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asset.ParseCSV
{
    public class CSVParser
    {
        /// <summary>
        /// Resources 폴더에 있는 fileName의 파일을 읽어내어, 
        /// 지정한 lineSeparater와 fieldSeparater로 데이터를 나누어 파싱을 한다.
        /// 
        /// 디폴트 lineSeparater = '\n', 디폴트 fieldSeparater = ','
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="lineSeparater"></param>
        /// <param name="fieldSeparater"></param>
        /// <returns>
        /// List<Dictionary<string, string>> 타입으로 반환, 
        /// 필드명을 키로 접근이 가능
        /// </returns>
        public static List<Dictionary<string, string>> ParseCSV(string fileName, char lineSeparater = '\n', char fieldSeparater = ',')
        {
            TextAsset csvFile = Resources.Load(fileName) as TextAsset;

            var container = new List<Dictionary<string, string>>();

            string[] lines = csvFile.text.Split(lineSeparater);
            string[] fieldName = lines[0].TrimEnd('\r').Split(fieldSeparater);
            string[] fields = new string[fieldName.Length];

            for (int i = 0; i < lines.Length - 2; ++i)
            // lines.Length - 2 : CSV 파일 첫 줄과 마지막 공백 줄을 넘김
            {
                fields = lines[i + 1].TrimEnd('\r').Split(fieldSeparater);

                var field = new Dictionary<string, string>();
                for (int j = 0; j < fields.Length; ++j)
                {
                    field.Add(fieldName[j], fields[j]);
                }

                container.Add(field);
            }

            return container;
        }

        public static PetShopList ParseCSV(string fileName, PetShopList petShopList, char lineSeparater = '\n', char fieldSeparater = ',')
        {
            TextAsset csvFile = Resources.Load(fileName) as TextAsset;

            //var container = new List<Dictionary<string, string>>();

            string[] lines = csvFile.text.Split(lineSeparater);
            string[] fieldName = lines[0].Split(fieldSeparater);
            string[] fields = new string[fieldName.Length];

            Debug.Log("필드" + fieldName.Length);

            for (int i = 0; i < lines.Length - 2; ++i)
            // lines.Length - 2 : CSV 파일 첫 줄과 마지막 공백 줄을 넘김
            {
                fields = lines[i + 1].Split(fieldSeparater);

                petShopList.Name[i] = fields[1];
                petShopList.Price[i] = int.Parse(fields[2]);
                petShopList.Grade[i] = (PetShopUIManager.PetProfile.EGrade)Enum.Parse(typeof(PetShopUIManager.PetProfile.EGrade), fields[3]);
                petShopList.Explanation[i] = fields[4];
            }

            return petShopList;
        }

        public static StartRoomQuestList ParseCSV(string fileName, StartRoomQuestList startRoomQuestList, char lineSeparater = '\n', char fieldSeparater = ',')
        {
            TextAsset csvFile = Resources.Load(fileName) as TextAsset;

            //var container = new List<Dictionary<string, string>>();

            string[] lines = csvFile.text.Split(lineSeparater);
            string[] fieldName = lines[0].Split(fieldSeparater);
            string[] fields = new string[fieldName.Length];

            Debug.Log("필드" + fieldName.Length);

            for (int i = 0; i < lines.Length - 2; ++i)
            // lines.Length - 2 : CSV 파일 첫 줄과 마지막 공백 줄을 넘김
            {
                fields = lines[i + 1].Split(fieldSeparater);

                startRoomQuestList.Dialogue[i] = fields[2];
                //startRoomQuestList.IsQuest[i] = int.Parse(fields[5]);
            }

            return startRoomQuestList;
        }

        public static Lobby1QuestList ParseCSV(string fileName, Lobby1QuestList lobby1QuestList, char lineSeparater = '\n', char fieldSeparater = '@')
        {
            TextAsset csvFile = Resources.Load(fileName) as TextAsset;

            //var container = new List<Dictionary<string, string>>();

            string[] lines = csvFile.text.Split(lineSeparater);
            string[] fieldName = lines[0].Split(fieldSeparater);
            string[] fields = new string[fieldName.Length];

            for (int i = 0; i < lines.Length - 2; ++i)
            // lines.Length - 2 : CSV 파일 첫 줄과 마지막 공백 줄을 넘김
            {
                fields = lines[i + 1].Split(fieldSeparater);

                lobby1QuestList.Dialogue[i] = fields[2];
                lobby1QuestList.IsQuest[i] = fields[5];
            }

            return lobby1QuestList;
        }

        public static List<string> ParseCSV(string fileName, List<string> conversationList, char lineSeparater = '\n', char fieldSeparater = '@')
        {
            TextAsset csvFile = Resources.Load(fileName) as TextAsset;

            //var container = new List<Dictionary<string, string>>();

            string[] lines = csvFile.text.Split(lineSeparater);
            string[] fieldName = lines[0].Split(fieldSeparater);
            string[] fields = new string[fieldName.Length];

            for (int i = 0; i < lines.Length - 2; ++i)
            // lines.Length - 2 : CSV 파일 첫 줄과 마지막 공백 줄을 넘김
            {
                fields = lines[i + 1].Split(fieldSeparater);

                conversationList.Add(fields[2]);

            }

            return conversationList;
        }

        public static void ParseCSV(string fileName, ref List<string> conversationList, char lineSeparater = '\n', char fieldSeparater = ',')
        {
            TextAsset csvFile = Resources.Load(fileName) as TextAsset;

            //var container = new List<Dictionary<string, string>>();

            string[] lines = csvFile.text.Split(lineSeparater);
            string[] fieldName = lines[0].Split(fieldSeparater);
            string[] fields = new string[fieldName.Length];

            Debug.Log("필드" + fieldName.Length);

            for (int i = 0; i < lines.Length - 2; ++i)
            // lines.Length - 2 : CSV 파일 첫 줄과 마지막 공백 줄을 넘김
            {
                fields = lines[i + 1].Split(fieldSeparater);

                conversationList.Add(fields[2]);

            }
        }
    }
}
