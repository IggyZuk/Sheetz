using System;
using UnityEngine;

namespace Sheetz.Demo
{
    public class VersionSheet : Sheet
    {
        public VersionSheet() :
            base(
                Documents.Database,
                1587936774,
                Format.TSV,
                "version.txt"
            )
        { }

        string remoteData;

        public void Check(Action<bool> onSuccess, Action<string> onFail)
        {
            Downloader.Download(
                URL(),
                data =>
                {
                    remoteData = data;

                    Model.Version localVersion = Parse();
                    localVersion.build = Application.version;

                    Model.Version remoteVersion = Parse(data);

                    Debug.Log($"Local: {localVersion} | Remote: {remoteVersion}");

                    bool isDifferent = localVersion >= remoteVersion &&
                                       localVersion.sheets != remoteVersion.sheets;

                    onSuccess?.Invoke(isDifferent);
                },
                onFail
            );
        }

        public void WriteWithRemoteData()
        {
            Sheets.version.Write(remoteData);
        }

        public Model.Version Parse() => Parse(Read());

        public static Model.Version Parse(string data)
        {
            string[] columns = data.Split('\t');

            Model.Version version = new Model.Version();
            version.build = columns[0];
            version.sheets = columns[1];
            return version;
        }
    }
}