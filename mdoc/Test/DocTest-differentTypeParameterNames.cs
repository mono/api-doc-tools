namespace TheNamespace
{
#if FIRST
    public class GenericType<K1, V1>
    {
        public void GenericMethod<T1, U1>()
        {
        }

        public delegate void GenericDelegate<D1>();
    }
#endif

#if SECOND
    public class GenericType<K2, V2>
    {
        public void GenericMethod<T2, U2>()
        {
        }

        public delegate void GenericDelegate<D2>();
    }
#endif
}