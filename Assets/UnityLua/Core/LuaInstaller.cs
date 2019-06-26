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
        private LuaTable luaenv;
        private GlobalXLua.InstallBindings luaInstallBindings;
        private GlobalXLua.OnSceneLoaded luaOnSceneLoaded;
        private Action luaOnEnable;
        private Action luaUpdate;
        private Action luaOnDisable;

        public override void InstallBindings()
        {
            GlobalXLua.LuaEnv.DoString("require 'Lua/Scripts/LuaInstaller'");

            luaenv = GlobalXLua.LuaEnv.Global.Get<LuaTable>("LuaInstaller");
            luaenv.Get("InstallBindings", out luaInstallBindings);
            luaenv.Get("OnSceneLoaded", out luaOnSceneLoaded);
            luaenv.Get("OnEnable", out luaOnEnable);
            luaenv.Get("Update", out luaUpdate);
            luaenv.Get("OnDisbale", out luaOnDisable);

            luaInstallBindings?.Invoke(Container);
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
