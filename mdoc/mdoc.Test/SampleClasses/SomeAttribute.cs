using System;

namespace mdoc.Test.SampleClasses
{
    public class SomeAttribute
    {
        [DefaultValueObject(PropertiyObject = true)]
        public void PropertiyObjectWithBoolType()
        {
        }

        [DefaultValueObject(PropertiyObject = null)]
        public void PropertiyObjectWithNull()
        {
        }

        [DefaultValueObject(PropertiyObject = SomeEnum.TestEnumElement2)]
        public void PropertiyObjectWithInternalEnumType()
        {
        }

        [DefaultValueObject(PropertiyObject = ConsoleColor.Red)]
        public void PropertiyObjectWithExternalEnumType()
        {
        }
    }
}
