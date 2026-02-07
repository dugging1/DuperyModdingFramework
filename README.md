# Dupery Modding Framework

The Dupery Modding Framework is an unofficial modding framework, similar to Minecraft's Forge, for the steam game Dupery, by Giled Rune Games. While this framework is, technically, a mod for Dupery, it does not add any additional, playable, content to the game

## For Users

This framework uses the Unity Modding system [BepInEx](https://github.com/BepInEx/BepInEx) for the heavy lifting, and so it must be installed first. There are install instructions for BepInEx [here](https://docs.bepinex.dev/articles/user_guide/installation/index.html) and the release list [here](https://github.com/BepInEx/BepInEx/releases). For Windows or linux machines, the 64bit (x64) windows release is likely the desired release. For Mac machines there is only a single mac release.

For linux users, there may be some difficulty getting Steam/Dupery to use BepInEx. This is likely because the Steam compatibility layer is getting in the way. In the Steam properties, adding the launch option `WINEDLLOVERRIDES="winhttp=n,b" %command%` may fix the problem.

After installing BepInEx and generating its various folders and files, download the user install zip file from the [release](https://github.com/dugging1/DuperyModdingFramework/releases) page. This zip file should be extracted into the base directory of Dupery (similar to BepInEx). This should mean that `Dupery.exe` and the `Mods` folder should be in the same directory.

Finally any Dupery mods can be added to the `Mods` folder; the framework will load them automatcally.


## For Mod Creators

NOTICE: Creating a mod for this framework currently requires some basic knowledge of programming in C#, and such knowledge will be assumed going forwards. It will also be assumed that dotnet and an IDE or code editor is already installed. The SDK used is `netstandard2.1` with `latest` language version.

It is first advised to install the framework for testing the created mods.

To setup the mod project for development, first download the example mod zip file. This zip file contains a small example mod with a dotnet project file setup for development. The project (and any DMF mod) requires some additional libraries for development. See the extract of the project file below:

```
<ItemGroup>
    <Reference Include="Assembly-CSharp">
        <HintPath>..\libs\Assembly-CSharp.dll</HintPath>
    </Reference>
    <Reference Include="grglib">
        <HintPath>..\libs\gildedrunegames.grglib.dll</HintPath>
    </Reference>
    <Reference Include="DMF-Common-Library">
        <HintPath>..\libs\DMF-Common-Library.dll</HintPath>
    </Reference>
</ItemGroup>
```

This extract shows the three library (dll) files that are required and where they should be put (relative to the project file). The `DMF-Common-Library` can be found in the user install zip on the [release](https://github.com/dugging1/DuperyModdingFramework/releases) page or the local installation. The other two are Dupery game files (and so not re-distributed here). They can both be found in a local install of the game at `[Base Game Directory]/Dupery_Data/Managed/`. There are two `grglib` library files in the game, be sure to get the right one (not the localisation library).

After setting up the project folder, your IDE may need to be restarted to read the library files. Now, development can begin. Read through the example mod, the DMF common library code, and the wiki on github, to get started.


## For Framework Contributors

NOTICE: Working in this framework knowledge of programming in C#, and such knowledge will be assumed going forwards. It will also be assumed that dotnet and an IDE or code editor is already installed. The SDK used is `netstandard2.1` with `latest` language version.

Contributions to this framework are welcome, especially as I have little experience with Harmony and modding Unity games. Cloning the development branch (or some other branch) will is the quick start, but will require similar library management to the mod creation instructions above.

Suggestions and discovered bugs can be raised as issues, discuissions can be used for both technical and non-technical idea sharing, and the wiki should be kept as updated as possible both mod creators and our own sanity.
