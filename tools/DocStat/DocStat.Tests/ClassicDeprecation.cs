using NUnit.Framework;
using System;
using System.Linq;
using System.Xml.Linq;

using DocStat;
namespace DocStat.Tests
{
	[TestFixture ()]
	public class ClassicDeprecation
	{
		[Test ()]
		public void TypeSummary ()
		{
			TestTransferOfElement ("Type", "Docs", "summary");
		}

		[Test ()]
		public void TypeRemarks ()
		{
			TestTransferOfElement ("Type", "Docs", "remarks");
		}

		[Test ()]
		public void MemberSummaries ()
		{
			// Load the XML docs
			var classic = Data.Load (Data.ClassicXml);
			var unified = Data.Load (Data.UnifiedXml);

			var classicMembers = classic.Element ("Type").Elements ("Members").Elements ("Member")
										.Where (m =>
										{
											var apistyle = m.Attribute ("apistyle");
											return apistyle == null || apistyle.Value != "classic";
										});
			foreach (var member in classicMembers)
			{
				var getResult = FixElement (member, unified, "Docs", "summary");
				var result = getResult ();

				XElement classicElement = result.Item1, unifiedElement = result.Item2;
				Assert.IsTrue (XNode.DeepEquals (classicElement, unifiedElement));

				// memberr remarks
				getResult = FixElement (member, unified, "Docs", "remarks");
				getResult = FixElement (member, unified, "Docs", "returns");
				//getResult = FixElements (member, unified, "Docs", "param");
				//getResult = FixElements (member, unified, "Docs", "typeparam");

				// assert that the docs were transferred
			}
		}

		private static void TestTransferOfElement (params string[] query)
		{
			// Load the XML docs
			var classic = Data.Load (Data.ClassicXml);
			var unified = Data.Load (Data.UnifiedXml);

			var getResult = FixElement (classic, unified, query);

			var result = getResult ();
			XElement classicElement = result.Item1, unifiedElement = result.Item2;

			// assert that the docs were transferred
			Assert.IsTrue (XNode.DeepEquals (classicElement, unifiedElement));
		}

		/// <summary>Transfers the element referred to by the query</summary>
		/// <returns>A function that you can use to retrieve the post-fix elements, for comparison</returns>
		/// <param name="classic">Classic XDocument. The source of the docs</param>
		/// <param name="unified">Unified XDocument. The target of the docs</param>
		/// <param name="query">The path to the target element</param>
		private static Func<Tuple<XElement, XElement>> FixElement (XDocument classic, XDocument unified, params string[] query)
		{
			var classicElement = classic.Element (query);
			return FixElement (classicElement, unified);
		}
		/// <summary>Transfers the element referred to by the query</summary>
		/// <returns>A function that you can use to retrieve the post-fix elements, for comparison</returns>
		/// <param name="classicElement">Classic element.</param>
		/// <param name="unified">Unified.</param>
		/// <param name="query">Query.</param>
		private static Func<Tuple<XElement, XElement>> FixElement (XElement classicElement, XDocument unified, params string[] query)
		{
			if (query != null && query.Length > 0)
			{
				classicElement = classicElement.Element (query);
			}
			var selector = EcmaXmlHelper.GetSelectorFor (classicElement);

			// pick out the corresponding "new element"
			var unifiedElement = selector (unified);

			// FIX the docs
			if (classicElement.Attribute ("apistyle") != null)
				classicElement.Attribute ("apistyle").Remove ();

			EcmaXmlHelper.Fix (unifiedElement, classicElement);

			return () => new Tuple<XElement, XElement> (classicElement, selector (unified));
		}
	}

	/// <summary>Some data for the purposes of testing</summary>
	static class Data
	{
		public static XDocument Load (string xml) => XDocument.Parse (xml);

