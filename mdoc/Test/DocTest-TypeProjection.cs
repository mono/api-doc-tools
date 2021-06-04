using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mono.DocTest
{

    public class GenericType<T>
    {
        public GenericType<T> TestMethodA(GenericType<T> A)
        {
            return A;
        }
    }

    public class NonGenericType
    {
        public NonGenericType TestMethodB(NonGenericType B)
        {
            return B;
        }
    }

}