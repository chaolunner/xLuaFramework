using UnityEngine.SceneManagement;
using XLua;

namespace UniEasy
{
    public static class GlobalXLua
    {
        // all lua framework shared one luaenv only!
        public static LuaEnv LuaEnv = new LuaEnv();

        public static string IndexStr = "__index";
        public static string SelfStr = "self";
        public static string InitializeStr = "initialize";
        public static string OnEnableStr = "onenable";
        public static string OnDisableStr = "ondisable";
        public static string OnDestroyStr = "ondestroy";
        public static string InstallBindingsStr = "installbindings";
        public static string OnSceneLoadedStr = "onsceneloaded";

        [CSharpCallLua]
        public delegate void OnSceneLoaded(Scene scene, LoadSceneMode mode);

        /// <summary>
        /// setting up an independent environment for each systems, 
        /// can prevent global variables and function conflicts among systems to some extent.
        /// </summary>
        public static LuaTable NewLuaEnv(LuaTable luaenv = null)
        {
            var newenv = LuaEnv.NewTable();
            var meta = LuaEnv.NewTable();

            meta.Set(IndexStr, luaenv == null ? LuaEnv.Global : luaenv);
            newenv.SetMetaTable(meta);
            meta.Dispose();

            return newenv;
        }
    }
}
