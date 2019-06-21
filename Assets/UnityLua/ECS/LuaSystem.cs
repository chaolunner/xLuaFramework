using UniEasy.ECS;
using UnityEngine;
using System;
using XLua;

namespace UnityLua
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

        private static string __IndexStr = "__index";
        private static string SelfStr = "self";
        private static string InitializeStr = "initialize";
        private static string OnEnableStr = "onenable";
        private static string OnDisableStr = "ondisable";
        private static string OnDestroyStr = "ondestroy";

        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory, PrefabFactory prefabFactory)
        {
            base.Initialize(eventSystem, poolManager, groupFactory, prefabFactory);

            InitializeLuaSystem();
            luaInitialize?.Invoke();
        }

        private void InitializeLuaSystem()
        {
            if (IsGlobal)
            {
                systemEnv = GlobalXLua.LuaEnv.Global;
            }
            else
            {
                systemEnv = GlobalXLua.LuaEnv.NewTable();

                // setting up an independent environment for each systems, 
                // can prevent global variables and function conflicts among systems to some extent.
                var meta = GlobalXLua.LuaEnv.NewTable();
                meta.Set(__IndexStr, GlobalXLua.LuaEnv.Global);
                systemEnv.SetMetaTable(meta);
                meta.Dispose();
            }

            systemEnv.Set(SelfStr, this);

            if (LuaAsset)
            {
                GlobalXLua.LuaEnv.DoString(LuaAsset.text, LuaAsset.name, systemEnv);
            }

            systemEnv.Get(InitializeStr, out luaInitialize);
            systemEnv.Get(OnEnableStr, out luaOnEnable);
            systemEnv.Get(OnDisableStr, out luaOnDisable);
            systemEnv.Get(OnDestroyStr, out luaOnDestroy);
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
