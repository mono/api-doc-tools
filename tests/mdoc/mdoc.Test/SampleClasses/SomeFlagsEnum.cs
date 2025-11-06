using System;

namespace mdoc.Test.SampleClasses
{
    [Flags]
    public enum SomeFlagsEnum
    {
        Read = 1,
        Write = 2,
        ReadWrite = Read | Write,
        Open = 4,
        Close = 8
    }
}
