using Cardboard.Utils;

namespace Cardboard.Interfaces
{
    /// <summary>
    /// Handler for modded lobby events.
    /// </summary>
    public interface ICardboardModdedHandler {
        /// <summary>
        /// Determines if the current lobby is a modded lobby.
        /// </summary>
        public bool Modded => CardboardModded.IsModded;
        
        /// <summary>
        /// Called when the player joins a modded lobby.
        /// </summary>
        public void OnModdedJoin() { }

        /// <summary>
        /// Called when the player leaves a modded lobby.
        /// </summary>
        public void OnModdedLeave() { }
    }
}