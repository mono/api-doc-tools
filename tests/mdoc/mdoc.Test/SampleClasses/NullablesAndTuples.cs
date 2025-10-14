using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdoc.Test.SampleClasses
{
    public class NullablesAndTuples
    {
        public int? NullableInt() => null;

        public (int,string) TupleReturn() => (0,"test");
    }
}