		/// <summary>Some XMl with "classic" elements</summary>
		public static string ClassicXml = @"<Type Name=""MTLOrigin"" FullName=""MonoTouch.Metal.MTLOrigin"">
  <TypeSignature Language=""C#"" Value=""public struct MTLOrigin"" />
  <TypeSignature Language=""ILAsm"" Value="".class public sequential ansi sealed beforefieldinit MTLOrigin extends System.ValueType"" />
  <AssemblyInfo apistyle=""classic"">
    <AssemblyName>monotouch</AssemblyName>
    <AssemblyVersion>0.0.0.0</AssemblyVersion>
  </AssemblyInfo>
  <AssemblyInfo apistyle=""unified"">
    <AssemblyName>Xamarin.iOS</AssemblyName>
    <AssemblyVersion>0.0.0.0</AssemblyVersion>
  </AssemblyInfo>
  <Base>
    <BaseTypeName>System.ValueType</BaseTypeName>
  </Base>
  <Interfaces />
  <Docs>
    <summary>The location of a pixel in an image or texture.</summary>
    <remarks>this has docs</remarks>
  </Docs>
  <Members>
    <Member MemberName="".ctor"">
      <MemberSignature Language=""C#"" Value=""public MTLOrigin (int x, int y, int z);"" apistyle=""classic"" />
      <MemberSignature Language=""ILAsm"" Value="".method public hidebysig specialname rtspecialname instance void .ctor(int32 x, int32 y, int32 z) cil managed"" apistyle=""classic"" />
      <MemberSignature Language=""C#"" Value=""public MTLOrigin (nint x, nint y, nint z);"" apistyle=""unified"" />
      <MemberSignature Language=""ILAsm"" Value="".method public hidebysig specialname rtspecialname instance void .ctor(valuetype System.nint x, valuetype System.nint y, valuetype System.nint z) cil managed"" apistyle=""unified"" />
      <MemberType>Constructor</MemberType>
      <AssemblyInfo apistyle=""classic"">
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <AssemblyInfo apistyle=""unified"">
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <Parameters>
        <Parameter Name=""x"" Type=""System.Int32"" apistyle=""classic"" />
        <Parameter Name=""y"" Type=""System.Int32"" apistyle=""classic"" />
        <Parameter Name=""z"" Type=""System.Int32"" apistyle=""classic"" />
        <Parameter Name=""x"" Type=""System.nint"" apistyle=""unified"" />
        <Parameter Name=""y"" Type=""System.nint"" apistyle=""unified"" />
        <Parameter Name=""z"" Type=""System.nint"" apistyle=""unified"" />
      </Parameters>
      <Docs>
        <param name=""x"">this has docs</param>
        <param name=""y"">this has docs</param>
        <param name=""z"">this has docs</param>
        <summary>this has docs</summary>
        <remarks>this has docs</remarks>
      </Docs>
    </Member>
    <Member MemberName=""SomeField"" apistyle=""classic"">
      <MemberSignature Language=""C#"" Value=""SomeField"" />
      <MemberSignature Language=""ILAsm"" Value="".field public static literal valuetype System.Int32 SomeField"" />
      <MemberType>Field</MemberType>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <ReturnValue>
        <ReturnType>System.Int32</ReturnType>
      </ReturnValue>
      <Docs>
        <summary>These should be thrown away</summary>
      </Docs>
    </Member>
   </Members>
</Type>";

		/// <summary>The new/empty XMl stubs for the same type</summary>
		public static string UnifiedXml = @"<Type Name=""MTLOrigin"" FullName=""Metal.MTLOrigin"">
  <TypeSignature Language=""C#"" Value=""public struct MTLOrigin"" />
  <TypeSignature Language=""ILAsm"" Value="".class public sequential ansi sealed beforefieldinit MTLOrigin extends System.ValueType"" />
  <AssemblyInfo>
    <AssemblyName>Xamarin.iOS</AssemblyName>
    <AssemblyVersion>0.0.0.0</AssemblyVersion>
  </AssemblyInfo>
  <Base>
    <BaseTypeName>System.ValueType</BaseTypeName>
  </Base>
  <Interfaces />
  <Docs>
    <summary>To be added.</summary>
    <remarks>To be added.</remarks>
  </Docs>
  <Members>
    <Member MemberName="".ctor"">
      <MemberSignature Language=""C#"" Value=""public MTLOrigin (nint x, nint y, nint z);"" />
      <MemberSignature Language=""ILAsm"" Value="".method public hidebysig specialname rtspecialname instance void .ctor(valuetype System.nint x, valuetype System.nint y, valuetype System.nint z) cil managed"" />
      <MemberType>Constructor</MemberType>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <Parameters>
        <Parameter Name=""x"" Type=""System.nint"" />
        <Parameter Name=""y"" Type=""System.nint"" />
        <Parameter Name=""z"" Type=""System.nint"" />
      </Parameters>
      <Docs>
        <param name=""x"">To be added.</param>
        <param name=""y"">To be added.</param>
        <param name=""z"">To be added.</param>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
   </Members>
</Type>";
	}
}