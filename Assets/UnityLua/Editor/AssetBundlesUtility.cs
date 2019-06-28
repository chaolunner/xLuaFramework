using UnityEditor;
using UnityEngine;
using System.IO;

namespace UniEasy
{
    public static class AssetBundlesUtility
    {
        [MenuItem("Assets/Build AssetBundles")]
        private static void BuildAssetBundles()
        {
            var outputPath = Application.dataPath + "/../AssetBundles";
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            SetAssetBundlesNameAndVariant("Lua/Preferences");
            SetAssetBundlesNameAndVariant("Lua/Scripts");
            BuildAssetBundles(outputPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
        }

        private static void BuildAssetBundles(string outputPath, BuildAssetBundleOptions assetBundleOptions, BuildTarget targetPlatform, AssetBundleBuild[] builds = null)
        {
            if (builds != null && builds.Length > 0)
            {
                BuildPipeline.BuildAssetBundles(outputPath, builds, assetBundleOptions, targetPlatform);
            }
            else
            {
                BuildPipeline.BuildAssetBundles(outputPath, assetBundleOptions, targetPlatform);
            }
        }

        private static void SetAssetBundlesNameAndVariant(string path)
        {
            var list = Resources.LoadAll(path);
            foreach (var item in list)
            {
                AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(item)).SetAssetBundleNameAndVariant(path, "");
            }
        }
    }
}
