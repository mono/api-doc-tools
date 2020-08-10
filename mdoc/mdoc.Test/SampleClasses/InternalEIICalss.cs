using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mdoc.Test.SampleClasses;

namespace mdoc.Test2
{
    public class InternalEIICalss : InterfaceA
    {
        public InternalEIICalss()
        { }

        public string Getstring(int a)
        {
            return a.ToString();
        }

        string InterfaceA.Getstring(int a)
        {
            return "";
        }

        string InterfaceA.color { get { return "Red"; } }
    }
}
