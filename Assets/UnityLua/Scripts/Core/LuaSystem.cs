using UnityEngine;
using System;
using XLua;

namespace UniEasy.ECS
{
    public class LuaSystem : SystemBehaviour
    {
        public LuaTable SystemEnv;
        private GlobalXLua.Initialize luaInitialize;
        private Action luaOnEnable;
        private Action luaOnDisable;
        private Action luaOnDestroy;

        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory, PrefabFactory prefabFactory)
        {
            base.Initialize(eventSystem, poolManager, groupFactory, prefabFactory);

            SystemEnv.Get("Initialize", out luaInitialize);
            SystemEnv.Get("OnEnable", out luaOnEnable);
            SystemEnv.Get("OnDisable", out luaOnDisable);
            SystemEnv.Get("OnDestroy", out luaOnDestroy);

            luaInitialize?.Invoke(eventSystem, poolManager, groupFactory, prefabFactory);
        }

        public override void OnEnable()
        {
            base.OnEnable();

            luaOnEnable?.Invoke();
        }

        public override void OnDisable()
        {
            base.OnDisable();

            luaOnDisable?.Invoke();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            luaOnDestroy?.Invoke();
        }
    }
}
