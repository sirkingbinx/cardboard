using System.Reflection;
using UnityEngine;

namespace Cardboard.Utils
{
    public static class CardboardAssetLoader
    {
        /// <summary>
        /// Loads the specified GameObject from filestream.
        /// </summary>
        /// <param name="_path">Resource to load assets from.</param>
        /// <param name="_name">Name of the GameObject to load.</param>
        /// <returns>The instantiated GameObject.</returns>
        [Obsolete("Please use CardboardAssetLoader.LoadGameObject().")]
        public static GameObject Load(string _path, string _name) =>
            LoadGameObject(_path, _name);

        /// <summary>
        /// Loads the specified GameObject from filestream.
        /// </summary>
        /// <param name="_path">Resource to load assets from.</param>
        /// <param name="_name">Name of the GameObject to load.</param>
        /// <returns>The instantiated GameObject.</returns>
        public static GameObject LoadGameObject(string _path, string _name)
        {
            AssetBundle AB = AssetBundle.LoadFromStream(Assembly.GetCallingAssembly().GetManifestResourceStream(_path));
            Object obj = AB.LoadAsset(_name);
            AB.Unload(false);

            return GameObject.Instantiate((GameObject)obj);
        }
    }
}
