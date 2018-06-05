﻿using NUnit.Framework;
using System;
using Mono.Documentation;
using System.Xml;
using System.Linq;
using Mono.Cecil;
using System.Collections.Generic;
using Mono.Documentation.Updater.Frameworks;
using Mono.Documentation.Updater;

namespace mdoc.Test
{
    public class OneAttribute : Attribute { }
    public class TwoAttribute : Attribute { }

    [One]
    public class MyClass
    {
        [One]
        public void Meth (int a, string d, int c) { }

    }
    public class MyClass2
    {
        public void Meth (int a, string b, int c) { }
    }

}

namespace mdoc.Test2
{
    using mdoc.Test;

    [One, Two]
    public class MyClass
    {
        [One, Two]
        public void Meth (int a, string b, int c) { }
    }

    [Two]
    public class MyClass2
    {
        [Two]
        public void Meth (int d, string e, int f) { }
    }
}
namespace mdoc.Test
{
    /// <summary>
    /// Tests functions that update the EcmaXMl under various conditions from
    /// corresponding classes.
    /// </summary>
    [TestFixture ()]
    public class XmlUpdateTests
    {
        class ParamContext {
            public XmlDocument doc;
            public string beforeXML;
            public MethodReference method;
            public List<ParameterDefinition> parameters;
            public MDocUpdater updater;
            public FrameworkIndex fx;
        }

        #region Myclass Tests
        [Test ()]
        public void Parameters_Updating_Normal ()
        {
            var context = InitContext <MyClass>(startingEmptyXml, 0);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[0].Types.First ();
            bool fxAlternateTriggered = false;

            context.updater.MakeParameters (context.doc.FirstChild as XmlElement, context.method, context.parameters, typeEntry, ref fxAlternateTriggered);

            var afterXML = context.doc.OuterXml;

            Assert.AreEqual (Normalize(normalSingleXml), afterXML);

        }

        [Test ()]
        public void Parameters_Updating_Normal2 ()
        {
            var context = InitContext <MyClass>(normalSingleXml,1);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[1].Types.First ();
            bool fxAlternateTriggered = false;

            context.updater.MakeParameters (context.doc.FirstChild as XmlElement, context.method, context.parameters, typeEntry, ref fxAlternateTriggered);

            var afterXML = context.doc.OuterXml;

            Assert.AreEqual (Normalize (normalSingleXml), afterXML);

        }

        [Test ()]
        public void Parameters_Updating_Normal3 ()
        {
            var context = InitContext <MyClass>(normalSingleXml, 2);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[2].Types.First ();
            bool fxAlternateTriggered = false;

            context.updater.MakeParameters (context.doc.FirstChild as XmlElement, context.method, context.parameters, typeEntry, ref fxAlternateTriggered);

            var afterXML = context.doc.OuterXml;

            Assert.AreEqual (Normalize (multiFrameworkXml), afterXML);

        }

        [Test ()]
        public void Parameters_Updating_Existing_MultiFramework ()
        {
            var context = InitContext <MyClass>(multiFrameworkXml, 0);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[0].Types.First ();
            bool fxAlternateTriggered = false;

            context.updater.MakeParameters (context.doc.FirstChild as XmlElement, context.method, context.parameters, typeEntry, ref fxAlternateTriggered);

            var afterXML = context.doc.OuterXml;

            Assert.AreEqual (context.beforeXML, Normalize(afterXML));

        }

        [Test ()]
        public void Parameters_Updating_Existing_MultiFramework2 ()
        {
            var context = InitContext <MyClass>(multiFrameworkXml, 2);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[2].Types.First ();
            bool fxAlternateTriggered = false;

            context.updater.MakeParameters (context.doc.FirstChild as XmlElement, context.method, context.parameters, typeEntry, ref fxAlternateTriggered);

            var afterXML = context.doc.OuterXml;

            Assert.AreEqual (context.beforeXML, Normalize (afterXML));

        }



        [Test ()]
        public void Parameters_Updating_Existing_Align ()
        {
            var context = InitContext<MyClass> (multiFrameworkXml, 2, forceAlignment: true);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[2].Types.First ();
            bool fxAlternateTriggered = false;

            context.updater.MakeParameters (context.doc.FirstChild as XmlElement, context.method, context.parameters, typeEntry, ref fxAlternateTriggered);

            var afterXML = context.doc.OuterXml;

            Assert.IsFalse (fxAlternateTriggered);
            Assert.AreEqual (Normalize (multiFrameworkAligned), Normalize (afterXML));

        }
        #endregion

        #region MyClass2 tests

