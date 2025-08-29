using Photon.Pun;

namespace Cardboard.Utils
{
    /// <summary>
    /// Util class for networking properties.
    /// </summary>
    public class CardboardNetwork
    {
        /// <summary>
        /// Adds a property to the PhotonNetwork LocalPlayer.
        /// </summary>
        /// <param name="key">Key of the property to add.</param>
        /// <param name="value">Value of the property to add.</param>
        public static void CreateProperty(string key, string value)
        {
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add(key, value);
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }
    }
}
