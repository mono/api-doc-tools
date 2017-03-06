namespace MyNamespace {
	public abstract class MyBaseClassOne {}
    public abstract class MyBaseClassTwo {}

    public class MyClass
    #if FXONE
        : MyBaseClassOne
    #endif
    #if FXTWO
        : MyBaseClassTwo
    #endif
    {}
}