        [Test ()]
        public void Parameters2_Updating_Normal ()
        {
            var context = InitContext<MyClass2> (startingEmptyXml, 0);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[0].Types.First ();
            bool fxAlternateTriggered = false;

            context.updater.MakeParameters (context.doc.FirstChild as XmlElement, context.method, context.parameters, typeEntry, ref fxAlternateTriggered);

            var afterXML = context.doc.OuterXml;

            Assert.AreEqual (Normalize (normalSingleXml2), afterXML);

        }

        [Test ()]
        public void Parameters_Updating_Normal22 ()
        {
            var context = InitContext<MyClass2> (normalSingleXml2, 1);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[1].Types.First ();
            bool fxAlternateTriggered = false;

            context.updater.MakeParameters (context.doc.FirstChild as XmlElement, context.method, context.parameters, typeEntry, ref fxAlternateTriggered);

            var afterXML = context.doc.OuterXml;

            Assert.AreEqual (Normalize (multiFrameworkXml2_Second), afterXML);

        }

        [Test ()]
        public void Parameters_Updating_Normal32 ()
        {
            var context = InitContext<MyClass2> (multiFrameworkXml2_Second, 2);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[2].Types.First ();
            bool fxAlternateTriggered = false;

            context.updater.MakeParameters (context.doc.FirstChild as XmlElement, context.method, context.parameters, typeEntry, ref fxAlternateTriggered);

            var afterXML = context.doc.OuterXml;

            Assert.AreEqual (Normalize (multiFrameworkXml2), afterXML);

        }

        [Test ()]
        public void Parameters_Updating_Existing_MultiFramework22 ()
        {
            var context = InitContext<MyClass2> (multiFrameworkXml2, 0);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[0].Types.First ();
            bool fxAlternateTriggered = false;

            context.updater.MakeParameters (context.doc.FirstChild as XmlElement, context.method, context.parameters, typeEntry, ref fxAlternateTriggered);

            var afterXML = context.doc.OuterXml;

            Assert.AreEqual (context.beforeXML, Normalize (afterXML));

        }

        [Test ()]
        public void Parameters_Updating_Existing_MultiFramework222 ()
        {
            var context = InitContext<MyClass2> (multiFrameworkXml2, 2);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[2].Types.First ();
            bool fxAlternateTriggered = false;

            context.updater.MakeParameters (context.doc.FirstChild as XmlElement, context.method, context.parameters, typeEntry, ref fxAlternateTriggered);

            var afterXML = context.doc.OuterXml;

            Assert.AreEqual (context.beforeXML, Normalize (afterXML));

        }



        [Test ()]
        public void Parameters_Updating_Existing_Align2 ()
        {
            var context = InitContext<MyClass2> (multiFrameworkXml2, 1, forceAlignment: true);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[1].Types.First ();
            bool fxAlternateTriggered = false;

            context.updater.MakeParameters (context.doc.FirstChild as XmlElement, context.method, context.parameters, typeEntry, ref fxAlternateTriggered);

            var afterXML = context.doc.OuterXml;

            Assert.IsFalse (fxAlternateTriggered);
            Assert.AreEqual (Normalize (multiFrameworkAligned2), Normalize (afterXML));

        }
        #endregion

        #region Simple Parameter Change across multi FX

        [Test ()]
        public void Parameters_Updating_Existing_NameChange ()
        {
            var context = InitContext<MyClass2> (normalSingleXml2, 1, forceAlignment: false);

            Func<int, FrameworkTypeEntry> typeEntry = i => context.fx.Frameworks[i].Types.First ();
            bool fxAlternateTriggered = false;
            context.updater.MakeParameters (context.doc.FirstChild as XmlElement, context.method, context.parameters, typeEntry(0), ref fxAlternateTriggered);
            Assert.IsTrue (fxAlternateTriggered);

            fxAlternateTriggered = false;
            context.updater.MakeParameters (context.doc.FirstChild as XmlElement, context.method, context.parameters, typeEntry (1), ref fxAlternateTriggered);
            Assert.IsFalse (fxAlternateTriggered);

            fxAlternateTriggered = false;
            context.updater.MakeParameters (context.doc.FirstChild as XmlElement, context.method, context.parameters, typeEntry (2), ref fxAlternateTriggered);
            Assert.IsFalse (fxAlternateTriggered);

            var afterXML = context.doc.OuterXml;
            Assert.AreEqual (Normalize (multiFrameworkAlignedOther), Normalize (afterXML));

        }
        #endregion


