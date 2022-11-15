using System.Collections.Generic;
using UnityEngine;

namespace Asset.ParseCSV
{
    public class CSVParser
    {
        /// <summary>
        /// Resources ������ �ִ� fileName�� ������ �о��, 
        /// ������ lineSeparater�� fieldSeparater�� �����͸� ������ �Ľ��� �Ѵ�.
        /// 
        /// ����Ʈ lineSeparater = '\n', ����Ʈ fieldSeparater = ','
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="lineSeparater"></param>
        /// <param name="fieldSeparater"></param>
        /// <returns>
        /// List<Dictionary<string, string>> Ÿ������ ��ȯ, 
        /// �ʵ���� Ű�� ������ ����
        /// </returns>
        public static List<Dictionary<string, string>> ParseCSV(string fileName, char lineSeparater = '\n', char fieldSeparater = ',')
        {
            TextAsset csvFile = Resources.Load(fileName) as TextAsset;
            
            var container = new List<Dictionary<string, string>>();

            string[] lines = csvFile.text.Split(lineSeparater);
            string[] fieldName = lines[0].Split(fieldSeparater);

            for (int i = 1; i < lines.Length - 1; ++i)
                // lines.Length - 1 : CSV ���� ������ ���� �� ���� �ѱ�
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
