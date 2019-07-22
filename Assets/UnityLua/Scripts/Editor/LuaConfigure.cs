using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System;
using XLua;

namespace UniEasy.Editor
{
    public static class LuaConfigure
    {
        static List<string> ExcludeHotfixNamespaceList = new List<string>()
        {
            "XLua",
            "UnityEditor",
            "UniEasy.Editor",
        };

        static List<Type> ExcludeHotfixTypeList = new List<Type>()
        {
            typeof(UniEasy.HttpServerSettings),
        };

        static bool IsExcludeHotfix(Type type)
        {
            return ExcludeHotfixNamespaceList.Any(str => !string.IsNullOrEmpty(type.Namespace) && type.Namespace.StartsWith(str)) || ExcludeHotfixTypeList.Any(t => t.IsSameOrSubclassOf(type));
        }

        [Hotfix]
        static IEnumerable<Type> HotfixInject
        {
            get
            {
                return (from type in Assembly.Load("Assembly-CSharp").GetExportedTypes()
                        where !IsExcludeHotfix(type)
                        select type);
            }
        }

        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(System.Object),
            typeof(UnityEngine.Object),
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4),
            typeof(Quaternion),
            typeof(Color),
            typeof(Ray),
            typeof(Bounds),
            typeof(Ray2D),
            typeof(Time),
            typeof(GameObject),
            typeof(Component),
            typeof(Behaviour),
            typeof(Transform),
            typeof(Resources),
            typeof(TextAsset),
            typeof(Keyframe),
            typeof(AnimationCurve),
            typeof(AnimationClip),
            typeof(MonoBehaviour),
            typeof(ParticleSystem),
            typeof(SkinnedMeshRenderer),
            typeof(Renderer),
            typeof(WWW),
            typeof(Light),
            typeof(Mathf),
            typeof(System.Collections.Generic.List<int>),
            typeof(Action<string>),
            typeof(UnityEngine.Debug),
            typeof(UnityEngine.Networking.UnityWebRequest),
            typeof(UniEasy.DI.DiContainer),
            typeof(UniEasy.ObjectExtensions),
            typeof(UniEasy.IObservableExtensions)
        };

        static List<string> ExcludeDelegateNamespaceList = new List<string>()
        {
            "XLua",
            "UnityEditor",
            "UniEasy.Editor",
            "System.Reflection",
        };

        static List<Type> ExcludeDelegateTypeList = new List<Type>()
        {
        };

        static bool DelegateHasRef(Type delegateType, string refNamespace)
        {
            if (TypeHelper.TypeHasRef(delegateType, refNamespace)) return true;
            var method = delegateType.GetMethod("Invoke");
            if (method == null)
            {
                return false;
            }
            if (TypeHelper.TypeHasRef(method.ReturnType, refNamespace)) return true;
            return method.GetParameters().Any(pinfo => TypeHelper.TypeHasRef(pinfo.ParameterType, refNamespace));
        }

        static bool IsExcludeDelegate(Type delegateType)
        {
            return ExcludeDelegateTypeList.Contains(delegateType) || delegateType.BaseType != typeof(MulticastDelegate) || TypeHelper.HasGenericParameter(delegateType) || ExcludeDelegateNamespaceList.Any(str => DelegateHasRef(delegateType, str));
        }

        [CSharpCallLua]
        static IEnumerable<Type> LuaFunctionInject
        {
            get
            {
                BindingFlags flag = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
                List<Type> allTypes = new List<Type>();
                var allAssemblys = new Assembly[]
                {
                    Assembly.Load("Assembly-CSharp")
                };
                foreach (var t in (from assembly in allAssemblys from type in assembly.GetTypes() select type))
                {
                    var p = t;
                    while (p != null)
                    {
                        allTypes.Add(p);
                        p = p.BaseType;
                    }
                }
                allTypes = allTypes.Distinct().ToList();
                var allMethods = from type in allTypes
                                 from method in type.GetMethods(flag)
                                 select method;
                var returnTypes = from method in allMethods
                                  select method.ReturnType;
                var paramTypes = allMethods.SelectMany(m => m.GetParameters()).Select(pinfo => pinfo.ParameterType.IsByRef ? pinfo.ParameterType.GetElementType() : pinfo.ParameterType);
                var fieldTypes = from type in allTypes
                                 from field in type.GetFields(flag)
                                 select field.FieldType;
                return (returnTypes.Concat(paramTypes).Concat(fieldTypes)).Where(t => !IsExcludeDelegate(t)).Distinct();
            }
        }

        [BlackList]
        public static List<List<string>> BlackList = new List<List<string>>()
        {
            new List<string>(){"System.Xml.XmlNodeList", "ItemOf"},
            new List<string>(){"UnityEngine.WWW", "movie"},
#if UNITY_WEBGL
            new List<string>(){"UnityEngine.WWW", "threadPriority"},
#endif
            new List<string>(){"UnityEngine.Texture2D", "alphaIsTransparency"},
            new List<string>(){"UnityEngine.Security", "GetChainOfTrustValue"},
            new List<string>(){"UnityEngine.CanvasRenderer", "onRequestRebuild"},
            new List<string>(){"UnityEngine.Light", "areaSize"},
            new List<string>(){"UnityEngine.Light", "lightmapBakeType"},
            new List<string>(){"UnityEngine.Light", "SetLightDirty"}, //2018.3.10f1 not support
            new List<string>(){"UnityEngine.Light", "shadowRadius"}, //2018.3.10f1 not support
            new List<string>(){"UnityEngine.Light", "shadowAngle"}, //2018.3.10f1 not support
            new List<string>(){"UnityEngine.WWW", "MovieTexture"},
            new List<string>(){"UnityEngine.WWW", "GetMovieTexture"},
            new List<string>(){"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
#if !UNITY_WEBPLAYER
            new List<string>(){"UnityEngine.Application", "ExternalEval"},
#endif
            new List<string>(){"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
            new List<string>(){"UnityEngine.Component", "networkView"},  //4.6.2 not support
            new List<string>(){"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
            new List<string>(){"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
            new List<string>(){"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
            new List<string>(){"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
            new List<string>(){"System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity"},
            new List<string>(){"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
            new List<string>(){"UnityEngine.MonoBehaviour", "runInEditMode"},
        };
    }
}
