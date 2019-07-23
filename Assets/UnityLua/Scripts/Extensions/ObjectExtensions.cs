using UnityEngine;

namespace UniEasy
{
    public static partial class ObjectExtensions
    {
        public static bool luaIsNull(this Object obj)
        {
            return obj == null;
        }
    }
}
