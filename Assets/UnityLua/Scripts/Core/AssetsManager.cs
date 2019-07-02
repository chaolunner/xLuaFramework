using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine;

public static class AssetsManager
{
    public static async void Update(params object[] labels)
    {
        //CleanCache();
        //foreach (var label in labels)
        //{
        //    var handle = Addressables.GetDownloadSizeAsync(label);

        //    await handle.Task;

        //    if (handle.Status == AsyncOperationStatus.Succeeded)
        //    {
        //        Debug.Log(handle.Result);
        //    }
        //}

        CleanCache();

        Addressables.GetDownloadSizeAsync("default").Completed += op =>
        {
            Debug.Log("default size: " + op.Result);
        };

        Addressables.DownloadDependenciesAsync("default").Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                Addressables.LoadAssetAsync<GameObject>("Cube").Completed += op1 =>
                {
                    GameObject.Instantiate(op1.Result);
                };
            }
        };

        Addressables.GetDownloadSizeAsync("lua").Completed += op =>
        {
            Debug.Log("lua size: " + op.Result);
        };

        Addressables.DownloadDependenciesAsync("lua").Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                Addressables.LoadAssetAsync<TextAsset>("Preferences/Overview.lua").Completed += op1 =>
                {
                    Debug.Log(op1.Result.text);
                };
            }
        };

        //foreach (var label in labels)
        //{
        //    var handle = Addressables.DownloadDependenciesAsync(label);

        //    await handle.Task;

        //    if (handle.Status == AsyncOperationStatus.Succeeded)
        //    {
        //        Debug.Log(handle.Result);
        //    }
        //}
    }

    public static bool CleanCache()
    {
        var result = Caching.ClearCache();
#if UNITY_EDITOR
        if (result)
        {
            Debug.Log("Successfully cleaned the cache.");
        }
        else
        {
            Debug.Log("Cache is being used.");
        }
#endif
        return result;
    }
}
