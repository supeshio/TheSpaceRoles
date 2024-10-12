using BepInEx.Unity.IL2CPP.Utils;
using Il2CppInterop.Runtime.Attributes;
using System.Collections;
using System.IO;
using System.Text.Json;
using UnityEngine;
using UnityEngine.Networking;
using static TheSpaceRoles.CustomHatManager;

namespace TheSpaceRoles
{
    public class HatsLoader : MonoBehaviour
    {
        private bool isRunning;

        public void FetchHats()
        {
            if (isRunning) return;
            this.StartCoroutine(CoFetchHats());
        }

        [HideFromIl2Cpp]
        private IEnumerator CoFetchHats()
        {
            isRunning = true;
            var www = new UnityWebRequest();
            www.SetMethod(UnityWebRequest.UnityWebRequestMethod.Get);
            TSR.Logger.LogMessage($"Download manifest at: {RepositoryUrl}/{ManifestFileName}");
            www.SetUrl($"{RepositoryUrl}/{ManifestFileName}");
            www.downloadHandler = new DownloadHandlerBuffer();
            var operation = www.SendWebRequest();

            while (!operation.isDone)
            {
                yield return new WaitForEndOfFrame();
            }

            if (www.isNetworkError || www.isHttpError)
            {
                TSR.Logger.LogError(www.error);
                yield break;
            }

            var response = JsonSerializer.Deserialize<SkinsConfigFile>(www.downloadHandler.text, new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            });
            www.downloadHandler.Dispose();
            www.Dispose();

            www = new UnityWebRequest();

            TSR.Logger.LogMessage($"Download manifest at: {RepositoryUrl}/{LoadHatjson}");
            www.SetUrl($"{RepositoryUrl}/{LoadHatjson}");
            www.downloadHandler = new DownloadHandlerBuffer();
            var operation2 = www.SendWebRequest();

            while (!operation2.isDone)
            {
                yield return new WaitForEndOfFrame();
            }

            if (www.isNetworkError || www.isHttpError)
            {
                Logger.Error(www.error);
                yield break;
            }

            var response2 = JsonSerializer.Deserialize<repoConfig>(www.downloadHandler.text, new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            });
            www.downloadHandler.Dispose();
            www.Dispose();

            if (!Directory.Exists(HatsDirectory)) Directory.CreateDirectory(HatsDirectory);





            //if (false/*イベント処理はいるらしい*/) UnregisteredHats.AddRange(CustomHatManager.loadHorseHats());

            UnregisteredHats.AddRange(SanitizeHats(response));
            foreach (var repo in response2.Hats)
            {
                www = new UnityWebRequest();
                www.SetMethod(UnityWebRequest.UnityWebRequestMethod.Get);//$"https://raw.githubusercontent.com/{owner}/{repository}/main"
                string OtherRepositoryURL = "https://raw.githubusercontent.com/" + repo.Repository;
                //Logger.Message($"Download manifest at: {OtherRepositoryURL}/{ManifestFileName}");
                www.SetUrl($"{OtherRepositoryURL}/{ManifestFileName}");
                www.downloadHandler = new DownloadHandlerBuffer();
                var op = www.SendWebRequest();

                while (!op.isDone)
                {
                    yield return new WaitForEndOfFrame();
                }

                if (www.isNetworkError || www.isHttpError)
                {
                    TSR.Logger.LogError(www.error);
                    yield break;
                }

                var res = JsonSerializer.Deserialize<SkinsConfigFile>(www.downloadHandler.text, new JsonSerializerOptions
                {
                    AllowTrailingCommas = true
                });
                www.downloadHandler.Dispose();
                www.Dispose();
                UnregisteredHats.AddRange(SanitizeHats(res, OtherRepositoryURL));
            }

            UnregisteredHats.AddRange(SanitizeHats(response, RepositoryUrl));
            var toDownload = GenerateDownloadList(UnregisteredHats);

            //Logger.Message($"I'll download {toDownload.Count} hat files");

            foreach (var fileName in toDownload)
            {
                //Logger.Message($"downloading hat: {fileName}");
                yield return CoDownloadHatAsset(fileName.Item1, fileName.Item2);
            }

            isRunning = false;
        }

        private static IEnumerator CoDownloadHatAsset(string fileName, string url)
        {
            var www = new UnityWebRequest();
            www.SetMethod(UnityWebRequest.UnityWebRequestMethod.Get);
            fileName = fileName.Replace(" ", "%20");
            //TSR.Logger.LogMessage($"downloading hat: {url}/hats/{fileName}");
            www.SetUrl($"{url}/hats/{fileName}");
            www.downloadHandler = new DownloadHandlerBuffer();
            var operation = www.SendWebRequest();

            while (!operation.isDone)
            {
                yield return new WaitForEndOfFrame();
            }

            if (www.isNetworkError || www.isHttpError)
            {
                Logger.Error(www.error);
                yield break;
            }

            var filePath = Path.Combine(HatsDirectory, fileName);
            filePath = filePath.Replace("%20", " ");
            var persistTask = File.WriteAllBytesAsync(filePath, www.downloadHandler.data);
            while (!persistTask.IsCompleted)
            {
                if (persistTask.Exception != null)
                {
                    TSR.Logger.LogError(persistTask.Exception.Message);
                    break;
                }

                yield return new WaitForEndOfFrame();
            }

            www.downloadHandler.Dispose();
            www.Dispose();
        }
    }
}
