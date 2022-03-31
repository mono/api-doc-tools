namespace mdoc.Test.SampleClasses
{
    public class FunctionPointers
    {
        public unsafe static R UnsafeCombine<T1, T2, R>(delegate*<T1, T2, R> combinator, T1 left, T2 right) =>
    combinator(left, right);

        public unsafe static R UnsafeCombine1<T1, T2, R>(delegate* unmanaged[Cdecl]<T1, T2, R> combinator, T1 left, T2 right) =>
            combinator(left, right);

        public unsafe static R UnsafeCombine2<T1, T2, T3, R>(delegate* unmanaged[Stdcall]<ref T1, in T2, out T3, R> combinator, T1 left, T2 right, T3 outVar) =>
    combinator(ref left, right, out outVar);

        public unsafe static R UnsafeCombine3<T1, T2, R>(delegate* unmanaged[Fastcall]<T1, T2, ref R> combinator, T1 left, T2 right) =>
    combinator(left, right);

        public unsafe static R UnsafeCombine4<T1, T2, R>(delegate* unmanaged[Thiscall]<T1, T2, ref readonly R> combinator, T1 left, T2 right) =>
combinator(left, right);
    }
}