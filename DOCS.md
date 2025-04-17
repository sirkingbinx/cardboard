# Docs
## Patching
### ``CardboardLib.HarmonyPatches``
- ``PatchInstance(BaseUnityPlugin)``
Will patch your plugin from your base class.

Example:
```cs
void Start() {
    PatchInstance(this);
}
```