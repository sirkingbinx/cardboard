using Cardboard.Utils;

namespace Cardboard.Classes
{
    public class CardboardModded
    {
        public string Name { get; private set; }
        public MethodInfo ModdedJoinMethod;
        public MethodInfo ModdedLeaveMethod;

        public CardboardModded(string _name)
        {
            Name = _name;

            ModdedJoinMethod = typeof(this).GetMethod("ModdedJoin");
            ModdedLeaveMethod = typeof(this).GetMethod("ModdedLeave");
        }

        public void SafeInvoke(ModdedEventType _todo)
        {
            if (_todo = ModdedEventType.ModdedJoin) Method.TryInvoke(ModdedJoinMethod);
            if (_todo = ModdedEventType.ModdedLeave) Method.TryInvoke(ModdedLeaveMethod);  
        }
    }
}