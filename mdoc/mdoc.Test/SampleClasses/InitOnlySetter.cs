namespace mdoc.Test.SampleClasses
{
    public class InitOnlySetter
    {
        public int Property1 { get; set; }
        public int Property2 { get; init; }
        public int Property3 { get; protected init; }
        public int this[int index]
        {
            get
            {
                throw null;
            }
            init
            {
                throw null;
            }
        }
    }
}
