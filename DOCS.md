# Docs
Not great at making docs but felt like I need to. Here's everything with some examples.
In these docs, it is organized by namespace.

> Note:
> If these docs are not up to date, all summaries of the mods are up-to-date, so your Visual Studio install can tell you what functions do.

# Classes
## CardboardModded
- **Description**:
A class providing modded functionality from most Utilla forks. If Utilla is not installed, Cardboard will manage the modded room functionality itself.
To use it, subscribe a modded join function to delegates `ModdedJoin` and `ModdedLeave`.
- **Example**:
    ```cs
    using Cardboard.Classes;
    using BepInEx;

    public class MyGamemodeHandler : MonoBehaviour
    {
        bool inModded = false;

        public void ModdedJoin() => inModded = true;
        public void ModdedLeave() => inModded = false;

        public void Start()
        {
            CardboardModded.ModdedJoin += ModdedJoin;
            CardboardModded.ModdedLeave += ModdedLeave;
        }

        public void Update()
        {
            if (inModded)
            {
                // Do your modded stuff here
            }
        }
    }
    ```

# Utils
## CardboardHarmony
CardboardHarmony is a decent handler for patching your mod. It is pretty simple to use and only takes a couple arguments.

### `Harmony` `CardboardHarmony.PatchInstance(BepInPlugin)`
- **Description**:
Patches the `BaseUnityPlugin` provided and returns the `Harmony` class used to patch the plugin.

- **Example**:
    ```cs
    using Cardboard.Utils;
    using BepInEx;

    [BepInPlugin("modauthor.modname", "Mod Name", "1.0.0")]
    public class MyMod : BaseUnityPlugin
    {
        Harmony thisHarmony;

        void Start() {
            // Hand it the currently executing BaseUnityPlugin. Check overloads for all the ways you can call PatchInstance.
            thisHarmony = CardboardHarmony.PatchInstance(this);
        }
    }
    ```

- **Overloads**:
    - ``PatchInstance(BepInPlugin _instance)``
    - ``PatchInstance(string _UUID)``

### `void` `CardboardHarmony.UnpatchInstance(Harmony _instance)`
- **Description**:
    Removes patches for the `_instance` of `Harmony`.
- **Example**:
    ```cs
    Harmony thisInstance;

    void OnEnable() => thisInstance = CardboardHarmony.PatchInstance(this);

    // Only implemented when using CI/GorillaComputer
    void OnDisable() => CardboardHarmony.UnpatchInstance(thisInstance);
    ```
## Player
Player-based stuff.
### `bool` `Steam`
- **Description**: Value representing if the player is playing on SteamVR. `false` if playing on Oculus Rift.
- **Example**: No.

## Input
Handles controller inputs.
### `enum`  `InputType`
- leftPrimary
- leftSecondary
- leftGrip
- leftTrigger
- leftStick

- rightPrimary
- rightSecondary
- rightGrip
- rightTrigger
- rightStick

### `bool`  `Input.GetValue(InputType _inputType)`
- **Description**: Gathers the value of the `_inputType`.
- **Example**:
    ```cs
    void Update()
    {
        if (Input.GetValue(InputType.rightPrimary))
        {
            // stuff to do when the right primary is pressed
            // you couldn't have figured out how to use GetValue() without me
            // thank me for my services by starring the CardboardLib repository
        }
    }
    ```

## CardboardAssetLoader
Loads assets, big shocker
### `GameObject` `Load(string _path, string _name)`
- **Description**: Loads GameObject `_name` from the resource at `_path`.
- **Example**:
    ```cs
    GameObject myAsset = CardboardAssetLoader.Load("Resources/myassetbundle", "FooBarPrefab");
    ```

## CardboardBootstrap
Handles mod installation checks
### `bool` `Installed(string _UUID)`
- **Description**: Checks if mod with UUID `_UUID` is installed.
- **Example**:
    ```cs
    if (CardboardBootstrap.Installed("me.mymod"))
        Debug.Log("Thank you very much!");
    ```