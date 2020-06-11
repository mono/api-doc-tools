using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdoc.Test2
{
    public class EiiImplementClass : Interface_A,Interface_B
    {
        public event EventHandler<EventArgs> ItemChanged;
        public string color { get; set; }

        public int GetNum() => 3;

        public int no { get; }
        int Interface_B.no { get;  }

        public string GetNo() => "1";
        string Interface_A.GetNo() => "7";

        public bool IsNum(string no) => false;
        bool Interface_B.IsNum(string no) => true;

        protected virtual void OnItemChanged(EventArgs e)
        { }
        
        event EventHandler<EventArgs> Interface_B.ItemChanged
        {
            add { }
            remove { }
        }

    }
}
