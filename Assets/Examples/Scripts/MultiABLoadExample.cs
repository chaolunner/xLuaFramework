using System.Collections;
using UnityEngine;
using UniEasy;

public class MultiABLoadExample : MonoBehaviour
{
    private string sceneName = "single_ab_load";
    private string bundleName = "single_ab_load/prefabs.ab";
    private string assetName = "Cube.prefab";

    private IEnumerator Start()
    {
        Debug.Log(GetType() + "Start AssetBundle Framework Test!");
        StartCoroutine(AssetBundleManager.GetInstance().DownloadAssetBundle());
        while (AssetBundleManager.GetInstance().GetDownloadProgress() < 100)
        {
            Debug.Log(string.Format("Download Progress {0}%, Downloaded Size {1}KB/{2}KB", AssetBundleManager.GetInstance().GetDownloadProgress(), AssetBundleManager.GetInstance().GetDownloadedSize().ToString("0"), AssetBundleManager.GetInstance().GetContentSize().ToString("0")));
            yield return null;
        }
        Debug.Log(string.Format("Download Progress {0}%, Downloaded Size {1}KB/{2}KB", AssetBundleManager.GetInstance().GetDownloadProgress(), AssetBundleManager.GetInstance().GetDownloadedSize().ToString("0"), AssetBundleManager.GetInstance().GetContentSize().ToString("0")));
        StartCoroutine(AssetBundleManager.GetInstance().LoadAssetBundle(sceneName, bundleName, OnLoadCompleted));
    }

    private void OnLoadCompleted(string abName)
    {
        var cubePrefab = AssetBundleManager.GetInstance().LoadAsset(sceneName, bundleName, assetName, false);
        if (cubePrefab != null)
        {
            Instantiate(cubePrefab);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            AssetBundleManager.GetInstance().Dispose(sceneName);
        }
    }
}
