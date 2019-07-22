using UnityEngine.SceneManagement;
using UniEasy.ECS;
using XLua;

namespace UniEasy
{
    public static class LuaDefine
    {
        // all lua framework shared one luaenv only!
        public static LuaEnv LuaEnv = new LuaEnv();

        public delegate void OnSceneLoaded(Scene scene, LoadSceneMode mode);
        public delegate void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory, PrefabFactory prefabFactory);
    }
}
