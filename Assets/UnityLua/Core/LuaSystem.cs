using UnityEngine;
using System;
using XLua;

namespace UniEasy.ECS
{
    public class LuaSystem : SystemBehaviour
    {
        public LuaTable systemEnv;
        private GlobalXLua.Initialize luaInitialize;
        private Action luaOnEnable;
        private Action luaOnDisable;
        private Action luaOnDestroy;

        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory, PrefabFactory prefabFactory)
        {
            base.Initialize(eventSystem, poolManager, groupFactory, prefabFactory);

            systemEnv.Set("self", this);

            systemEnv.Get("Initialize", out luaInitialize);
            systemEnv.Get("OnEnable", out luaOnEnable);
            systemEnv.Get("OnDisable", out luaOnDisable);
            systemEnv.Get("OnDestroy", out luaOnDestroy);

            luaInitialize?.Invoke(this, eventSystem, poolManager, groupFactory, prefabFactory);
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
            systemEnv.Dispose();
        }
    }
}
