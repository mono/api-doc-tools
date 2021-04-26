namespace TheNamespace
{
    public class TheClass
    {
        #if FIRST
        public TheClass(int arg)
        {}
        #endif
        #if SECOND
        public TheClass(string arg)
        {}
        #endif
    }
}