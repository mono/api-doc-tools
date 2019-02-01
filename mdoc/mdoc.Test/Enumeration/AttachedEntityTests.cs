using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Documentation.Util;
using Windows.UI.Xaml;

namespace mdoc.Test.Enumeration
{
    [TestFixture]
    public class AttachedEntityTests : CecilBaseTest
    {
        [TestCase]
        public void Test_NoEntities()
        {
            var type = GetTypeDef<AttachedTestClassNoAttachedEntities>();
            var list = AttachedEntitiesHelper.GetAttachedEntities(type);

            Assert.IsFalse(list.Any());
        }

        [TestCase]
        public void Test_AttachedProperty()
        {
            var type = GetTypeDef<AttachedTestClass>();
            var list = AttachedEntitiesHelper.GetAttachedEntities(type);

            Assert.AreEqual(1, list.Count());
        }

        public class AttachedTestClassNoAttachedEntities { }

        public class AttachedTestClass
        {
            public static readonly DependencyProperty SomeProperty;
            public static bool GetSome(DependencyObject obj) { return false;  }
            public static void SetSome(DependencyObject obj, bool val) { }


            public static DependencyProperty SomeNotReadonlyProperty;
            public static bool GetSomeNotReadOnly(DependencyObject obj) { return false; }
            public static void SetSomeNotReadOnly(DependencyObject obj, bool val) { }
        }
    }
}
