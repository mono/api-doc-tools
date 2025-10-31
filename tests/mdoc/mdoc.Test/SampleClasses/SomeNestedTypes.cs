using System;

namespace mdoc.Test.SampleClasses
{
    public class SomeNestedTypes
    {
        public class NestedClass
        {
        }

        public class NestedGenericType<T>
        {
            public class InnerNestedGenericType<T1>
            {
            }
        }

        public enum NestedEnum
        {
            Read = 1,
            Write = 2,
        }

        [Flags]
        public enum NestedFlagsEnum
        {
            Assembly = 1,
            Module = 2,
            Class = 4,
            Struct = 8,
            Enum = 16
        }
    }
}
