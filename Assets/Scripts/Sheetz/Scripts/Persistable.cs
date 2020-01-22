using System.IO;
using UnityEngine;

namespace Sheetz
{
    public class Persistable
    {
        public readonly string FolderName;
        public readonly string FileName;

        public Persistable(string folderName, string fileName)
        {
            this.FolderName = folderName;
            this.FileName = fileName;

            if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);

            if (!File.Exists(FilePath))
            {
                string filePath = Path.Combine(FolderName, Path.GetFileNameWithoutExtension(FileName));

                TextAsset textAsset = Resources.Load<TextAsset>(filePath);

                if (textAsset != null) Write(textAsset.text);
            }
        }

        public string FolderPath => Path.Combine(Application.persistentDataPath, FolderName);
        public string FilePath => Path.Combine(FolderPath, FileName);

        public string Read() => File.ReadAllText(FilePath);
        public void Write(string data) => File.WriteAllText(FilePath, data);
        public void Delete() => File.Delete(FilePath);
    }
}