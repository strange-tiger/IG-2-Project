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
            string[] fieldName = lines[0].Split(fieldSeparater);

            for (int i = 1; i < lines.Length - 1; ++i)
                // lines.Length - 1 : CSV 파일 마지막 공백 한 줄을 넘김
            {
                string[] fields = lines[i].Split(fieldSeparater);

                var field = new Dictionary<string, string>();
                for (int j = 0; j < fields.Length; ++j)
                {
                    field.Add(fieldName[j], fields[j]);
                }

                container.Add(field);
            }

            return container;
        }
    }
}
