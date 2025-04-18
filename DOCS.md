# Docs
Not great at making docs but felt like I need to. Here's everything with some examples.
In these docs, it is organized by namespace.

> Note:
> If these docs are not up to date, all summaries of the mods are up-to-date, so your Visual Studio install can tell you what functions do.

# Classes
## CardboardModdedHandler
- **Description**:
An attribute providing modded functionality from most Utilla forks. If Utilla is not installed, Cardboard will manage the modded room functionality itself.
It deals with the attributes `CardboardModdedJoin` and `CardboardModdedLeave`.
- **Example**:
    ```cs
    using Cardboard.Classes;
    using BepInEx;

    [CardboardModdedHandler]
    public class MyGamemodeHandler : MonoBehaviour
    {
        bool inModded = false;

        [CardboardModdedJoin]
        public void ModdedJoin() => inModded = true;

        [CardboardModdedLeave]
        public void ModdedLeave() => inModded = false;

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
## Method
### `void` `TryInvoke(MethodInfo _methodInfo)`
- **Description**: Attempt to invoke `_methodInfo` with no arguments.
- **Example**:
    ```cs
    MethodInfo UpdateMethodInfo;
    
    void Update()
    {
        if (UpdateEnabled)
            Method.TryInvoke(UpdateMethodInfo);
    }
    ```
### `List<MethodInfo> FindCaseOfAttribute<T>()`
- **Description**: Get all methods that have attribute `T`.
- **Example**
    ```
    public class ExampleThingy : Attribute
    public List<MethodInfo> methods = Method.FindCaseOfAttribute<ExampleThingy>();
    ```
- **Overloads**:
    - `FindCaseOfAttribute<T>()`
    - `FindCaseOfAttribute<T>(Assembly _assembly)`

## CardboardAssetLoader
Loads assets, big shocker
### `GameObject` `Load(string _path, string _name)`
- **Description**: Loads GameObject `_name` from the resource at `_path`.
- **Example**:
    ```cs
    GameObject myAsset = CardboardAssetLoader.Load("Resources/myassetbundle", "FooBarPrefab");
    ```