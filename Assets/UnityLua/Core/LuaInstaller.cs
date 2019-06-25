using UnityEngine.SceneManagement;
using System;
using XLua;

namespace UniEasy.DI
{
    public class LuaInstaller : MonoInstaller
    {
        public LuaTable LuaEnv
        {
            get
            {
                return GlobalXLua.LuaEnv.Global;
            }
        }

        private Action luaInstallBindings;
        private Action luaOnEnable;
        private Action luaOnDisable;
        private GlobalXLua.OnSceneLoaded luaOnSceneLoaded;

        public override void InstallBindings()
        {
            GlobalXLua.LuaEnv.DoString("require 'Lua/LuaInstaller'");

            LuaEnv.Get(GlobalXLua.InstallBindingsStr, out luaInstallBindings);
            LuaEnv.Get(GlobalXLua.OnEnableStr, out luaOnEnable);
            LuaEnv.Get(GlobalXLua.OnDisableStr, out luaOnDisable);
            LuaEnv.Get(GlobalXLua.OnSceneLoadedStr, out luaOnSceneLoaded);

            luaInstallBindings?.Invoke();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            luaOnEnable?.Invoke();
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            luaOnDisable?.Invoke();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            luaOnSceneLoaded?.Invoke(scene, mode);
        }
    }
}
