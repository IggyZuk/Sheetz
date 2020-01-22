using System;
using System.Collections.Generic;

namespace Sheetz.Demo
{
    public class CharactersSheet : Sheet
    {
        public CharactersSheet() :
            base(
                Documents.Database,
                0,
                Format.TSV,
                "characters.txt"
            )
        { }

        public void Download(Action<Dictionary<string, Model.Character>> onSuccess)
        {
            base.Download(_ => onSuccess(Parse()));
        }

        public Dictionary<string, Model.Character> Parse()
        {
            Dictionary<string, Model.Character> characters = new Dictionary<string, Model.Character>();

            string[] rows = Rows();

            for (int i = 1; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split('\t');

                Model.Character character = new Model.Character();
                character.id = columns[0];
                character.damage = int.Parse(columns[1]);
                character.health = int.Parse(columns[2]);
                character.weapon = columns[3];
                character.power = columns[4];

                characters.Add(character.id, character);
            }

            return characters;
        }
    }
}