namespace MyNamespace {
    public abstract class MyBaseClassOne 
    {
        public virtual string AllVirtual {get;}
        public abstract void AllAbstract();
    }
    public abstract class MyBaseClassTwo 
    {
        public string TwoMember {get;}
        public virtual string AllVirtual {get;}
        public abstract void AllAbstract();
    }

    public class MyClass
    #if FXONE
        : MyBaseClassOne
    #endif
    #if FXTWO
        : MyBaseClassTwo
    #endif
    {
        public override void AllAbstract() {}

    #if FXONE
        public override string AllVirtual { get { return ""; } }
    #endif
    }
}
