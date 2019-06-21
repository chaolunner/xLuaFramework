using XLua;

namespace UnityLua
{
    public static class GlobalXLua
    {
        // all lua framework shared one luaenv only!
        public static LuaEnv LuaEnv = new LuaEnv();
    }
}
