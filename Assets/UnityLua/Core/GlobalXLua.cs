using UnityEngine.SceneManagement;
using UniEasy.ECS;
using XLua;

namespace UniEasy
{
    public static class GlobalXLua
    {
        // all lua framework shared one luaenv only!
        public static LuaEnv LuaEnv = new LuaEnv();

        [CSharpCallLua]
        public delegate void OnSceneLoaded(Scene scene, LoadSceneMode mode);

        [CSharpCallLua]
        public delegate void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory, PrefabFactory prefabFactory);
    }
}
