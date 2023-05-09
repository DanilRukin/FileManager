using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models.Services.FileRules
{
    public class TranslateRule : DefaultTranslateRule
    {
        private string _fileName;

        public TranslateRule(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Возвращает транслирующую таблицу. Формат файла:
        /// 'ключ=значение' в столбец без кавычек
        /// </summary>
        /// <returns></returns>
        protected override Dictionary<string, string> GetTranslationTable()
        {
            if (_fileName == null)
            {
                return GetDefaultTable();
            }
            else
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                StreamReader reader = new StreamReader(_fileName);
                string[] reports = reader.ReadToEnd().Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                string[] keyValue;
                try
                {
                    for (int i = 0; i < reports.Length; i++)
                    {
                        keyValue = reports[i].Split("=", StringSplitOptions.RemoveEmptyEntries);
                        result.Add(keyValue[0], keyValue[1]);
                    }
                }
                catch (Exception)
                {
                    reader.Close();
                    throw new InvalidDataException($"Неверный формат данных в файле {_fileName}");
                }
                reader.Close();
                return result;
            }
        }
    }
}
