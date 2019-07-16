using UnityEngine;
using UniEasy;

public class SingleABLoadExample : MonoBehaviour
{
    private ABLoader texturesLoader = null;
    private ABLoader materialsLoader = null;
    private ABLoader prefabsLoader = null;

    private string abDependTextures = "single_ab_load/textures.ab";
    private string abDependMaterials = "single_ab_load/materials.ab";
    private string abDependPrefabs = "single_ab_load/prefabs.ab";
    private string assetName = "Cube.prefab";

    void Awake()
    {
        StartCoroutine(ABManifestLoader.GetInstance().LoadMainifestFile());
    }

    void Start()
    {
        texturesLoader = new ABLoader(abDependTextures, loadCompleted: OnTexturesLoadCompleted);
        StartCoroutine(texturesLoader.LoadAssetBundle());
    }

    private void OnTexturesLoadCompleted(string abName)
    {
        materialsLoader = new ABLoader(abDependMaterials, loadCompleted: OnMaterialsLoadCompleted);
        StartCoroutine(materialsLoader.LoadAssetBundle());
    }

    private void OnMaterialsLoadCompleted(string abName)
    {
        prefabsLoader = new ABLoader(abDependPrefabs, loadCompleted: OnPrefabsLoadCompleted);
        StartCoroutine(prefabsLoader.LoadAssetBundle());
    }

    private void OnPrefabsLoadCompleted(string abName)
    {
        var cubePrefab = prefabsLoader.LoadAsset(assetName, false);

        Instantiate(cubePrefab);

        var assetsNames = prefabsLoader.GetAllAssetNames();
        foreach (var name in assetsNames)
        {
            Debug.Log(name);
        }
    }

    void Update()
    {
        if (texturesLoader != null)
        {
            if (Input.GetKey(KeyCode.U) && Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("Unloads unused textures in the bundle");
                texturesLoader.DisposeUnused();
            }
            else if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("Unloads all textures in the bundle");
                texturesLoader.Dispose();
            }
        }
        if (materialsLoader != null)
        {
            if (Input.GetKey(KeyCode.U) && Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("Unloads unused materials in the bundle");
                materialsLoader.DisposeUnused();
            }
            else if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("Unloads all materials in the bundle");
                materialsLoader.Dispose();
            }
        }
        if (prefabsLoader != null)
        {
            if (Input.GetKey(KeyCode.U) && Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("Unloads unused prefabs in the bundle");
                prefabsLoader.DisposeUnused();
            }
            else if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("Unloads all prefabs in the bundle");
                prefabsLoader.Dispose();
            }
        }
    }
}
