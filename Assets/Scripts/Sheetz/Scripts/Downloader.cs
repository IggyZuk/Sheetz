using System;
using UnityEngine.Networking;

namespace Sheetz
{
    public static class Downloader
    {
        public static void Download(string URL, Action<string> onSuccess, Action<string> onFail)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(URL);
            
            webRequest.SendWebRequest().completed += op =>
            {
                if (webRequest.isNetworkError) onFail?.Invoke(webRequest.error);
                else onSuccess?.Invoke(webRequest.downloadHandler.text);

                webRequest.Dispose();
            };
        }
    }
}