        [Test ()]
        public void MemberSignature_Updating_Existing_Align ()
        {
            var context = InitContext <MyClass>(SigmultiFrameworkXml, 0, forceAlignment: true);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[0].Types.First ();


            var sig = new CSharpMemberFormatter ();
            MDocUpdater.UpdateSignature (sig, context.method, context.doc.FirstChild as XmlElement, typeEntry, fxAlternateTriggered:false);

            var afterXML = context.doc.OuterXml;
            // first framework looks like it already looked, so no need to update
            Assert.AreEqual (Normalize (SigmultiFrameworkXml), Normalize (afterXML));

        }

        [Test ()]
        public void MemberSignature_Updating_Existing_Align2 ()
        {
            var context = InitContext <MyClass>(SigmultiFrameworkXml, 2, forceAlignment: true);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[2].Types.First ();


            var sig = new CSharpMemberFormatter ();
            MDocUpdater.UpdateSignature (sig, context.method, context.doc.FirstChild as XmlElement, typeEntry, fxAlternateTriggered: false);

            var afterXML = context.doc.OuterXml;

            Assert.AreEqual (Normalize (SigmultiFrameworkAligned), Normalize (afterXML));

        }

        [Test ()]
        public void MemberSignature_Updating_Existing_NoChange ()
        {
            var context = InitContext <MyClass>(SigmultiFrameworkXml, 2, forceAlignment: false);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[2].Types.First ();


            var sig = new CSharpMemberFormatter ();
            MDocUpdater.UpdateSignature (sig, context.method, context.doc.FirstChild as XmlElement, typeEntry, fxAlternateTriggered: false);

            var afterXML = context.doc.OuterXml;

            Assert.AreEqual (Normalize (SigmultiFrameworkXml), Normalize (afterXML));

        }


        [Test ()]
        public void MemberSignature_Updating_Existing_NoChange_regular ()
        {
            var context = InitContext<MyClass> (SigRegular, 1, forceAlignment: false);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[1].Types.First ();


            var sig = new CSharpMemberFormatter ();
            MDocUpdater.UpdateSignature (sig, context.method, context.doc.FirstChild as XmlElement, typeEntry, fxAlternateTriggered: false);

            var afterXML = context.doc.OuterXml;

            Assert.AreEqual (Normalize (SigRegular), Normalize (afterXML));

        }



        [Test ()]
        public void MemberSignature_Updating_Existing_NameChanged_SingleFX()
        {
            // handles the case 
            var context = InitContext<MyClass> (SigRegular, 2, forceAlignment: false);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[0].Types.First ();
            context.fx.Frameworks.RemoveAt (2);
            context.fx.Frameworks.RemoveAt (1);

            var sig = new CSharpMemberFormatter ();
            MDocUpdater.UpdateSignature (sig, context.method, context.doc.FirstChild as XmlElement, typeEntry, fxAlternateTriggered: true);

            var afterXML = context.doc.OuterXml;

            Assert.AreEqual (Normalize (SigRegularChanged), Normalize (afterXML));

        }

        [Test ()]
        public void MemberSignature_Updating_Existing_NameChanged_MultiFX ()
        {
            // handles the case 
            var context = InitContext<MyClass> (SigRegular, 2, forceAlignment: false);

            Func<int, FrameworkTypeEntry> typeEntry = i => context.fx.Frameworks[i].Types.First ();

            var sig = new CSharpMemberFormatter ();
            MDocUpdater.UpdateSignature (sig, context.method, context.doc.FirstChild as XmlElement, typeEntry(0), fxAlternateTriggered: true);
            MDocUpdater.UpdateSignature (sig, context.method, context.doc.FirstChild as XmlElement, typeEntry(1), fxAlternateTriggered: false);
            MDocUpdater.UpdateSignature (sig, context.method, context.doc.FirstChild as XmlElement, typeEntry(2), fxAlternateTriggered: false);

            var afterXML = context.doc.OuterXml;

            Assert.AreEqual (Normalize (SigRegularAllAligned), Normalize (afterXML));

        }

        [Test ()]
        public void DocMemberEnumerator()
        {
            var context = InitContext <MyClass>(string.Format (typeFrameXml, multiFrameworkXml), 1, forceAlignment: true);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[1].Types.First ();

            var enumerator = new DocumentationEnumerator ();
            var matches = enumerator.GetDocumentationMembers (context.doc, context.method.DeclaringType.Resolve (), typeEntry);

            Assert.IsTrue (matches.Any (m => m.Member == context.method && m.Node != null), "didn't match the member");
        }

