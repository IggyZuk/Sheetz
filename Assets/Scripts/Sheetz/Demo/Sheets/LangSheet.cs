using System;
using System.Collections.Generic;

namespace Sheetz.Demo
{
    public class LangSheet : Sheet
    {
        public LangSheet() :
            base(
                Documents.Database,
                1834096039,
                Format.TSV,
                "lang.txt")
        { }

        public void Download(Action<Dictionary<string, Model.Language>> onSuccess)
        {
            base.Download(_ => onSuccess(Parse()));
        }

        public Dictionary<string, Model.Language> Parse()
        {
            Dictionary<string, Model.Language> languages = new Dictionary<string, Model.Language>();

            string[] rows = Rows();
            string[] langNames = rows[0].Split('\t');

            for (int i = 1; i < langNames.Length; i++)
            {
                Model.Language lang = new Model.Language();
                lang.name = langNames[i];

                languages.Add(lang.name, lang);
            }

            for (int i = 1; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split('\t');

                string id = columns[0];

                for (int j = 1; j < columns.Length; j++)
                {
                    Model.Word word = new Model.Word();
                    word.id = id;
                    word.text = columns[j];

                    languages[langNames[j]].words.Add(word.id, word);
                }
            }

            return languages;
        }
    }
}