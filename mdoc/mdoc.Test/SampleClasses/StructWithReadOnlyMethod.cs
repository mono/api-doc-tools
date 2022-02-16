namespace mdoc.Test.SampleClasses
{
    public struct StructWithReadOnlyMethod
    {
        public double X { get; set; }
        public double Y { get; set; }

        public readonly double Sum()
        {
            return X + Y;
        }
    }
}
