using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdoc.Test2
{
   public interface Interface_B
   {
        string color { get; set; }
        int no { get;  }
        string GetNo();
        bool IsNum(string no);
        event EventHandler<EventArgs> ItemChanged;


    }
}
