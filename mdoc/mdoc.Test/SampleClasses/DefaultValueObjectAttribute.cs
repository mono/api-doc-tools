using System;

namespace mdoc.Test.SampleClasses
{
    public class DefaultValueObjectAttribute : Attribute
    {
        public DefaultValueObjectAttribute()
        {
        }

        public DefaultValueObjectAttribute(object constructorObject)
        {
            ConstructorObject = constructorObject;
        }

        public object ConstructorObject { get; private set; }

        public object PropertiyObject { get; set; }
    }
}
