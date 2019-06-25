using UnityEngine;
using System;
using XLua;

namespace UniEasy.ECS
{
    public class LuaSystem : SystemBehaviour
    {
        public bool IsGlobal;
        public TextAsset LuaAsset;

        private LuaTable systemEnv;
        private Action luaInitialize;
        private Action luaOnEnable;
        private Action luaOnDisable;
        private Action luaOnDestroy;

        public static LuaTable GlobalEnv = GlobalXLua.NewLuaEnv();

        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory, PrefabFactory prefabFactory)
        {
            base.Initialize(eventSystem, poolManager, groupFactory, prefabFactory);

            InitializeLuaSystem();
            luaInitialize?.Invoke();
        }

        private void InitializeLuaSystem()
        {
            systemEnv = IsGlobal ? GlobalEnv : GlobalXLua.NewLuaEnv(GlobalEnv);
            systemEnv.Set(GlobalXLua.SelfStr, this);

            if (LuaAsset)
            {
                GlobalXLua.LuaEnv.DoString(LuaAsset.text, LuaAsset.name, systemEnv);
            }

            systemEnv.Get(GlobalXLua.InitializeStr, out luaInitialize);
            systemEnv.Get(GlobalXLua.OnEnableStr, out luaOnEnable);
            systemEnv.Get(GlobalXLua.OnDisableStr, out luaOnDisable);
            systemEnv.Get(GlobalXLua.OnDestroyStr, out luaOnDestroy);
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
