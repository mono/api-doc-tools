using System;

namespace TestInterfaceImplementation
{
    public interface Interface1
    {
        #region Methods
        void Method1();
        Interface1 Method2();
        #endregion

        #region Properties
        int Property1 { get; }
        int Property2 { get; set; }
        #endregion

        #region Events
        event EventHandler Event1;
        event EventHandler<EventArgs> Event2;
        #endregion

        #region Indexers
        int this[int index]
        {
            get;
        }

        int this[float index]
        {
            get; set;
        }
        #endregion
    }
}
