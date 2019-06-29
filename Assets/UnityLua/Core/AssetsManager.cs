using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine;

public static class AssetsManager
{
    public static async void Update(params object[] labels)
    {
        //foreach (var label in labels)
        //{
        //    var handle = Addressables.GetDownloadSizeAsync(label);

        //    await handle.Task;

        //    if (handle.Status == AsyncOperationStatus.Succeeded)
        //    {
        //        Debug.Log(handle.Result);
        //    }
        //}

        Addressables.GetDownloadSizeAsync("default").Completed += op =>
        {
            Debug.Log(op.Result);
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

        Addressables.LoadAssetAsync<TextAsset>("Overview.lua").Completed += op1 =>
        {
            Debug.Log(op1.Result.text);
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
}
