using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdoc.Test.UWP.TestComponent
{
    public sealed class UwpClassWithProperties
    {
        public string MyReadWriteProperty { get; set; }
        public string MyReadOnlyProperty { get;  }

        // Setter-only properties not supported by Windows metadata.
        // public string MyWriteOnlyProperty { set { } }
    }
}
