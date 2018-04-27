using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Issue212Example
{
    [CompilerGenerated]
    [TypeIdentifier]
    public interface ICustomInterface
    {

        int Count { get; }
        double CountDouble { get; set; }
    }

    public interface ICustomInterface2
    {
        string Count2 { get; }
        double CountDouble2 { get; set; }
    }

    public class Class1 : ICustomInterface
    {
        public int Count => 100;
        public double CountDouble { get; set; }
    }

    public class Class2 : ICustomInterface2
    {
        public string Count2 => "bla-bla string";

        public double CountDouble2 { get; set; }
    }
    
}