        [Test ()]
        public void DocMemberEnumerator2 ()
        {
            var context = InitContext <MyClass>(string.Format (typeFrameXml, multiFrameworkXml), 2, forceAlignment: true);

            FrameworkTypeEntry typeEntry = context.fx.Frameworks[2].Types.First ();

            var enumerator = new DocumentationEnumerator ();
            var matches = enumerator.GetDocumentationMembers (context.doc, context.method.DeclaringType.Resolve (), typeEntry);

            Assert.IsTrue (matches.Any (m => m.Member == context.method && m.Node != null), "didn't match the member");
        }

        [Test]
        public void Attributes_TypeOrMethod() 
        {
            var context = InitContext<MyClass> (string.Format (typeFrameXml, multiFrameworkXml), 2, forceAlignment: false);
            var fx = context.fx.Frameworks[1];
            FrameworkTypeEntry typeEntry = fx.Types.First ();

            string[] attributeList = new[] { "One" };

            MDocUpdater.MakeAttributes (context.doc.FirstChild as XmlElement, attributeList, fx, context.method.DeclaringType, typeEntry);
            var attrNode = context.doc.FirstChild.SelectSingleNode ("Attributes");
            var attributes = attrNode.SelectNodes ("Attribute").Cast<XmlElement>().ToArray();

            Assert.IsTrue (attributes.Count () == 1);
            Assert.AreEqual ("One", attributes[0].FirstChild.InnerText);
            Assert.AreEqual ("Three", attributes[0].GetAttribute (Consts.FrameworkAlternate));
        }

        [Test]
        public void Attributes_TypeOrMethod_AllFX ()
        {
            var context = InitContext<MyClass> (string.Format (typeFrameXml, multiFrameworkXml), 2, forceAlignment: false);

            foreach (var fx in context.fx.Frameworks)
            {
                //var fx = context.fx.Frameworks[1];
                FrameworkTypeEntry typeEntry = fx.Types.First ();

                string[] attributeList = new[] { "One" };

                MDocUpdater.MakeAttributes (context.doc.FirstChild as XmlElement, attributeList, fx, context.method.DeclaringType, typeEntry);
            }

            var attrNode = context.doc.FirstChild.SelectSingleNode ("Attributes");
            var attributes = attrNode.SelectNodes ("Attribute").Cast<XmlElement> ().ToArray ();

            Assert.IsTrue (attributes.Count () == 1);
            Assert.AreEqual ("One", attributes[0].FirstChild.InnerText);
            Assert.IsFalse (attributes[0].HasAttribute (Consts.FrameworkAlternate));
        }

        [Test]
        public void Attributes_TypeOrMethod_AllFX_OneMissing ()
        {
            var context = InitContext<MyClass> (string.Format (typeFrameXml, multiFrameworkXml), 2, forceAlignment: false);

            foreach (var fx in context.fx.Frameworks)
            {
                //var fx = context.fx.Frameworks[1];
                FrameworkTypeEntry typeEntry = fx.Types.First ();

                string[] attributeList = new[] { "One" };

                if (fx.IsFirstFramework)
                    attributeList = new string[0];

                MDocUpdater.MakeAttributes (context.doc.FirstChild as XmlElement, attributeList, fx, context.method.DeclaringType, typeEntry);
            }

            var attrNode = context.doc.FirstChild.SelectSingleNode ("Attributes");
            var attributes = attrNode.SelectNodes ("Attribute").Cast<XmlElement> ().ToArray ();

            Assert.IsTrue (attributes.Count () == 1);
            Assert.AreEqual ("One", attributes[0].FirstChild.InnerText);
            Assert.IsTrue (attributes[0].HasAttribute (Consts.FrameworkAlternate));
            Assert.AreEqual ("Three;Two", attributes[0].GetAttribute (Consts.FrameworkAlternate));
        }

        [Test]
        public void Attributes_TypeOrMethod_AllFX_OneMissing_Last ()
        {
            var context = InitContext<MyClass> (string.Format (typeFrameXml, multiFrameworkXml), 2, forceAlignment: false);

            foreach (var fx in context.fx.Frameworks)
            {
                //var fx = context.fx.Frameworks[1];
                FrameworkTypeEntry typeEntry = fx.Types.First ();

                string[] attributeList = new[] { "One" };

                if (fx.IsLastFramework)
                    attributeList = new string[0];

                MDocUpdater.MakeAttributes (context.doc.FirstChild as XmlElement, attributeList, fx, context.method.DeclaringType, typeEntry);
            }

            var attrNode = context.doc.FirstChild.SelectSingleNode ("Attributes");
            var attributes = attrNode.SelectNodes ("Attribute").Cast<XmlElement> ().ToArray ();

            Assert.IsTrue (attributes.Count () == 1);
            Assert.AreEqual ("One", attributes[0].FirstChild.InnerText);
            Assert.IsTrue (attributes[0].HasAttribute (Consts.FrameworkAlternate));
            Assert.AreEqual ("One;Three", attributes[0].GetAttribute (Consts.FrameworkAlternate));
        }

