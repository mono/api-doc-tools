using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mono.DocTest
{
    public interface ITypeA
    {
        ITypeA Method(ITypeA A, int B);
    }

}