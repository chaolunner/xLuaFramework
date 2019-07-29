using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using System;
using XLua;

namespace UniEasy.DI
{
    public class LuaInstaller : MonoInstaller
    {
        public float GCInterval = 1;
        private bool isInitialized;
        private float lastGCTime;
        private LuaTable InstallerEnv;
        public Action luaInitialize;
        public LuaDefine.OnSceneLoaded luaOnSceneLoaded;
        public Action luaOnEnable;
        public Action luaUpdate;
        public Action luaOnDisable;

        static public List<string> PreloadingList = new List<string>()
        {
            "luacore/preferences.ab",
            "luacore/scripts.ab",
            "luacore/hotfixs.ab",
        };

        public override void InstallBindings() { }

        private void Awake()
        {
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            foreach (var abName in PreloadingList)
            {
                var sceneName = abName.Substring(0, abName.IndexOf("/"));
                yield return AssetBundleManager.GetInstance().LoadAssetBundle(sceneName, abName);
            }

            LuaDefine.LuaEnv.AddLoader(CustomizeLoader);
            LuaDefine.LuaEnv.DoString("require 'LuaCore/Scripts/LuaInstaller'");
            LuaDefine.LuaEnv.Global.Get("InstallerEnv", out InstallerEnv);
            InstallerEnv.Set("self", this);
            InstallerEnv.Set("Container", Container);
            InstallerEnv.Get("Initialize", out luaInitialize);
            InstallerEnv.Get("OnSceneLoaded", out luaOnSceneLoaded);
            InstallerEnv.Get("OnEnable", out luaOnEnable);
            InstallerEnv.Get("Update", out luaUpdate);
            InstallerEnv.Get("OnDisbale", out luaOnDisable);

            luaInitialize?.Invoke();

            while (ABLoaderManager.GetDownloadProgress() < 1) { yield return null; }

            var manifest = ABManifestLoader.GetInstance().GetABManifest();
            foreach (var abName in ABManifestLoader.GetInstance().AssetBundleList)
            {
                var sceneName = abName.Substring(0, abName.IndexOf("/"));
                if (sceneName.StartsWith("lua"))
                {
                    yield return StartCoroutine(AssetBundleManager.GetInstance().LoadAssetBundle(sceneName, abName));
                }
            }

            isInitialized = true;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                OnSceneLoaded(SceneManager.GetSceneAt(i), i == 0 ? LoadSceneMode.Single : LoadSceneMode.Additive);
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            luaOnEnable?.Invoke();
        }

        private void Update()
        {
            luaUpdate?.Invoke();
            if (Time.time - lastGCTime > GCInterval)
            {
                LuaDefine.LuaEnv.Tick();
                lastGCTime = Time.time;
            }
        }

        private void OnDisable()
        {
            luaOnDisable?.Invoke();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (isInitialized)
            {
                luaOnSceneLoaded?.Invoke(scene, mode);
            }
        }

        private byte[] CustomizeLoader(ref string filepath)
        {
            if (HttpServerSettings.GetOrCreateSettings().IsEnable)
            {
                var sceneName = filepath.Substring(0, filepath.IndexOf("/")).ToLower();
                var abName = filepath.Substring(0, filepath.LastIndexOf("/")).ToLower() + ".ab";
                var assetName = filepath.Substring(filepath.LastIndexOf("/") + 1) + ".lua.txt";
                var textAsset = AssetBundleManager.GetInstance().LoadAsset(sceneName, abName, assetName, false) as TextAsset;
                return textAsset.bytes;
            }
            else
            {
                filepath = Application.dataPath + "/UnityLua/AB_Resources/" + filepath + ".lua.txt";
                if (File.Exists(filepath))
                {
                    return File.ReadAllBytes(filepath);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
