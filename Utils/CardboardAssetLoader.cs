using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cardboard.Utils
{
    /// <summary>
    /// Class for loading asset bundles.
    /// </summary>
    public static class CardboardAssetLoader
    {
        // Asset caching
        private static Dictionary<string, Object> AssetCache = new Dictionary<string, Object>();

        /// <summary>
        /// Loads the specified object from filestream.
        /// </summary>
        /// <param name="_path">Resource to load assets from.</param>
        /// <param name="_name">Name of the object to load.</param>
        /// <returns>The instantiated object.</returns>
        public static T Load<T>(string _path, string _name) where T : Object
        {
            if (AssetCache.TryGetValue($"{_path}|${_name}", out Object asset) && asset is T)
                return asset;
            
            AssetBundle AB = AssetBundle.LoadFromStream(Assembly.GetCallingAssembly().GetManifestResourceStream(_path));
            Object obj = (T)AB.LoadAsset(_name);
            AB.Unload(false);

            AssetCache.Add($"{_path}|${_name}", obj);
            return Object.Instantiate(obj); // returning a copy for immutability
        }

        /// <summary>
        /// Unloads and removes all items from the asset cache. This should be done after all assets have been loaded to save memory.
        /// </summary>
        public static void FreeAssetCache() {
            AssetCache.Values.ForEach(obj => obj.Destroy());
            AssetCache.Clear();
        }

        /// <summary>
        /// Loads the specified GameObject from filestream.
        /// </summary>
        /// <param name="_path">Resource to load assets from.</param>
        /// <param name="_name">Name of the GameObject to load.</param>
        /// <returns>The instantiated GameObject.</returns>
        public static GameObject LoadGameObject(string _path, string _name) =>
            Load<GameObject>(_path, _name);
    }
}
