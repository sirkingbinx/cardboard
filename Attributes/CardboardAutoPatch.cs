using System.Reflection;

namespace Cardboard.Attributes
{
    public class CardboardAutoPatch : Attribute
    {
        public Assembly asm; // just in case ig
        public CardboardAutoPatch() => asm = Assembly.GetCallingAssembly();
    }
}