using UnityEngine;

namespace UniEasy
{
    public static partial class ObjectExtensions
    {
        public static bool IsNull(this Object obj)
        {
            return obj == null;
        }
    }
}
