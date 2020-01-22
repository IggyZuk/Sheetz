using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sheetz.Demo
{
    public class Main : MonoBehaviour
    {
        [SerializeField] Text statusLabel;
        [SerializeField] Text versionLabel;
        [SerializeField] Button restartButton;

        Model.State state;

        enum VersionCheck
        {
            Checking,
            Changed,
            Identical,
            Error
        }

        void Awake()
        {
            restartButton.onClick.AddListener(StartDemo);

            StartDemo();
        }

        void StartDemo() => StartCoroutine(DemoCouroutine());

        IEnumerator DemoCouroutine()
        {
            Debug.Log(Application.persistentDataPath);

            state = new Model.State();

            statusLabel.text = "Splash Animation";
            versionLabel.text = "Checking...";

            VersionCheck check = VersionCheck.Checking;

            // Download and compare local and remote versions
            Sheets.version.Check(
                isDifferent =>
                {
                    check = isDifferent ? VersionCheck.Changed : VersionCheck.Identical;
                    versionLabel.text = isDifferent ? "NEW!" : "Nothing Changed";
                },
                errorMessage =>
                {
                    check = VersionCheck.Error;
                    versionLabel.text = errorMessage;
                }
            );

            // Wait until the splash animation is over
            yield return new WaitForSeconds(2f);

            statusLabel.text = "Waiting for version check";

            // If the version has changed then download all sheets
            if (check == VersionCheck.Changed)
            {
                yield return WaitForDownload(() =>
                {
                    // Since all sheet data was downloaded and saved we can now override the version
                    Sheets.version.WriteWithRemoteData();
                },
                () =>
                {
                    Debug.LogWarning("Error Downloading!");
                    statusLabel.text = "Error";
                });
            }

            // Parse data and apply to the state
            state.characters = Sheets.characters.Parse();
            state.languages = Sheets.languages.Parse();

            statusLabel.text = "Game";
            versionLabel.text = Sheets.version.Parse().ToString();

            // After a delay show the state
            yield return new WaitForSeconds(1f);
            statusLabel.text = state.ToString();
        }

        IEnumerator WaitForDownload(System.Action onSuccess, System.Action onFail)
        {
            int downloaded = 0;

            List<Sheet> sheets = new List<Sheet>();
            sheets.Add(Sheets.version);
            sheets.Add(Sheets.characters);
            sheets.Add(Sheets.languages);

            statusLabel.text = $"Downloading: {0}/{sheets.Count}";

            bool isError = false;

            foreach (Sheet sheet in sheets)
            {
                sheet.Download(_ =>
                {
                    downloaded++;
                    statusLabel.text = $"Downloading: {downloaded}/{sheets.Count}";
                },
                error => isError = true);
            }

            yield return new WaitUntil(() => downloaded == sheets.Count || isError);

            if (isError) onFail?.Invoke();
            else onSuccess?.Invoke();
        }
    }
}