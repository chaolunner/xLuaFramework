# LuaFramework

Created by chaolun

Requirements
---

- [xLua](https://github.com/Tencent/xLua)

- [UnityFramework](https://github.com/chaolunner/UnityFramework)

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

- [LuaFramework/wiki](https://github.com/chaolunner/LuaFramework/wiki)

  LuaFramework documents.
