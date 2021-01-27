using System;

namespace mdoc.Test.NullableReferenceTypes.Delegate
{
    public delegate void Handler(object sender, EventArgs args);
    public delegate void NullableSender(object? sender, EventArgs args);
    public delegate void NullableSenderAndEventArgs(object? sender, EventArgs? args);

    public delegate void GenericHandler<TEventArgs>(object sender, TEventArgs args);
    public delegate void NullableSender<TEventArgs>(object? sender, TEventArgs args);
    public delegate void NullableSenderAndEventArgs<TEventArgs>(object? sender, TEventArgs? args) where TEventArgs : class;
    public delegate void ActionHandler<TClass, TStruct>(TClass t1, TStruct t2) where TClass : class where TStruct : struct;
    public delegate void NullableActionHandler<TClass, TStruct>(TClass? t1, TStruct? t2) where TClass : class where TStruct : struct;

    public delegate TReturn FuncHandler<TReturn>();
    public delegate TReturn? NullableReferenceType<TReturn>() where TReturn : class;
    public delegate TReturn? NullableValueType<TReturn>() where TReturn : struct;
}
