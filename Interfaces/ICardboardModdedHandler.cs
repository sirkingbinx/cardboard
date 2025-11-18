namespace Cardboard.Interfaces
{
    public interface ICardboardModdedHandler {
        public bool Modded => CardboardModded.IsModded;
        
        public void OnModdedJoin() { };
        public void OnModdedLeave() { };
        public void OnPlayerInitialized() { };
    }
}