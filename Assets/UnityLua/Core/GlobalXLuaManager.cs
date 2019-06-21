using UnityEngine;

namespace UnityLua
{
    public class GlobalXLuaManager : MonoBehaviour
    {
        public float GCInterval = 1;
        private float lastGCTime;

        private void Update()
        {
            if (Time.time - lastGCTime > GCInterval)
            {
                GlobalXLua.LuaEnv.Tick();
                lastGCTime = Time.time;
            }
        }
    }
}
