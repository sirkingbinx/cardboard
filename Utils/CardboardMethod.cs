namespace Cardboard.Utils
{ 
    public class Method
    {
        private void TryInvoke(MethodInfo _toInvoke)
        {
            try {
                _toInvoke.Invoke(null, null);
            } catch (Exception ex) {    };
        }
    }
}