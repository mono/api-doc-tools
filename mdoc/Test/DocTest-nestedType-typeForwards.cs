#if SECOND || THIRD
using System.Runtime.CompilerServices;
[assembly:TypeForwardedToAttribute(typeof(TheNamespace.TheClass))] 
#endif

namespace TheNamespace
{
    #if FIRST
    public class TheClass
    {
        protected class InnerClass
        {
            protected class Enumerator {}
        }
    }
    #endif
}