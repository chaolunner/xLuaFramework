using UnityEngine;
using ABExplorer.Core;

namespace Examples.Scripts
{
    public class SingleAbLoadExample : MonoBehaviour
    {
        private AbLoader _texturesLoader;
        private AbLoader _materialsLoader;
        private AbLoader _prefabsLoader;

        private const string AbDependTextures = "single_ab_load/textures.ab";
        private const string AbDependMaterials = "single_ab_load/materials.ab";
        private const string AbDependPrefabs = "single_ab_load/prefabs.ab";
        private const string AssetName = "Cube.prefab";

        void Start()
        {
            _texturesLoader = new AbLoader(AbDependTextures, onLoadCompleted: OnTexturesLoadCompleted);
            StartCoroutine(_texturesLoader.LoadAsync());
        }

        private void OnTexturesLoadCompleted(string abName)
        {
            _materialsLoader = new AbLoader(AbDependMaterials, onLoadCompleted: OnMaterialsLoadCompleted);
            StartCoroutine(_materialsLoader.LoadAsync());
        }

        private void OnMaterialsLoadCompleted(string abName)
        {
            _prefabsLoader = new AbLoader(AbDependPrefabs, onLoadCompleted: OnPrefabsLoadCompleted);
            StartCoroutine(_prefabsLoader.LoadAsync());
        }

        private void OnPrefabsLoadCompleted(string abName)
        {
            var cubePrefab = _prefabsLoader.LoadAsset<GameObject>(AssetName, false);

            Instantiate(cubePrefab);

            var assetsNames = _prefabsLoader.GetAllAssetNames();
            foreach (var name in assetsNames)
            {
                Debug.Log(name);
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