using NUnit.Framework;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Mono.Documentation.Util;
using Mono.Documentation.Updater;
using Mono.Documentation.Updater.Formatters;

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
            var type = GetTypeDef<AttachedPropertyTestClass>();
            var list = AttachedEntitiesHelper.GetAttachedEntities(type);

            Assert.AreEqual(3, list.Count());
        }

        [TestCase]
        public void Test_AttachedProperty_Formatter()
        {
            string expected = "see GetSome, and SetSome";

            var type = GetTypeDef<AttachedPropertyTestClass>();
            var list = AttachedEntitiesHelper.GetAttachedEntities(type);

            MemberFormatter formatter = new CSharpMemberFormatter();
            string def = formatter.GetDeclaration(list.First());
            Assert.AreEqual(expected, def);
        }

        [TestCase]
        public void Test_AttachedProperty_Formatter_GetOnly()
        {
            string expected = "see GetSomeGet";

            var type = GetTypeDef<AttachedPropertyTestClass>();
            var list = AttachedEntitiesHelper.GetAttachedEntities(type);

            MemberFormatter formatter = new CSharpMemberFormatter();
            string def = formatter.GetDeclaration(list.Skip(1).First());
            Assert.AreEqual(expected, def);
        }

        [TestCase]
        public void Test_AttachedProperty_Formatter_SetOnly()
        {
            string expected = "see SetSomeSet";

            var type = GetTypeDef<AttachedPropertyTestClass>();
            var list = AttachedEntitiesHelper.GetAttachedEntities(type);

            MemberFormatter formatter = new CSharpMemberFormatter();
            string def = formatter.GetDeclaration(list.Skip(2).First());
            Assert.AreEqual(expected, def);
        }

        [TestCase]
        public void Test_AttachedEntities()
        {
            var type = GetTypeDef<AttachedEntitiesTestClass>();
            Assert.IsTrue(type.Fields.Any(t => t.Name == "TestingEvent"));
            Assert.IsTrue(type.Fields.Any(t => t.Name == "TargetProperty"));
            Assert.IsTrue(type.Fields.Any(t => t.Name == "TargetPropertyProperty"));
            Assert.IsTrue(type.Properties.Any(t => t.Name == "AttributeAttachProperty"));

            var list = AttachedEntitiesHelper.GetAttachedEntities(type);
            Assert.AreEqual(4, list.Count());
            Assert.IsTrue(list.Any(t => t.Name == "Testing" && t is AttachedEventReference));
            Assert.IsTrue(list.Any(t => t.Name == "Target" && t is AttachedPropertyReference));
            Assert.IsTrue(list.Any(t => t.Name == "TargetProperty" && t is AttachedPropertyReference));
            Assert.IsTrue(list.Any(t => t.Name == "AttributeAttach" && t is AttachedPropertyReference));
        }

        [TestCase(IncludePlatform = "Win32NT")]
        public void Test_AttachedEntities_NetFramework()
        {
            var os = Environment.OSVersion;
            Console.WriteLine("OS platform is: {0}", os.Platform.ToString());

            var type = GetTypeDef<System.Windows.Media.Animation.Storyboard>();
            var list = AttachedEntitiesHelper.GetAttachedEntities(type);
            Assert.AreEqual(3, list.Count());
            Assert.IsTrue(type.Fields.Any(t => t.Name == "TargetProperty"));
            Assert.IsTrue(type.Fields.Any(t => t.Name == "TargetPropertyProperty"));
            Assert.IsTrue(list.Any(t => t.Name == "Target" && t is AttachedPropertyReference));
            Assert.IsTrue(list.Any(t => t.Name == "TargetProperty" && t is AttachedPropertyReference));

            type = GetTypeDef<System.Windows.Controls.Primitives.Selector>();
            list = AttachedEntitiesHelper.GetAttachedEntities(type);
            Assert.AreEqual(4, list.Count());
            Assert.IsTrue(list.Any(t => t.Name == "IsSelected" && t is AttachedPropertyReference));
            Assert.IsTrue(list.Any(t => t.Name == "IsSelectionActive" && t is AttachedPropertyReference));
            Assert.IsTrue(list.Any(t => t.Name == "Selected" && t is AttachedEventReference));
            Assert.IsTrue(list.Any(t => t.Name == "Unselected" && t is AttachedEventReference));

            type = GetTypeDef<Windows.UI.Xaml.Media.Animation.Storyboard>();
            list = AttachedEntitiesHelper.GetAttachedEntities(type);
            Assert.AreEqual(2, list.Count());
            Assert.IsTrue(type.Properties.Any(t => t.Name == "TargetNameProperty"));
            Assert.IsTrue(type.Properties.Any(t => t.Name == "TargetPropertyProperty"));
            Assert.IsTrue(list.Any(t => t.Name == "TargetName" && t is AttachedPropertyReference));
            Assert.IsTrue(list.Any(t => t.Name == "TargetProperty" && t is AttachedPropertyReference));
        }

        public class AttachedTestClassNoAttachedEntities { }

        public class AttachedPropertyTestClass
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

        public class AttachedEntitiesTestClass
        {
            public DependencyProperty AttributeAttachProperty { get; set; }
            public static bool GetAttributeAttach(DependencyObject obj) { return false; }
            public static void SetAttributeAttach(DependencyObject obj, bool val) { }

            public static readonly DependencyProperty TargetProperty;
            public static bool GetTarget(DependencyObject obj) { return false; }
            public static void SetTarget(DependencyObject obj, bool val) { }

            public delegate void EventHandler(object sender, EventArgs e);
            public static readonly RoutedEvent TestingEvent;
            public static void AddTestingHandler(DependencyObject element, EventHandler handler) { }
            public static void RemoveTestingHandler(DependencyObject element, EventHandler handler) { }

            public static readonly DependencyProperty TargetPropertyProperty;
            public static string GetTargetProperty(DependencyObject obj)
            {
                if (obj == null) { throw new ArgumentNullException("obj"); }
                return (string)obj.GetValue(TargetPropertyProperty);
            }
            public static void SetTargetProperty(DependencyObject obj, string val)
            {
                if (obj == null) { throw new ArgumentNullException("obj"); }
                obj.SetValue(TargetPropertyProperty, val);
            }
        }
    }
}
