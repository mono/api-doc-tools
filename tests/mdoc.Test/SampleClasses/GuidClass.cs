using System;

namespace mdoc.Test.SampleClasses
{
    public class GuidClass
    {
        private readonly Guid _guid = Guid.Empty;

        private GuidClass() { this._guid = Guid.NewGuid(); }

        public GuidClass (Guid guid) { this._guid = guid; }

        public static Guid CreateNewGuid() { return new Guid(); }
        public bool ObjectIndentical(Guid objGuid1, Guid objGuid2) { return objGuid1 == objGuid2; }
        public bool IsUnique(Guid guid) { return guid == _guid; }
    }
}
