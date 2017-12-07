namespace TestInterfaceImplementation
{
    public interface Interface7<T>
    {
        int Method1<P>(T t, P p);
        int Method2<T>(T t);
        int Method3(T t);
        int Method4();
    }
}