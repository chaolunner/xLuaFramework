using UnityEngine.SceneManagement;
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
        private float lastGCTime;
        private LuaTable InstallerEnv;
        public Action luaInitialize;
        public LuaDefine.OnSceneLoaded luaOnSceneLoaded;
        public Action luaOnEnable;
        public Action luaUpdate;
        public Action luaOnDisable;

        public override void InstallBindings() { }

        private void Awake()
        {
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            while (AssetBundleManager.GetInstance().GetDownloadProgress() < 100) { yield return null; }

            var manifest = ABManifestLoader.GetInstance().GetABManifest();
            var luaPackCount = 0;
            var completedCount = 0;
            foreach (var abName in manifest.GetAllAssetBundles())
            {
                var sceneName = abName.Substring(0, abName.IndexOf("/"));
                if (sceneName == "lua")
                {
                    StartCoroutine(AssetBundleManager.GetInstance().LoadAssetBundle(sceneName, abName, (_) =>
                    {
                        completedCount++;
                    }));
                    luaPackCount++;
                }
            }

            while (completedCount < luaPackCount) { yield return null; }

            LuaDefine.LuaEnv.AddLoader(CustomizeLoader);
            LuaDefine.LuaEnv.DoString("require 'Lua/Scripts/LuaInstaller'");
            LuaDefine.LuaEnv.Global.Get("InstallerEnv", out InstallerEnv);
            InstallerEnv.Set("self", this);
            InstallerEnv.Set("Container", Container);
            InstallerEnv.Get("Initialize", out luaInitialize);
            InstallerEnv.Get("OnSceneLoaded", out luaOnSceneLoaded);
            InstallerEnv.Get("OnEnable", out luaOnEnable);
            InstallerEnv.Get("Update", out luaUpdate);
            InstallerEnv.Get("OnDisbale", out luaOnDisable);

            luaInitialize?.Invoke();
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
            luaOnSceneLoaded?.Invoke(scene, mode);
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
