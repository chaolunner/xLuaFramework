# LuaFramework

Created by chaolun

Requirements
---

- [xLua](https://github.com/Tencent/xLua)

- [UnityFramework](https://github.com/chaolunner/UnityFramework)

- Addressable `"com.unity.addressables": "1.1.4-preview"`

How to Start
---

- Download [Visual Studio Code](https://code.visualstudio.com/)

- Ignore unity files and add support for **.lua.txt** format

  ![](https://github.com/chaolunner/LuaFramework/blob/master/Documents/settings.png)

  Click `Edit in settings.json`.

  ![](https://github.com/chaolunner/LuaFramework/blob/master/Documents/associations.png)

  Add the following JSON to your workspace settings.

  ```
  {
    "files.exclude": {
        "**/.git": true,
        "**/.DS_Store": true,
        "**/*.meta": true,
        "**/*.*.meta": true,
        "**/*.unity": true,
        "**/*.unityproj": true,
        "**/*.mat": true,
        "**/*.fbx": true,
        "**/*.FBX": true,
        "**/*.tga": true,
        "**/*.cubemap": true,
        "**/*.prefab": true,
        "**/Library": true,
        "**/ProjectSettings": true,
        "**/Temp": true
    },
    "files.associations": {
        "*.lua.bytes": "lua",
        "*.lua.txt": "lua"
    }
  }
  ```

  ![](https://github.com/chaolunner/LuaFramework/blob/master/Documents/settings-json.png)

- Install Lua plugin (choose one)

  - Install [EmmyLua](https://github.com/EmmyLua/IntelliJ-EmmyLua) (free)。

    ![](https://github.com/chaolunner/LuaFramework/blob/master/Documents/emmylua.png)

  - Install [luaide](https://github.com/k0204/LuaIde) (vscode extensions - paid, github - free)

    ![](https://github.com/chaolunner/LuaFramework/blob/master/Documents/luaide.png)

Reference
---

- [Unity Development with VS Code](https://code.visualstudio.com/docs/other/unity)

  VS Code documents about Unity.

- [LuaFramework/wiki](https://github.com/chaolunner/LuaFramework/wiki)

  LuaFramework documents.

- [Unity Asset Bundle Browser tool](https://docs.unity3d.com/Manual/AssetBundles-Browser.html) (Deprecated can be replaced by Addressable Asset System after Unity2018.2+)

  Editor tool for viewing and debugging asset bundle contents before and after builds.

- [Addressable Asset System](https://docs.unity3d.com/Packages/com.unity.addressables@1.1/manual/index.html)

  The Addressable Asset System provides an easy way to load assets by “address”. It handles asset management overhead by simplifying content pack creation and deployment.
