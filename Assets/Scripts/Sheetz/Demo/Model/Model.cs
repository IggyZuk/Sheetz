using System.Collections.Generic;

namespace Sheetz.Demo.Model
{
    public class Version
    {
        public string build;
        public string sheets;

        public static bool operator >(Version left, Version right) => left.CompareTo(right) > 0;
        public static bool operator <(Version left, Version right) => left.CompareTo(right) < 0;
        public static bool operator >=(Version left, Version right) => left.CompareTo(right) >= 0;
        public static bool operator <=(Version left, Version right) => left.CompareTo(right) <= 0;

        public int CompareTo(Version other)
        {
            var v1 = new System.Version(build);
            var v2 = new System.Version(other.build);

            return v1.CompareTo(v2);
        }

        public override string ToString()
        {
            return $"Build: ({build}), Sheets: ({sheets})";
        }
    }

    public class Character
    {
        public string id;
        public int damage;
        public int health;
        public string weapon;
        public string power;

        public override string ToString()
        {
            return $"ID:{id}, Damage:{damage}, Health:{health}, Weapon:{weapon}, Power:{power}";
        }
    }

    public class Word
    {
        public string id;
        public string text;

        public override string ToString()
        {
            return $"ID:{id}, Text:{text}";
        }
    }

    public class Language
    {
        public string name;
        public Dictionary<string, Word> words = new Dictionary<string, Word>();

        public override string ToString()
        {
            string result = name + ":\n";

            foreach (Word word in words.Values)
            {
                result += word + "\n";
            }

            return result;
        }
    }

    public class State
    {
        public Dictionary<string, Character> characters = new Dictionary<string, Character>();
        public Dictionary<string, Language> languages = new Dictionary<string, Language>();

        public override string ToString()
        {
            string result = string.Empty;

            result += "Characters:\n";
            foreach (Character character in characters.Values)
            {
                result += $"{character}\n";
            }

            result += "\n";

            result += "Languages:\n";
            foreach (Language lang in languages.Values)
            {
                result += $"{lang}\n";
            }

            return result;
        }
    }
}