using ABExplorer;
using UnityEngine;
using ABExplorer.Core;

namespace Examples.Scripts
{
    public class SingleAbLoadExample : MonoBehaviour
    {
        private AbLoader _texturesLoader;
        private AbLoader _materialsLoader;
        private AbLoader _prefabsLoader;
        private AbManifest _abManifest;

        private const string AbTextures = "single_ab_load/textures.ab";
        private const string AbMaterials = "single_ab_load/materials.ab";
        private const string AbPrefabs = "single_ab_load/prefabs.ab";
        private const string AssetName = "Cube.prefab";

        async void Start()
        {
            await AbResources.CheckUpdateAsync();

            _abManifest = AbManifestManager.AbManifest;

            if (!_abManifest.IsValid)
            {
                Debug.LogWarning($"Can‘t find the manifest, please make sure to build the asset bundle first.");
                return;
            }

            _texturesLoader = new AbLoader(AbTextures, _abManifest.GetAssetBundleHash(AbTextures));
            await _texturesLoader.LoadAbAsync();
            _materialsLoader = new AbLoader(AbMaterials, _abManifest.GetAssetBundleHash(AbMaterials));
            await _materialsLoader.LoadAbAsync();
            _prefabsLoader = new AbLoader(AbPrefabs, _abManifest.GetAssetBundleHash(AbPrefabs));
            await _prefabsLoader.LoadAbAsync();

            var cubePrefab = _prefabsLoader.LoadAsset<GameObject>(AssetName, false);
            Instantiate(cubePrefab);

            var assetNames = _prefabsLoader.GetAllAssetNames();
            foreach (var assetName in assetNames)
            {
                Debug.Log(assetName);
            }
        }

        void Update()
        {
            if (_texturesLoader != null)
            {
                if (Input.GetKey(KeyCode.U) && Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Debug.Log("Unloads unused textures in the bundle");
                    _texturesLoader.Unload(false);
                }
                else if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Debug.Log("Unloads all textures in the bundle");
                    _texturesLoader.Dispose();
                }
            }

            if (_materialsLoader != null)
            {
                if (Input.GetKey(KeyCode.U) && Input.GetKeyDown(KeyCode.Alpha2))
                {
                    Debug.Log("Unloads unused materials in the bundle");
                    _materialsLoader.Unload(false);
                }
                else if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.Alpha2))
                {
                    Debug.Log("Unloads all materials in the bundle");
                    _materialsLoader.Dispose();
                }
            }

            if (_prefabsLoader != null)
            {
                if (Input.GetKey(KeyCode.U) && Input.GetKeyDown(KeyCode.Alpha3))
                {
                    Debug.Log("Unloads unused prefabs in the bundle");
                    _prefabsLoader.Unload(false);
                }
                else if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.Alpha3))
                {
                    Debug.Log("Unloads all prefabs in the bundle");
                    _prefabsLoader.Dispose();
                }
            }
        }
    }
}