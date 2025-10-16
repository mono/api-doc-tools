using System;

namespace mdoc.Test.NullableReferenceTypes
{
    public class Event
    {
        public event EventHandler EventHandler;

        public event EventHandler? NullableEventHandler;

        public event EventHandler<EventArgs> GenericEventHandler;

        public event EventHandler<EventArgs?> GenericEventHandlerOfNullableEventArgs;

        public event EventHandler<EventArgs>? NullableGenericEventHandler;

        public event EventHandler<EventArgs?>? NullableGenericEventHandlerOfNullableEventArgs;
    }
}