        [Test]
        public void Attributes_TypeOrMethod_AllFX_RunExisting_Middle ()
        {
            var context = InitContext<MyClass> (string.Format (typeFrameXml, multiFrameworkXml), 2, forceAlignment: false);

            // first, go through and add "One" and "Two" to all of them
            foreach (var fx in context.fx.Frameworks)
            {
                FrameworkTypeEntry typeEntry = fx.Types.First ();

                string[] attributeList = new[] { "One", "Two" };

                MDocUpdater.MakeAttributes (context.doc.FirstChild as XmlElement, attributeList, fx, context.method.DeclaringType, typeEntry);
            }

            // Now, to test the first deployment on an existing set
            // in this case, the truth of the matter is that `Two` only exists in the middle
            foreach (var fx in context.fx.Frameworks)
            {
                FrameworkTypeEntry typeEntry = fx.Types.First ();

                string[] attributeList = new[] { "One" };

                if (!fx.IsFirstFramework && !fx.IsLastFramework) {
                    attributeList = new[] { "One", "Two" };
                }

                MDocUpdater.MakeAttributes (context.doc.FirstChild as XmlElement, attributeList, fx, context.method.DeclaringType, typeEntry);
            }

            var attrNode = context.doc.FirstChild.SelectSingleNode ("Attributes");
            var attributes = attrNode.SelectNodes ("Attribute").Cast<XmlElement> ().ToArray ();

            Assert.IsTrue (attributes.Count () == 2);
            Assert.AreEqual ("One", attributes[0].FirstChild.InnerText);
            Assert.IsFalse (attributes[0].HasAttribute (Consts.FrameworkAlternate));
            Assert.AreEqual ("Two", attributes[1].FirstChild.InnerText);
            Assert.IsTrue (attributes[1].HasAttribute (Consts.FrameworkAlternate));
            Assert.AreEqual ("Three", attributes[1].GetAttribute (Consts.FrameworkAlternate));
        }

        [Test]
        public void Attributes_TypeOrMethod_AllFX_RunExisting_First ()
        {
            var context = InitContext<MyClass> (string.Format (typeFrameXml, multiFrameworkXml), 2, forceAlignment: false);

            // first, go through and add "One" and "Two" to all of them
            foreach (var fx in context.fx.Frameworks)
            {
                FrameworkTypeEntry typeEntry = fx.Types.First ();

                string[] attributeList = new[] { "One", "Two" };

                MDocUpdater.MakeAttributes (context.doc.FirstChild as XmlElement, attributeList, fx, context.method.DeclaringType, typeEntry);
            }

            // Now, to test the first deployment on an existing set
            // in this case, the truth of the matter is that `Two` only exists in the middle
            foreach (var fx in context.fx.Frameworks)
            {
                FrameworkTypeEntry typeEntry = fx.Types.First ();

                string[] attributeList = new[] { "One" };

                if (fx.IsFirstFramework)
                {
                    attributeList = new[] { "One", "Two" };
                }

                MDocUpdater.MakeAttributes (context.doc.FirstChild as XmlElement, attributeList, fx, context.method.DeclaringType, typeEntry);
            }

            var attrNode = context.doc.FirstChild.SelectSingleNode ("Attributes");
            var attributes = attrNode.SelectNodes ("Attribute").Cast<XmlElement> ().ToArray ();

            Assert.IsTrue (attributes.Count () == 2);
            Assert.AreEqual ("One", attributes[0].FirstChild.InnerText);
            Assert.IsFalse (attributes[0].HasAttribute (Consts.FrameworkAlternate));
            Assert.AreEqual ("Two", attributes[1].FirstChild.InnerText);
            Assert.IsTrue (attributes[1].HasAttribute (Consts.FrameworkAlternate));
            Assert.AreEqual ("One", attributes[1].GetAttribute (Consts.FrameworkAlternate));
        }

