using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mono.DocTest
{
    public interface IVector
    {
        IVector Method(IVector A, IVector B);
    }

}