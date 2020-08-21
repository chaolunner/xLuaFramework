using System.Threading.Tasks;
using UnityEngine;
using ABExplorer;

namespace Examples.Scripts
{
    public class MultiAbLoadExample : MonoBehaviour
    {
        private const string SceneName = "single_ab_load";
        private const string AbName = "single_ab_load/prefabs.ab";
        private const string assetName = "Cube.prefab";

        private async void Start()
        {
            Debug.Log(GetType() + "Start MultiAbLoad Test!");

            var downloadTask = AbResources.DownloadAbAsync().Task;
            while (!downloadTask.IsCompleted)
            {
                var progress = AbResources.GetDownloadProgress(out var downloadedSize, out var contentSize,
                    out var unit);
                Debug.Log(
                    $"Download Progress {(progress * 100):F2}%, Downloaded Size {Mathf.RoundToInt(downloadedSize)}{unit}/{Mathf.RoundToInt(contentSize)}{unit}");
                await Task.Yield();
            }

            {
                var progress = AbResources.GetDownloadProgress(out var downloadedSize, out var contentSize,
                    out var unit);
                Debug.Log(
                    $"Download Progress {(progress * 100):F2}%, Downloaded Size {Mathf.RoundToInt(downloadedSize)}{unit}/{Mathf.RoundToInt(contentSize)}{unit}");
            }

            AbResources.LoadAssetAsync<GameObject>("Single_Ab_Load/Prefabs/Cube.prefab").Completed += op =>
            {
                var cubePrefab = op.Result;
                if (cubePrefab != null)
                {
                    Instantiate(cubePrefab);
                }
            };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                AbResources.Unload(SceneName);
            }
        }
    }
}