        [Test]
        public void Attributes_TypeOrMethod_AllFX_RunExisting_Last ()
        {
            var context = InitContext<MyClass> (string.Format (typeFrameXml, multiFrameworkXml), 2, forceAlignment: false);

            // first, go through and add "One" and "Two" to all of them
            foreach (var fx in context.fx.Frameworks)
            {
                FrameworkTypeEntry typeEntry = fx.Types.First ();

                string[] attributeList = new[] { "One", "Two" };

                MDocUpdater.MakeAttributes (context.doc.FirstChild as XmlElement, attributeList, fx, context.method.DeclaringType, typeEntry);
            }

            // Now, to test the first deployment on an existing set
            // in this case, the truth of the matter is that `Two` only exists in the middle
            foreach (var fx in context.fx.Frameworks)
            {
                FrameworkTypeEntry typeEntry = fx.Types.First ();

                string[] attributeList = new[] { "Two" };

                if (fx.IsLastFramework)
                {
                    attributeList = new[] { "One", "Two" };
                }

                MDocUpdater.MakeAttributes (context.doc.FirstChild as XmlElement, attributeList, fx, context.method.DeclaringType, typeEntry);
            }

            var attrNode = context.doc.FirstChild.SelectSingleNode ("Attributes");
            var attributes = attrNode.SelectNodes ("Attribute").Cast<XmlElement> ().ToArray ();

            Assert.IsTrue (attributes.Count () == 2);
            Assert.AreEqual ("One", attributes[1].FirstChild.InnerText);
            Assert.IsTrue (attributes[1].HasAttribute (Consts.FrameworkAlternate));
            Assert.AreEqual ("Two", attributes[1].GetAttribute (Consts.FrameworkAlternate));
            Assert.AreEqual ("Two", attributes[0].FirstChild.InnerText);
            Assert.IsFalse (attributes[0].HasAttribute (Consts.FrameworkAlternate));

        }

        [Test]
        public void Attributes_TypeOrMethod_AllFX_OneMissing_Middle ()
        {
            var context = InitContext<MyClass> (string.Format (typeFrameXml, multiFrameworkXml), 2, forceAlignment: false);

            foreach (var fx in context.fx.Frameworks)
            {
                //var fx = context.fx.Frameworks[1];
                FrameworkTypeEntry typeEntry = fx.Types.First ();

                string[] attributeList = new[] { "One" };

                if (!fx.IsLastFramework && !fx.IsFirstFramework)
                    attributeList = new string[0];

                MDocUpdater.MakeAttributes (context.doc.FirstChild as XmlElement, attributeList, fx, context.method.DeclaringType, typeEntry);
            }

            var attrNode = context.doc.FirstChild.SelectSingleNode ("Attributes");
            var attributes = attrNode.SelectNodes ("Attribute").Cast<XmlElement> ().ToArray ();

            Assert.IsTrue (attributes.Count () == 1);
            Assert.AreEqual ("One", attributes[0].FirstChild.InnerText);
            Assert.IsTrue (attributes[0].HasAttribute (Consts.FrameworkAlternate));
            Assert.AreEqual ("One;Two", attributes[0].GetAttribute (Consts.FrameworkAlternate));
        }


        [Test]
        public void Attributes_Assembly ()
        {
            var context = InitContext<MyClass> (string.Format (typeFrameXml, multiFrameworkXml), 2, forceAlignment: false);

            foreach (var fx in context.fx.Frameworks)
            {
                FrameworkTypeEntry typeEntry = fx.Types.First ();

                string[] attributeList = new[] { "One" };
                string assemblyName = "one.dll";
                if (!fx.IsLastFramework && !fx.IsFirstFramework)
                {
                    attributeList = new string[0];
                    assemblyName = "three.dll";
                }


                MDocUpdater.MakeAttributes (context.doc.FirstChild as XmlElement, attributeList, fx,

                                            assemblyFilename: "three.dll");
            }

            var attrNode = context.doc.FirstChild.SelectSingleNode ("Attributes");
            var attributes = attrNode.SelectNodes ("Attribute").Cast<XmlElement> ().ToArray ();

            Assert.IsTrue (attributes.Count () == 1);
            Assert.AreEqual ("One", attributes[0].FirstChild.InnerText);
            Assert.IsTrue (attributes[0].HasAttribute (Consts.FrameworkAlternate));
            Assert.AreEqual ("One;Two", attributes[0].GetAttribute (Consts.FrameworkAlternate));
        }

        [Test]
        public void Attributes_Assembly_OtherAssembly ()
        {
            var context = InitContext<MyClass> (string.Format (typeFrameXml, multiFrameworkXml), 2, forceAlignment: false);

            var fx = context.fx.Frameworks[1];


                FrameworkTypeEntry typeEntry = fx.Types.First ();

                string[] attributeList = new[] { "One" };
                
                // this is the 'second' fx, and we've changed the expected assembly name, 
                // so the attribute, while it doesn't exist yet, shouldn't have an FX made since it doesn't exist in any other FX
                MDocUpdater.MakeAttributes (context.doc.FirstChild as XmlElement, attributeList, fx,
                                            assemblyFilename: "three.dll");
            

            var attrNode = context.doc.FirstChild.SelectSingleNode ("Attributes");
            var attributes = attrNode.SelectNodes ("Attribute").Cast<XmlElement> ().ToArray ();

            Assert.IsTrue (attributes.Count () == 1);
            Assert.AreEqual ("One", attributes[0].FirstChild.InnerText);
            Assert.IsFalse (attributes[0].HasAttribute (Consts.FrameworkAlternate));
        }

