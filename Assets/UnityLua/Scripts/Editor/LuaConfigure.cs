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
        static List<string> ExcludeList = new List<string>
        {
            "HideInInspector", "ExecuteInEditMode",
            "AddComponentMenu", "ContextMenu",
            "RequireComponent", "DisallowMultipleComponent",
            "SerializeField", "AssemblyIsEditorAssembly",
            "Attribute", "Types",
            "UnitySurrogateSelector", "TrackedReference",
            "TypeInferenceRules", "FFTWindow",
            "RPC", "Network", "MasterServer",
            "BitStream", "HostData",
            "ConnectionTesterStatus", "GUI", "EventType",
            "EventModifiers", "FontStyle", "TextAlignment",
            "TextEditor", "TextEditorDblClickSnapping",
            "TextGenerator", "TextClipping", "Gizmos",
            "ADBannerView", "ADInterstitialAd",
            "Android", "Tizen", "jvalue",
            "iPhone", "iOS", "Windows", "CalendarIdentifier",
            "CalendarUnit", "CalendarUnit",
            "ClusterInput", "FullScreenMovieControlMode",
            "FullScreenMovieScalingMode", "Handheld",
            "LocalNotification", "NotificationServices",
            "RemoteNotificationType", "RemoteNotification",
            "SamsungTV", "TextureCompressionQuality",
            "TouchScreenKeyboardType", "TouchScreenKeyboard",
            "MovieTexture", "UnityEngineInternal",
            "Terrain", "Tree", "SplatPrototype",
            "DetailPrototype", "DetailRenderMode",
            "MeshSubsetCombineUtility", "AOT", "Social", "Enumerator",
            "SendMouseEvents", "Cursor", "Flash", "ActionScript",
            "OnRequestRebuild", "Ping",
            "ShaderVariantCollection", "SimpleJson.Reflection",
            "CoroutineTween", "GraphicRebuildTracker",
            "Advertisements", "UnityEditor", "WSA",
            "EventProvider", "Apple",
            "ClusterInput", "Motion",
            "UnityEngine.UI.ReflectionMethodsCache", "NativeLeakDetection",
            "NativeLeakDetectionMode", "WWWAudioExtensions", "UnityEngine.Experimental",
            "UniEasy.HttpServerSettings", "XLua", "UnityEditor", "UniEasy.Editor",
            "System.Reflection", "UniEasy.TypeExtensions", "UniEasy.TypeHelper",
            "UniEasy.AssemblyHelper", "UniEasy.HttpServer",
            "UnityEngine.AnimatorControllerParameter", "UnityEngine.AudioSettings",
            "UnityEngine.DrivenRectTransformTracker", "UnityEngine.Caching",
            "UnityEngine.Input", "UnityEngine.LightProbeGroup",
            "UnityEngine.ParticleSystemForceField", "UnityEngine.QualitySettings",
            "UnityEngine.Texture", "UnityEngine.UI.Graphic",
            "UnityEngine.UI.Text",
        };

        static List<string> ExcludeDelegateList = new List<string>
        {
            "XLua", "UnityEditor", "System.Reflection",
            "UniEasy.HttpServerSettings", "UniEasy.Editor",
        };

        static bool IsExclude(Type type)
        {
            var fullName = type.FullName;
            for (int i = 0; i < ExcludeList.Count; i++)
            {
                if (fullName == null || fullName.Contains(ExcludeList[i]))
                {
                    return true;
                }
            }
            return false;
        }

        [Hotfix]
        static IEnumerable<Type> HotfixInject
        {
            get
            {
                return (from type in Assembly.Load("Assembly-CSharp").GetExportedTypes()
                        where !IsExclude(type)
                        select type);
            }
        }

        static List<Type> IncludeTypeList = new List<Type>()
        {
            typeof(object),
            typeof(List<int>),
            typeof(List<float>),
            typeof(List<string>),
            typeof(List<UnityEngine.Object>),
            typeof(Action<int>),
            typeof(Action<long>),
            typeof(Action<float>),
            typeof(Action<string>),
            typeof(Action<UnityEngine.Object>),
        };

        [LuaCallCSharp]
        public static IEnumerable<Type> LuaCallCSharp
        {
            get
            {
                List<string> namespaces = new List<string>()
                {
                    "UnityEngine",
                    "UnityEngine.UI",
                };
                var unityTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                  where !(assembly.ManifestModule is System.Reflection.Emit.ModuleBuilder)
                                  from type in assembly.GetExportedTypes()
                                  where type.Namespace != null && namespaces.Contains(type.Namespace) && !IsExclude(type)
                                          && type.BaseType != typeof(MulticastDelegate) && !type.IsInterface && !type.IsEnum
                                  select type);

                string[] customAssemblys = new string[]
                {
                    "Assembly-CSharp",
                };
                var customTypes = (from assembly in customAssemblys.Select(s => Assembly.Load(s))
                                   from type in assembly.GetExportedTypes()
                                   where (type.Namespace == null || !IsExclude(type))
                                           && type.BaseType != typeof(MulticastDelegate) && !type.IsInterface && !type.IsEnum
                                   select type);
                return unityTypes.Concat(customTypes).Concat(IncludeTypeList).Distinct();
            }
        }

        static bool DelegateHasRef(Type delegateType, string refNamespace)
        {
            if (TypeHelper.TypeHasRef(delegateType, refNamespace)) { return true; }
            var method = delegateType.GetMethod("Invoke");
            if (method == null) { return false; }
            if (TypeHelper.TypeHasRef(method.ReturnType, refNamespace)) { return true; }
            return method.GetParameters().Any(pinfo => TypeHelper.TypeHasRef(pinfo.ParameterType, refNamespace));
        }

        static bool IsExcludeDelegate(Type delegateType)
        {
            return IsExclude(delegateType) || delegateType.BaseType != typeof(MulticastDelegate) || TypeHelper.HasGenericParameter(delegateType) || ExcludeDelegateList.Any(str => DelegateHasRef(delegateType, str));
        }

        [CSharpCallLua]
        static IEnumerable<Type> CSharpCallLua
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
                return (returnTypes.Concat(paramTypes).Concat(fieldTypes)).Concat(IncludeTypeList).Where(t => !IsExcludeDelegate(t)).Distinct();
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
