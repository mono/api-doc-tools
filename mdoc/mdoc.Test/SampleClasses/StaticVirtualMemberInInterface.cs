using System;

namespace mdoc.Test.SampleClasses
{
    public interface StaticVirtualMemberInInterface<TSelf, TOther, TResult> where TSelf : StaticVirtualMemberInInterface<TSelf, TOther, TResult>?
    {
#if NETCOREAPP
        static virtual int StaticVirtualMethod(int left, int right)
        {
            return Math.Max(left, right);
        }
#pragma warning disable CA2252
        static abstract TResult operator +(TSelf left, TOther right);
#pragma warning restore CA2252

        static virtual TResult operator checked +(TSelf left, TOther right) => left + right;
#endif
    }
}
