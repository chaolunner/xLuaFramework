using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using XLua;

namespace UniEasy.DI
{
    public class LuaInstaller : MonoInstaller
    {
        public float GCInterval = 1;
        private float lastGCTime;
        private LuaTable InstallerEnv;
        private Action luaInstallBindings;
        private GlobalXLua.OnSceneLoaded luaOnSceneLoaded;
        private Action luaOnEnable;
        private Action luaUpdate;
        private Action luaOnDisable;

        public override void InstallBindings()
        {
            GlobalXLua.LuaEnv.DoString("require 'Lua/Scripts/LuaInstaller'");
            GlobalXLua.LuaEnv.Global.Get("InstallerEnv", out InstallerEnv);
            InstallerEnv.Set("self", this);
            InstallerEnv.Set("Container", Container);
            InstallerEnv.Get("InstallBindings", out luaInstallBindings);
            InstallerEnv.Get("OnSceneLoaded", out luaOnSceneLoaded);
            InstallerEnv.Get("OnEnable", out luaOnEnable);
            InstallerEnv.Get("Update", out luaUpdate);
            InstallerEnv.Get("OnDisbale", out luaOnDisable);

            luaInstallBindings?.Invoke();
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
                GlobalXLua.LuaEnv.Tick();
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
    }
}
