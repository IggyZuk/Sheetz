using System;
using UnityEngine;

namespace Sheetz
{
    public class Sheet : Persistable
    {
        public readonly string DocID;
        public readonly int ID;
        public readonly Format Format;

        public Sheet(string docID, int ID, Format format, string fileName) :
            base("Sheets", fileName)
        {
            this.DocID = docID;
            this.ID = ID;
            this.Format = format;
        }

        public string[] Rows() => Read().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        public string URL() => Sheetz.URL.WithSheet(this);

        public void Download(Action<string> onSuccess = null, Action<string> onFail = null)
        {
            Debug.Log($"Downloading: {FileName}");
            Downloader.Download(
                URL(),
                data =>
                {
                    Debug.Log($"Received: {FileName}:\n{data}");
                    Write(data);
                    onSuccess?.Invoke(data);
                },
                onFail
            );
        }
    }
}