        string Normalize(string xml) {
            XmlDocument doc = new XmlDocument ();

            doc.LoadXml (xml);
            return doc.OuterXml;
        }
        string typeFrameXml = @"<Type><Members>{0}</Members></Type>";

        #region MyClass XML
        string startingEmptyXml = @"<Member MemberName=""Meth"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.Void</ReturnType>
      </ReturnValue>
      <Parameters>
      </Parameters>
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>";
        string normalSingleXml = @"<Member MemberName=""Meth"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.Void</ReturnType>
      </ReturnValue>
      <Parameters>
      <Parameter Name = ""a"" Type=""System.Int32"" />
          <Parameter Name = ""d"" Type=""System.String"" />
          <Parameter Name = ""c"" Type=""System.Int32"" />
      </Parameters>
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>";

        string multiFrameworkXml = @"<Member MemberName=""Meth"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.Void</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""a"" Type=""System.Int32"" Index=""0"" />
        <Parameter Name = ""d"" Type=""System.String"" Index=""1"" FrameworkAlternate=""One;Three"" />
        <Parameter Name = ""b"" Type=""System.String"" Index=""1"" FrameworkAlternate=""Two"" />
        <Parameter Name = ""c"" Type=""System.Int32"" Index=""2"" />
      </Parameters>
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>";

        string multiFrameworkAligned = @"<Member MemberName=""Meth"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.Void</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""a"" Type=""System.Int32"" Index=""0"" />
        <Parameter Name = ""d"" Type=""System.String"" Index=""1"" FrameworkAlternate=""One;Three;Two"" />
        <Parameter Name = ""c"" Type=""System.Int32"" Index=""2"" />
      </Parameters>
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>";
        #endregion 

        #region MyClass2 XML

        string normalSingleXml2 = @"<Member MemberName=""Meth"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.Void</ReturnType>
      </ReturnValue>
      <Parameters>
          <Parameter Name = ""a"" Type=""System.Int32"" />
          <Parameter Name = ""b"" Type=""System.String"" />
          <Parameter Name = ""c"" Type=""System.Int32"" />
      </Parameters>
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>";

        string multiFrameworkXml2_Second = @"<Member MemberName=""Meth"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.Void</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""a"" Type=""System.Int32"" Index=""0"" FrameworkAlternate=""One"" />
        <Parameter Name = ""d"" Type=""System.Int32"" Index=""0"" FrameworkAlternate=""Three"" />
        <Parameter Name = ""b"" Type=""System.String"" Index=""1"" FrameworkAlternate=""One"" />
        <Parameter Name = ""e"" Type=""System.String"" Index=""1"" FrameworkAlternate=""Three"" />
        <Parameter Name = ""c"" Type=""System.Int32"" Index=""2"" FrameworkAlternate=""One"" />
        <Parameter Name = ""f"" Type=""System.Int32"" Index=""2"" FrameworkAlternate=""Three"" />
      </Parameters>
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>";

        string multiFrameworkXml2 = @"<Member MemberName=""Meth"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.Void</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""a"" Type=""System.Int32"" Index=""0"" FrameworkAlternate=""One;Two"" />
        <Parameter Name = ""d"" Type=""System.Int32"" Index=""0"" FrameworkAlternate=""Three"" />
        <Parameter Name = ""b"" Type=""System.String"" Index=""1"" FrameworkAlternate=""One;Two"" />
        <Parameter Name = ""e"" Type=""System.String"" Index=""1"" FrameworkAlternate=""Three"" />
        <Parameter Name = ""c"" Type=""System.Int32"" Index=""2"" FrameworkAlternate=""One;Two"" />
        <Parameter Name = ""f"" Type=""System.Int32"" Index=""2"" FrameworkAlternate=""Three"" />
      </Parameters>
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>";

        string multiFrameworkAligned2 = @"<Member MemberName=""Meth"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.Void</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""a"" Type=""System.Int32"" Index=""0"" FrameworkAlternate=""One;Two;Three"" />
        <Parameter Name = ""b"" Type=""System.String"" Index=""1"" FrameworkAlternate=""One;Two;Three"" />
        <Parameter Name = ""c"" Type=""System.Int32"" Index=""2"" FrameworkAlternate=""One;Two;Three"" />
      </Parameters>
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>";

