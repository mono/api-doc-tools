using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Documentation.Util;
using Windows.UI.Xaml;
using Mono.Documentation.Updater;

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

            Assert.AreEqual(3, list.Count());
        }

        [TestCase]
        public void Test_AttachedProperty_Formatter()
        {
            string expected = "see GetSome, and SetSome";

            var type = GetTypeDef<AttachedTestClass>();
            var list = AttachedEntitiesHelper.GetAttachedEntities(type);

            MemberFormatter formatter = new CSharpMemberFormatter();
            string def = formatter.GetDeclaration(list.First());
            Assert.AreEqual(expected, def);
        }

        [TestCase]
        public void Test_AttachedProperty_Formatter_GetOnly()
        {
            string expected = "see GetSomeGet";

            var type = GetTypeDef<AttachedTestClass>();
            var list = AttachedEntitiesHelper.GetAttachedEntities(type);

            MemberFormatter formatter = new CSharpMemberFormatter();
            string def = formatter.GetDeclaration(list.Skip(1).First());
            Assert.AreEqual(expected, def);
        }

        [TestCase]
        public void Test_AttachedProperty_Formatter_SetOnly()
        {
            string expected = "see SetSomeSet";

            var type = GetTypeDef<AttachedTestClass>();
            var list = AttachedEntitiesHelper.GetAttachedEntities(type);

            MemberFormatter formatter = new CSharpMemberFormatter();
            string def = formatter.GetDeclaration(list.Skip(2).First());
            Assert.AreEqual(expected, def);
        }

        public class AttachedTestClassNoAttachedEntities { }

        public class AttachedTestClass
        {
            public static readonly DependencyProperty SomeProperty;
            public static bool GetSome(DependencyObject obj) { return false;  }
            public static void SetSome(DependencyObject obj, bool val) { }


            public static readonly DependencyProperty SomeGetProperty;
            public static bool GetSomeGet(DependencyObject obj) { return false; }


            public static readonly DependencyProperty SomeSetProperty;
            public static void SetSomeSet(DependencyObject obj, bool val) { }


            public static DependencyProperty SomeNotReadonlyProperty;
            public static bool GetSomeNotReadOnly(DependencyObject obj) { return false; }
            public static void SetSomeNotReadOnly(DependencyObject obj, bool val) { }
        }
    }
}
