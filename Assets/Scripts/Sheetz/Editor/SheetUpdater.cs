using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Sheetz.Tools
{
    public static class SheetUpdater
    {
        [MenuItem("Tools/Sheets/Update Sheets")]
        public static void UpdateSheets()
        {
            DeleteSheets();
            DownloadSheets();
        }

        [MenuItem("Tools/Sheets/Download Sheets")]
        public static void DownloadSheets()
        {
            foreach (Sheet sheet in GetSheets())
            {
                sheet.Download(data =>
                {
                    string path = $"{Application.dataPath}/Resources/Sheets/{sheet.FileName}";
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    File.WriteAllText(path, data);

                    AssetDatabase.Refresh();
                });
            }
        }

        [MenuItem("Tools/Sheets/Delete Sheets")]
        public static void DeleteSheets()
        {
            foreach (Sheet sheet in GetSheets())
            {
                sheet.Delete();
                Debug.Log($"Deleted: {sheet.FileName}");
            }
        }

        static IEnumerable<Sheet> GetSheets()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(Sheet)))
                .Select(type => Activator.CreateInstance(type) as Sheet);
        }
    }
}