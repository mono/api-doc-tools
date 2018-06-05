using Mono.Documentation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;

namespace mdoc.Test
{
    [TestFixture ()]
    public class ValidationTests
    {

        [Test ()]
        public void ValidationFailure ()
        {
            var context = InitializeTestContext ();

            context.Validator.ValidateFile (new StringReader ("<random></random>"));

            Assert.AreEqual (1, context.Errors.Count);
            Assert.IsTrue (context.Errors.Any (e => e.Message.Contains ("random")));
        }

        [Test ()]
        public void ValidationSuccess ()
        {
            var context = InitializeTestContext ();

            context.Validator.ValidateFile (new StringReader ("<Type Name=\"AVKitError\" FullName=\"AVKit.AVKitError\"></Type>"));

            Assert.AreEqual (0, context.Errors.Count);
        }

        [Test]
        public void FrameworkAlternate_Attributes_Type ()
        {
            string xmlString = @"<Type Name=""AVKitError"" FullName=""AVKit.AVKitError"">
  <Attributes>
    <Attribute>
      <AttributeName>ObjCRuntime.Introduced(ObjCRuntime.PlatformName.iOS, 9, 0, ObjCRuntime.PlatformArchitecture.None, null)</AttributeName>
    </Attribute>
    <Attribute FrameworkAlternate=""One;Two"">
      <AttributeName>ObjCRuntime.Native</AttributeName>
    </Attribute>
  </Attributes>
</Type>";


            var context = InitializeTestContext ();

            context.Validator.ValidateFile (new StringReader (xmlString));


            Assert.AreEqual (0, context.Errors.Count, context.ErrorText);
        }

        [Test]
        public void FrameworkAlternate_MemberSignature ()
        {
            string xmlString = @"<Type Name=""blah"" FullName=""blah.blah"">
            <Members><Member MemberName=""Meth"">
      <MemberSignature Language=""C#"" Value=""public void Meth (int a, string d, int c);"" FrameworkAlternate=""One;Three"" />
      <MemberSignature Language=""C#"" Value=""public void Meth (int a, string b, int c);"" FrameworkAlternate=""Two"" />
      <MemberType>b</MemberType>
      <Docs></Docs>
    </Member></Members></Type>";


            var context = InitializeTestContext ();

            context.Validator.ValidateFile (new StringReader (xmlString));


            Assert.AreEqual (0, context.Errors.Count, context.ErrorText);
        }

        [Test]
        public void FrameworkAlternate_TypeSignature ()
        {
            string xmlString = @"<Type Name=""blah"" FullName=""blah.blah"">
      <TypeSignature Language=""C#"" Value=""blah"" FrameworkAlternate=""One;Three"" />
      <TypeSignature Language=""C#"" Value=""blah2"" FrameworkAlternate=""Two"" />
</Type>";


            var context = InitializeTestContext ();

            context.Validator.ValidateFile (new StringReader (xmlString));


            Assert.AreEqual (0, context.Errors.Count, context.ErrorText);
        }

        [Test]
        public void FrameworkAlternate_Parameter ()
        {
            string xmlString = @"<Member MemberName=""Meth"">
      <MemberSignature Language=""C#"" Value=""blah"" />
      <MemberType>Method</MemberType>
      <Parameters>
        <Parameter Name = ""a"" Type=""System.Int32"" Index=""0"" FrameworkAlternate=""One"" />
        <Parameter Name = ""d"" Type=""System.Int32"" Index=""0"" FrameworkAlternate=""Three"" />
      </Parameters>
      <Docs></Docs>
    </Member>";


            var context = InitializeTestContext ();

            context.Validator.ValidateFile (new StringReader (xmlString));


            Assert.AreEqual (0, context.Errors.Count, context.ErrorText);
        }

        #region Test Context Stuff
        struct ValidationContext
        {
            public MDocValidator Validator;
            public List<Exception> Errors;
            public string ErrorText{
                get => Errors.Aggregate (
                    new StringBuilder (),
                    (sb, e) =>
                    {
                        sb.Append (e.Message + ";");
                        return sb;
                    }
                    ).ToString ();
            }
        }

        private static ValidationContext InitializeTestContext ()
        {
            List<Exception> errors = new List<Exception> ();

            MDocValidator validator = new MDocValidator ();
            validator.InitializeSchema (
                "ecma",
                (a, b) => errors.Add (b.Exception)
            );
            ValidationContext context = new ValidationContext
            {
                Validator = validator,
                Errors = errors
            };
            return context;
        }
        #endregion
    }
}
