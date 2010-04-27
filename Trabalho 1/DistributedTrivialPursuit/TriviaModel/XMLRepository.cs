using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace TriviaModel
{
    internal struct DataObject
    {
        internal readonly String question;
        internal readonly String answer;

        public DataObject(String question, String answer)
        {
            this.answer = answer;
            this.question = question;
        }
    }

    class XMLRepository : IRepository
    {

        private static IRepository _current;
        private static Dictionary<String, List<DataObject>> _catalog;

        public static IRepository GetInstance(String path)
        {
            if (_current == null)
                _current = new XMLRepository(path);
            return _current;
        }

        private XMLRepository(String path)
        {
            _catalog = new Dictionary<string, List<DataObject>>();

            Encoding enc = Encoding.GetEncoding("iso-8859-1");
            StreamReader file = new StreamReader(path, enc);
            String content = file.ReadToEnd();
            file.Close();

            XmlDocument data = new XmlDocument();
            data.LoadXml(content);
            EagerLoad(data);
            data = null;
        }

        private void EagerLoad(XmlDocument doc)
        {
            DataObject temp;
            List<DataObject> values;

            foreach (XmlNode theme in doc.SelectNodes("root/theme/@name"))
            {
                values = new List<DataObject>();
                foreach (XmlNode card in doc.SelectNodes(
                    String.Format("//theme[@name=\"{0}\"]/card",theme.Value)))
                {
                    temp = new DataObject(
                            card.FirstChild.Attributes["text"].Value,
                            card.LastChild.Attributes["text"].Value
                        );
                    values.Add(temp);
                }
                _catalog.Add(theme.Value, values);
            }
        }

        #region IRepository Members

        public List<string> GetThemes() { return _catalog.Keys.ToList(); }

        public string GetAnswer(List<string> keyWords, string theme)
        {
            try
            {
                return _catalog[theme].First(
                o => IsQuestion(keyWords, o.question)
                ).answer;
            }
            catch (InvalidOperationException)
            {
                return "I haven't got the answer for that!";
            }
        }

        private Boolean IsQuestion(List<string> keyWords, string question)
        {
            foreach (string k in keyWords)
            {
                if (!question.Contains(k))
                    return false;
            }
            return true;
        }

        #endregion
    }
}
