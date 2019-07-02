using UnityEngine;
using XLua;

namespace UniEasy
{
    [LuaCallCSharp]
    [ReflectionUse]
    public static partial class ObjectExtensions
    {
        public static bool IsNull(this Object obj)
        {
            return obj == null;
        }
    }
}
