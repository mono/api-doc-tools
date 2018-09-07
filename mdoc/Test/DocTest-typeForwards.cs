#if SECOND
using System.Runtime.CompilerServices;
[assembly:TypeForwardedToAttribute(typeof(TheNamespace.TheClass))] 
#endif

namespace TheNamespace
{
    #if FIRST
    public class TheClass
    {}
    #endif
}