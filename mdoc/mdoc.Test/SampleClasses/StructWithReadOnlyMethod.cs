namespace mdoc.Test.SampleClasses
{
    public struct StructWithReadOnlyMethod : Struct_Interface_A
    {
        public double X { get; set; }
        public double Y { get; set; }

        public readonly double Sum()
        {
            return X + Y;
        }

        readonly int Struct_Interface_A.GetNum() => 1;
    }
}
