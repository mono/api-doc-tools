using System;
using System.Runtime.CompilerServices;

namespace mdoc.Test.SampleClasses
{
    public class ExtensionTestClass
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class ExtensionTestContainer
    {
        private ExtensionTestClass[] items = new ExtensionTestClass[10];

        // Regular indexer
        public ExtensionTestClass this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }
    }

    // Extension methods for testing
    public static class ExtensionMembersExample
    {
        // Standard extension method
        public static string GetDisplayName(this ExtensionTestClass obj)
        {
            return $"{obj.Name} - {obj.Value}";
        }

        // Another extension method
        public static void SetNameAndValue(this ExtensionTestClass obj, string name, int value)
        {
            obj.Name = name;
            obj.Value = value;
        }

        // Extension method with complex parameters
        public static ExtensionTestClass CombineWith(this ExtensionTestClass obj, ExtensionTestClass other, string separator = " | ")
        {
            return new ExtensionTestClass
            {
                Name = obj.Name + separator + other.Name,
                Value = obj.Value + other.Value
            };
        }
    }
}
