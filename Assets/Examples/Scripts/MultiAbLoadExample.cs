using System.Collections;
using UnityEngine;
using ABExplorer;
using ABExplorer.Core;

namespace Examples.Scripts
{
    public class MultiAbLoadExample : MonoBehaviour
    {
        private const string SceneName = "single_ab_load";
        private const string AbName = "single_ab_load/prefabs.ab";
        private const string assetName = "Cube.prefab";

        private IEnumerator Start()
        {
            Debug.Log(GetType() + "Start MultiAbLoad Test!");
            StartCoroutine(AbResources.DownloadAbAsync());
            var downloadedSize = 0f;
            var contentSize = 0f;
            var unit = AbUnit.Byte;
            while (AbLoaderManager.GetDownloadProgress() < 1)
            {
                contentSize = AbLoaderManager.GetDownloadProgress();
                unit = AbLoaderManager.ToUnit(ref contentSize);
                downloadedSize = AbLoaderManager.GetDownloadedSize(unit);
                Debug.Log(string.Format("Download Progress {0}%, Downloaded Size {1}{2}/{3}{4}",
                    AbLoaderManager.GetDownloadPercent(), Mathf.RoundToInt(downloadedSize), unit,
                    Mathf.RoundToInt(contentSize), unit));
                yield return null;
            }

            Debug.Log(string.Format("Download Progress {0}%, Downloaded Size {1}{2}/{3}{4}",
                AbLoaderManager.GetDownloadPercent(), Mathf.RoundToInt(downloadedSize), unit,
                Mathf.RoundToInt(contentSize), unit));
            StartCoroutine(AssetBundleManager.Instance.LoadAbAsync(SceneName, AbName, OnLoadCompleted));
        }

        private void OnLoadCompleted(string abName)
        {
            var cubePrefab = AbResources.LoadAsset<GameObject>("Single_Ab_Load/Prefabs/Cube.prefab");
            if (cubePrefab != null)
            {
                Instantiate(cubePrefab);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                AssetBundleManager.Instance.Unload(SceneName);
            }
        }
    }
}