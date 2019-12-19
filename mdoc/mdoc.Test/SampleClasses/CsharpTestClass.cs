using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdoc.Test.SampleClasses
{
    public class CsharpTestClass
    {
        public CsharpTestClass() { }
        public string DoWithNullParams(out string a, object b = null, TestClass c = null)
        {
            if (b == null)
                a = "default";
            else
            {
                a = b.GetType().Name;
            }
            return a;
        }
    }
}
