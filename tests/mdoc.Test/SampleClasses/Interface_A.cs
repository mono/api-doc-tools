using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdoc.Test2
{
    public interface Interface_A
    {
        string color { get; set; }
        int  no  { get;  }
        string GetNo();
        int GetNum();
        event EventHandler<EventArgs> ItemChanged;
    }
}
