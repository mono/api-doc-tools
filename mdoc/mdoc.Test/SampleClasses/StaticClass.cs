using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdoc.Test.SampleClasses
{
    public class StaticClass
    {
        private static StaticClass m;

        private StaticClass()
        {
        }

        static StaticClass()
        {
            m= null;
        }
    }
}