        string multiFrameworkAlignedOther = @"<Member MemberName=""Meth"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.Void</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""d"" Type=""System.Int32"" Index=""0"" FrameworkAlternate=""One;Three;Two"" />
        <Parameter Name = ""e"" Type=""System.String"" Index=""1"" FrameworkAlternate=""One;Three;Two"" />
        <Parameter Name = ""f"" Type=""System.Int32"" Index=""2"" FrameworkAlternate=""One;Three;Two"" />
      </Parameters>
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>";

        #endregion

        string SigmultiFrameworkXml = @"<Member MemberName=""Meth"">
      <MemberSignature Language=""C#"" Value=""public void Meth (int a, string d, int c);"" FrameworkAlternate=""One;Three"" />
      <MemberSignature Language=""C#"" Value=""public void Meth (int a, string b, int c);"" FrameworkAlternate=""Two"" />
      
    </Member>";

        string SigmultiFrameworkAligned = @"<Member MemberName=""Meth"">
      <MemberSignature Language=""C#"" Value=""public void Meth (int a, string d, int c);"" FrameworkAlternate=""One;Three;Two"" />
      
    </Member>";

        string SigRegular = @"<Member MemberName=""Meth"">
      <MemberSignature Language=""C#"" Value=""public void Meth (int a, string d, int c);"" />
    </Member>";
        string SigRegularChanged = @"<Member MemberName=""Meth"">
      <MemberSignature Language=""C#"" Value=""public void Meth (int a, string b, int c);"" />
    </Member>";
        string SigRegularAllAligned = @"<Member MemberName=""Meth"">
      <MemberSignature Language=""C#"" Value=""public void Meth (int a, string b, int c);"" FrameworkAlternate=""One;Three;Two"" />
    </Member>";



        private ParamContext InitContext <T>(string methodXml, int fxIndex, bool forceAlignment=false)
        {
            Func<int, bool> indexCheck = fi => fi < 2;
            if (typeof(T) == typeof(MyClass2))
                indexCheck = fi => fi != 1;
            
            bool useFirst = forceAlignment || indexCheck(fxIndex);
            var doc = new XmlDocument ();
            //doc.PreserveWhitespace = true;
            doc.LoadXml (methodXml);
            var beforeXML = doc.OuterXml;
            XmlElement root = doc.SelectSingleNode ("//Docs") as XmlElement; // Docs

            TypeDefinition type = GetDefinition<T> ("mdoc.Test");
            var method = type.Methods.First (m => m.Name == "Meth") as MethodReference;
            var parameters = method.Parameters.ToList ();
            TypeDefinition type2 = GetDefinition<T> ("mdoc.Test2");
            var method2 = type2.Methods.First (m => m.Name == "Meth") as MethodReference;
            var parameters2 = method2.Parameters.ToList ();

            // updater
            var updater = new MDocUpdater ();
            var fx = new FrameworkIndex ("");
            fx.Frameworks.Add (new FrameworkEntry (fx.Frameworks) { Id = "One", Name = "One", Replace="mdoc.Test2", With="mdoc.Test" });
            fx.Frameworks.Add (new FrameworkEntry (fx.Frameworks) { Id = "Three", Name = "Three", Replace = "mdoc.Test2", With = "mdoc.Test"  });
            fx.Frameworks.Add (new FrameworkEntry (fx.Frameworks) { Id = "Two", Name = "Two", Replace = "mdoc.Test2", With = "mdoc.Test"  });

            var i = 0;
            foreach (var f in fx.Frameworks)
            {
                if (indexCheck(i) || forceAlignment)
                {
                    var t = f.ProcessType (type);
                    t.ProcessMember (method);

                    var aset = new AssemblySet (new[] { "one.dll" });
                    f.AddAssemblySet (aset);

                }
                else {
                    var t = f.ProcessType (type2);
                    t.ProcessMember (method2);
                }
                i++;
            }

            updater.InitializeFrameworksCache (fx);

            return new ParamContext ()
            {
                doc = doc,
                beforeXML = beforeXML,
                method = useFirst ? method : method2,
                parameters = useFirst ? parameters : parameters2,
                fx = fx,
                updater = updater
            };
        }

        TypeDefinition GetDefinition<T>(string ns) 
        {
            Type t = typeof (T);
            string path = t.Assembly.Location;
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly (path);
            return assembly.MainModule.Types.First (type => 
                                                    type.Name == t.Name && type.Namespace == ns);
        }
    }
}
