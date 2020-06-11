namespace mdoc.Test
{
    class XmlConsts
    {
        #region MyClass2 XML

        public const string NormalSingleXml2 = @"<Member MemberName=""Meth"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.Void</ReturnType>
      </ReturnValue>
      <Parameters>
          <Parameter Name = ""a"" Type=""System.Int32"" Index=""0"" FrameworkAlternate=""One"" />
          <Parameter Name = ""b"" Type=""System.String"" Index=""1"" FrameworkAlternate=""One"" />
          <Parameter Name = ""c"" Type=""System.Int32"" Index=""2"" FrameworkAlternate=""One"" />
      </Parameters>
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>";

        public const string NormalSingleXml = @"<Member MemberName=""Meth"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.Void</ReturnType>
      </ReturnValue>
      <Parameters>
          <Parameter Name = ""a"" Type=""System.Int32"" Index=""0"" FrameworkAlternate=""One"" />
          <Parameter Name = ""d"" Type=""System.String"" Index=""1"" FrameworkAlternate=""One"" />
          <Parameter Name = ""c"" Type=""System.Int32"" Index=""2"" FrameworkAlternate=""One"" />
      </Parameters>
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>";

        public const string MultiFrameworkXml2Second = @"<Member MemberName=""Meth"">
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

        public const string MultiFrameworkXml2 = @"<Member MemberName=""Meth"">
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

        public const string MultiFrameworkAligned2 = @"<Member MemberName=""Meth"">
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

        public const string MultiFrameworkAlignedOther = @"<Member MemberName=""Meth"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.Void</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""d"" Type=""System.Int32"" />
        <Parameter Name = ""e"" Type=""System.String"" />
        <Parameter Name = ""f"" Type=""System.Int32"" />
      </Parameters>
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>";

        public const string XML_METHOD_TESTMETHOD_BEFORE = @"<Member MemberName=""BeginRead"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.IAsyncResult</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""buffer"" Type=""System.Byte[]"" Index=""0"" FrameworkAlternate=""One"" />
        <Parameter Name = ""array"" Type=""System.Byte[]"" Index=""0"" FrameworkAlternate=""Two"" />
        <Parameter Name = ""offset"" Type=""System.Int32"" Index=""1"" FrameworkAlternate=""One;Two"" />
        <Parameter Name = ""count"" Type=""System.Int32"" Index=""2"" FrameworkAlternate=""One;Two"" />
        <Parameter Name = ""asyncCallback"" Type=""System.AsyncCallback"" Index=""3"" FrameworkAlternate=""One;Two"" />
        <Parameter Name = ""asyncState"" Type=""System.Object"" Index=""4"" FrameworkAlternate=""One;Two"" />
      </Parameters>
    </Member>";

        //byte[] buffer, int offset, int count, AsyncCallback cback, object state
        public const string XML_METHOD_TESTMETHOD_AFTER = @"<Member MemberName=""BeginRead"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.IAsyncResult</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""buffer"" Type=""System.Byte[]"" Index=""0"" FrameworkAlternate=""One;Three"" />
        <Parameter Name = ""array"" Type=""System.Byte[]"" Index=""0"" FrameworkAlternate=""Two"" />
        <Parameter Name = ""offset"" Type=""System.Int32"" Index=""1"" />
        <Parameter Name = ""count"" Type=""System.Int32"" Index=""2"" />
        <Parameter Name = ""asyncCallback"" Type=""System.AsyncCallback"" Index=""3"" FrameworkAlternate=""One;Two"" />
        <Parameter Name = ""cback"" Type=""System.AsyncCallback"" Index=""3"" FrameworkAlternate=""Three"" />
        <Parameter Name = ""asyncState"" Type=""System.Object"" Index=""4"" FrameworkAlternate=""One;Two"" />
        <Parameter Name = ""state"" Type=""System.Object"" Index=""4"" FrameworkAlternate=""Three"" />
      </Parameters>
    </Member>";

        #endregion

        #region Framework
        public const string FrameworkIndexXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Framework Name=""framework1"">
  <Namespace Name=""Namespace1"">
    <Type Name=""Type1"" Id=""T:Type1"">
      <Member Id=""M:Type1.#ctor"" />
    </Type>
    <Type Name=""Type2"" Id=""T:Type2"">
      <Member Id=""M:Type2.#ctor"" />
      <Member Id=""P:Type2.Property0"" />
    </Type>
  </Namespace>
</Framework>";

        public const string FrameworkIndexXml2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Framework Name=""framework1"">
  <Namespace Name=""Namespace1"">
    <Type Name=""Type1"" Id=""T:Type1"">
      <Member Id=""M:Type1.#ctor"" />
    </Type>
  </Namespace>
  <Namespace Name=""Namespace2"">
    <Type Name=""Type2"" Id=""T:Type2"">
      <Member Id=""M:Type2.#ctor"" />
      <Member Id=""P:Type2.Property0"" />
    </Type>
  </Namespace>
</Framework>";

        #endregion

        #region CheckDocidXml 

        public const string CheckDocidXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
        <Type Name=""MyClass"" FullName=""mdoc.Test2.MyClass"">
        <TypeSignature Language = ""C#"" Value=""public class MyClass"" />
        <TypeSignature Language = ""ILAsm"" Value="".class public auto ansi beforefieldinit MyClass extends System.Object"" />
        <TypeSignature Language = ""DocId"" Value=""T:mdoc.Test2.MyClass"" />
        <TypeSignature Language = ""VB.NET"" Value=""Public Class MyClass"" />
        <TypeSignature Language = ""C++ CLI"" Value=""public ref class MyClass"" />
        <TypeSignature Language = ""F#"" Value=""type MyClass = class"" />
        <AssemblyInfo>
            <AssemblyName>mdoc.Test</AssemblyName>
            <AssemblyVersion>0.0.0.0</AssemblyVersion>
        </AssemblyInfo>
        <Base>
            <BaseTypeName>System.Object</BaseTypeName>
        </Base>
        <Interfaces />
        <Attributes>
        <Attribute>
        <AttributeName>mdoc.Test.One</AttributeName>
        </Attribute>
        <Attribute>
        <AttributeName>mdoc.Test.Two</AttributeName>
        </Attribute>
        </Attributes>
        <Docs>
          <summary>To be added.</summary>
          <remarks>To be added.</remarks>
        </Docs>
        <Members>
          <Member MemberName = "".ctor"" >
          <MemberSignature Language=""C#"" Value=""public MyClass ();"" />
          <MemberSignature Language = ""ILAsm"" Value="".method public hidebysig specialname rtspecialname instance void .ctor() cil managed"" />
          <MemberSignature Language = ""VB.NET"" Value=""Public Sub New ()"" />
          <MemberSignature Language = ""C++ CLI"" Value=""public:&#xA; MyClass();"" />
          <MemberSignature Language = ""C++ CX"" Value=""public:&#xA; MyClass();"" />
          <MemberType>Constructor</MemberType>
          <AssemblyInfo>
            <AssemblyVersion>0.0.0.0</AssemblyVersion>
          </AssemblyInfo>
          <Parameters />
          <Docs>
            <summary>To be added.</summary>
            <remarks>To be added.</remarks>
          </Docs>
        </Member>
        <Member MemberName=""op_Implicit"">
       <MemberSignature Language = ""C#"" Value=""public static implicit operator int (System.Printing.IndexedProperties.PrintInt32Property attribRef);"" />
       <MemberSignature Language = ""ILAsm"" Value="".method public static hidebysig specialname int32 op_Implicit(class System.Printing.IndexedProperties.PrintInt32Property attribRef) cil managed"" />
       <MemberSignature Language = ""DocId"" Value=""M:System.Printing.IndexedProperties.PrintInt32Property.op_Implicit(System.Printing.IndexedProperties.PrintInt32Property)~System.Int32"" />
       <MemberSignature Language = ""VB.NET"" Value=""Public Shared Widening Operator CType (attribRef As PrintInt32Property) As Integer"" />
       <MemberSignature Language = ""C++ CLI"" Value=""public:&#xA; static operator int(System::Printing::IndexedProperties::PrintInt32Property ^ attribRef);"" />
       <MemberSignature Language = ""F#"" Value=""static member op_Implicit : System.Printing.IndexedProperties.PrintInt32Property -&gt; int"" Usage=""System.Printing.IndexedProperties.PrintInt32Property.op_Implicit attribRef"" />
       <MemberType>Method</MemberType>
       <AssemblyInfo>
        <AssemblyName>System.Printing</AssemblyName>
        <AssemblyVersion>4.0.0.0</AssemblyVersion>
       </AssemblyInfo>
       <ReturnValue>
        <ReturnType>System.Int32</ReturnType>
       </ReturnValue>
       <Parameters>
        <Parameter Name = ""attribRef"" Type=""System.Printing.IndexedProperties.PrintInt32Property"" />
       </Parameters>
       <Docs>
        <param name = ""attribRef"" > To be added.</param>
        <summary>To be added.</summary>
        <returns>To be added.</returns>
        <remarks>To be added.</remarks>
       </Docs>
       </Member>
       <Member MemberName= ""op_Implicit"" >    
          <MemberSignature Language= ""C#"" Value= ""public static implicit operator int (System.Printing.IndexedProperties.PrintInt32Property attribRef);"" FrameworkAlternate= ""netframework-4.7"" />
          <MemberSignature Language= ""ILAsm"" Value= "".method public static hidebysig specialname int32 op_Implicit(class System.Printing.IndexedProperties.PrintInt32Property attribRef) cil managed"" FrameworkAlternate= ""netframework-4.7"" />
          <MemberSignature Language= ""DocId"" Value= ""M:System.Printing.IndexedProperties.PrintInt32Property.op_Implicit(System.Printing.IndexedProperties.PrintInt32Property|System.Runtime.CompilerServices.IsImplicitlyDereferenced)~System.Int32"" FrameworkAlternate= ""netframework-4.7"" />
          <MemberSignature Language= ""VB.NET"" Value= ""Public Shared Widening Operator CType (attribRef As PrintInt32Property) As Integer"" FrameworkAlternate= ""netframework-4.7"" />
          <MemberSignature Language= ""C++ CLI"" Value= ""public:&#xA; static operator int(System::Printing::IndexedProperties::PrintInt32Property ^ attribRef);"" FrameworkAlternate= ""netframework-4.7"" />   
          <MemberSignature Language= ""F#"" Value= ""static member op_Implicit : System.Printing.IndexedProperties.PrintInt32Property -&gt; int"" Usage= ""System.Printing.IndexedProperties.PrintInt32Property.op_Implicit attribRef"" FrameworkAlternate= ""netframework-4.7"" />
          <MemberType > Method </MemberType >   
          <AssemblyInfo >   
            <AssemblyName > System.Printing </AssemblyName >   
            <AssemblyVersion > 4.0.0.0</AssemblyVersion>
          </AssemblyInfo>
             <ReturnValue>
          <ReturnType>System.Int32</ReturnType>
          </ReturnValue>
          <Parameters>
          <Parameter Name= ""attribRef"" Type= ""System.Printing.IndexedProperties.PrintInt32Property"" Index= ""0""  FrameworkAlternate= ""netframework-4.7"" />
          </Parameters>  
          <Docs>
         <param name= ""attribRef"" > To be added.</param>
         <summary>To be added.</summary>
         <returns>To be added.</returns>
         <remarks>To be added.</remarks>
         </Docs>
       </Member> 
      <Member MemberName=""Value"">
      <MemberSignature Language = ""C#"" Value=""public override object Value { get; set; }"" />
      <MemberSignature Language = ""ILAsm"" Value="".property instance object Value"" />
      <MemberSignature Language = ""DocId"" Value=""P:System.Printing.IndexedProperties.PrintInt32Property.Value"" />
      <MemberSignature Language = ""VB.NET"" Value=""Public Overrides Property Value As Object"" />
      <MemberSignature Language = ""C++ CLI"" Value=""public:&#xA; virtual property System::Object ^ Value { System::Object ^ get(); void set(System::Object ^ value); };"" />
      <MemberSignature Language = ""C++ CX"" Value=""public:&#xA; virtual property Platform::Object ^ Value { Platform::Object ^ get(); void set(Platform::Object ^ value); };"" />
      <MemberSignature Language = ""F#"" Value=""member this.Value : obj with get, set"" Usage=""System.Printing.IndexedProperties.PrintInt32Property.Value"" />
      <MemberType>Property</MemberType>
      <AssemblyInfo>
        <AssemblyName>System.Printing</AssemblyName>
        <AssemblyVersion>4.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <Attributes>
        <Attribute>
          <AttributeName>set: System.Security.SecurityCritical</AttributeName>
        </Attribute>
      </Attributes>
      <ReturnValue>
        <ReturnType>System.Object</ReturnType>
      </ReturnValue>
      <Docs>
        <summary>To be added.</summary>
        <value>To be added.</value>
        <remarks>To be added.</remarks>
      </Docs>
     </Member>
    </Members>
   </Type>";
        #endregion

        #region internalEllRemove
        public const string internalEllXml = @"<Type Name=""InternalEIICalss"" FullName=""mdoc.Test2.InternalEIICalss"">
  <TypeSignature Language = ""C#"" Value=""public class InternalEIICalss"" />
  <TypeSignature Language = ""ILAsm"" Value="".class public auto ansi beforefieldinit InternalEIICalss extends System.Object"" />
  <TypeSignature Language = ""DocId"" Value=""T:mdoc.Test2.InternalEIICalss"" />
  <TypeSignature Language = ""VB.NET"" Value=""Public Class InternalEIICalss"" />
  <TypeSignature Language = ""C++ CLI"" Value=""public ref class InternalEIICalss"" />
  <TypeSignature Language = ""F#"" Value=""type InternalEIICalss = class"" />
  <AssemblyInfo>
    <AssemblyName>mdoc.Test</AssemblyName>
    <AssemblyVersion>0.0.0.0</AssemblyVersion>
  </AssemblyInfo>
  <AssemblyInfo>
     <AssemblyName>System.Object</AssemblyName>
  </AssemblyInfo>
  <Base>
    <BaseTypeName>System.Object</BaseTypeName>
  </Base>
  <Interfaces />
  <Docs>
    <summary>To be added.</summary>
    <remarks>To be added.</remarks>
  </Docs>
  <Members>
    <Member MemberName = "".ctor"" >
      <MemberSignature Language=""C#"" Value=""public InternalEIICalss ();"" />
      <MemberSignature Language = ""ILAsm"" Value="".method public hidebysig specialname rtspecialname instance void .ctor() cil managed"" />
      <MemberSignature Language = ""DocId"" Value=""M:mdoc.Test2.InternalEIICalss.#ctor"" />
      <MemberSignature Language = ""VB.NET"" Value=""Public Sub New ()"" />
      <MemberSignature Language = ""C++ CLI"" Value=""public:&#xA; InternalEIICalss();"" />
      <MemberSignature Language = ""C++ CX"" Value=""public:&#xA; InternalEIICalss();"" />
      <MemberType>Constructor</MemberType>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <Parameters />
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
    <Member MemberName = ""Getstring"" >
      <MemberSignature Language=""C#"" Value=""public string Getstring (int a);"" />
      <MemberSignature Language = ""ILAsm"" Value="".method public hidebysig instance string Getstring(int32 a) cil managed"" />
      <MemberSignature Language = ""DocId"" Value=""M:mdoc.Test2.InternalEIICalss.Getstring(System.Int32)"" />
      <MemberSignature Language = ""VB.NET"" Value=""Public Function Getstring (a As Integer) As String"" />
      <MemberSignature Language = ""C++ CLI"" Value=""public:&#xA; System::String ^ Getstring(int a);"" />
      <MemberSignature Language = ""C++ CX"" Value=""public:&#xA; Platform::String ^ Getstring(int a);"" />
      <MemberSignature Language = ""F#"" Value=""member this.Getstring : int -&gt; string"" Usage=""internalEIICalss.Getstring a"" />
      <MemberType>Method</MemberType>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <ReturnValue>
        <ReturnType>System.String</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""a"" Type=""System.Int32"" />
      </Parameters>
      <Docs>
        <param name = ""a"" > To be added.</param>
        <summary>To be added.</summary>
        <returns>To be added.</returns>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
    <Member MemberName=""mdoc.Test.SampleClasses.InterfaceA.Getstring"">
      <MemberSignature Language = ""C#"" Value=""string InterfaceA.Getstring (int a);"" />
      <MemberSignature Language = ""ILAsm"" Value="".method hidebysig newslot virtual instance string mdoc.Test.SampleClasses.InterfaceA.Getstring(int32 a) cil managed"" />
      <MemberSignature Language = ""DocId"" Value=""M:mdoc.Test2.InternalEIICalss.mdoc#Test#SampleClasses#InterfaceA#Getstring(System.Int32)"" />
      <MemberSignature Language = ""VB.NET"" Value=""Function Getstring (a As Integer) As String Implements InterfaceA.Getstring"" />
      <MemberSignature Language = ""C++ CLI"" Value="" virtual System::String ^ mdoc.Test.SampleClasses.InterfaceA.Getstring(int a) = mdoc::Test::SampleClasses::InterfaceA::Getstring;"" />
      <MemberSignature Language = ""C++ CX"" Value="" virtual Platform::String ^ mdoc.Test.SampleClasses.InterfaceA.Getstring(int a) = mdoc::Test::SampleClasses::InterfaceA::Getstring;"" />
      <MemberSignature Language = ""F#"" Value=""abstract member mdoc.Test.SampleClasses.InterfaceA.Getstring : int -&gt; string&#xA;override this.mdoc.Test.SampleClasses.InterfaceA.Getstring : int -&gt; string"" Usage=""internalEIICalss.mdoc.Test.SampleClasses.InterfaceA.Getstring a"" />
      <MemberType>Method</MemberType>
      <AssemblyInfo>
        <AssemblyName>System.Object</AssemblyName>
      </AssemblyInfo>
      <ReturnValue>
        <ReturnType>System.String</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""a"" Type=""System.Int32"" />
      </Parameters>
      <Docs>
        <param name = ""a"" > To be added.</param>
        <summary>To be added.</summary>
        <returns>To be added.</returns>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
  </Members>
</Type>
";

        #endregion

        #region EiiErrorImplement xml
        public const string EiiErrorImplement = @" <Type Name=""EiiImplementClass"" FullName=""mdoc.Test2.EiiImplementClass"">
  <TypeSignature Language = ""C#"" Value=""public class EiiImplementClass : mdoc.Test2.Interface_A, mdoc.Test2.Interface_B"" />
  <TypeSignature Language = ""ILAsm"" Value="".class public auto ansi beforefieldinit EiiImplementClass extends System.Object implements class mdoc.Test2.Interface_A, class mdoc.Test2.Interface_B"" />
  <TypeSignature Language = ""DocId"" Value=""T:mdoc.Test2.EiiImplementClass"" />
  <TypeSignature Language = ""C++ WINRT"" Value=""[Windows::Foundation::Metadata::WebHostHidden]&#xA;class EiiImplementClass : mdoc::Test2::Interface_A, mdoc::Test2::Interface_B"" />
  <TypeSignature Language = ""C++ CLI"" Value=""public ref class EiiImplementClass : mdoc::Test2::Interface_A, mdoc::Test2::Interface_B"" />
  <AssemblyInfo>
    <AssemblyName>mdoc.Test</AssemblyName>
    <AssemblyVersion>0.0.0.0</AssemblyVersion>
  </AssemblyInfo>
  <Base>
    <BaseTypeName>System.Object</BaseTypeName>
  </Base>
  <Interfaces>
    <Interface>
      <InterfaceName>mdoc.Test2.Interface_A</InterfaceName>
    </Interface>
    <Interface>
      <InterfaceName>mdoc.Test2.Interface_B</InterfaceName>
    </Interface>
  </Interfaces>
  <Docs>
    <summary>To be added.</summary>
    <remarks>To be added.</remarks>
  </Docs>
  <Members>
    <Member MemberName = "".ctor"" >
      <MemberSignature Language=""C#"" Value=""public EiiImplementClass ();"" />
      <MemberSignature Language = ""ILAsm"" Value="".method public hidebysig specialname rtspecialname instance void .ctor() cil managed"" />
      <MemberSignature Language = ""DocId"" Value=""M:mdoc.Test2.EiiImplementClass.#ctor"" />
      <MemberSignature Language = ""C++ CX"" Value=""public:&#xA; EiiImplementClass();"" />
      <MemberSignature Language = ""C++ WINRT"" Value="" EiiImplementClass();"" />
      <MemberSignature Language = ""C++ CLI"" Value=""public:&#xA; EiiImplementClass();"" />
      <MemberType>Constructor</MemberType>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <Parameters />
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
    <Member MemberName = ""color"" >
      <MemberSignature Language=""C#"" Value=""public string color { get; set; }"" />
      <MemberSignature Language = ""ILAsm"" Value="".property instance string color"" />
      <MemberSignature Language = ""DocId"" Value=""P:mdoc.Test2.EiiImplementClass.color"" />
      <MemberSignature Language = ""C++ CX"" Value=""public:&#xA; property Platform::String ^ color { Platform::String ^ get(); void set(Platform::String ^ value); };"" />
      <MemberSignature Language = ""C++ WINRT"" Value=""winrt::hstring color();&#xA;&#xA;void color(winrt::hstring value);"" />
      <MemberSignature Language = ""C++ CLI"" Value=""public:&#xA; property System::String ^ color { System::String ^ get(); void set(System::String ^ value); };"" />
      <MemberType>Property</MemberType>
      <Implements>
        <InterfaceMember>P:mdoc.Test2.Interface_A.color</InterfaceMember>
        <InterfaceMember>P:mdoc.Test2.Interface_B.color</InterfaceMember>
      </Implements>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <ReturnValue>
        <ReturnType>System.String</ReturnType>
      </ReturnValue>
      <Docs>
        <summary>To be added.</summary>
        <value>To be added.</value>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
    <Member MemberName = ""GetNo"" >
      <MemberSignature Language=""C#"" Value=""public string GetNo ();"" />
      <MemberSignature Language = ""ILAsm"" Value="".method public hidebysig newslot virtual instance string GetNo() cil managed"" />
      <MemberSignature Language = ""DocId"" Value=""M:mdoc.Test2.EiiImplementClass.GetNo"" />
      <MemberSignature Language = ""C++ CX"" Value=""public:&#xA; Platform::String ^ GetNo();"" />
      <MemberSignature Language = ""C++ WINRT"" Value=""winrt::hstring GetNo();"" />
      <MemberSignature Language = ""C++ CLI"" Value=""public:&#xA; virtual System::String ^ GetNo();"" />
      <MemberType>Method</MemberType>
      <Implements>
        <InterfaceMember>M:mdoc.Test2.Interface_A.GetNo</InterfaceMember>
        <InterfaceMember>M:mdoc.Test2.Interface_B.GetNo</InterfaceMember>
      </Implements>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <ReturnValue>
        <ReturnType>System.String</ReturnType>
      </ReturnValue>
      <Parameters />
      <Docs>
        <summary>To be added.</summary>
        <returns>To be added.</returns>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
    <Member MemberName = ""GetNum"" >
      <MemberSignature Language=""C#"" Value=""public int GetNum ();"" />
      <MemberSignature Language = ""ILAsm"" Value="".method public hidebysig newslot virtual instance int32 GetNum() cil managed"" />
      <MemberSignature Language = ""DocId"" Value=""M:mdoc.Test2.EiiImplementClass.GetNum"" />
      <MemberSignature Language = ""C++ CX"" Value=""public:&#xA; int GetNum();"" />
      <MemberSignature Language = ""C++ WINRT"" Value=""int GetNum();"" />
      <MemberSignature Language = ""C++ CLI"" Value=""public:&#xA; virtual int GetNum();"" />
      <MemberType>Method</MemberType>
      <Implements>
        <InterfaceMember>M:mdoc.Test2.Interface_A.GetNum</InterfaceMember>
      </Implements>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <ReturnValue>
        <ReturnType>System.Int32</ReturnType>
      </ReturnValue>
      <Parameters />
      <Docs>
        <summary>To be added.</summary>
        <returns>To be added.</returns>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
    <Member MemberName = ""IsNum"" >
      <MemberSignature Language=""C#"" Value=""public bool IsNum (string no);"" />
      <MemberSignature Language = ""ILAsm"" Value="".method public hidebysig instance bool IsNum(string no) cil managed"" />
      <MemberSignature Language = ""DocId"" Value=""M:mdoc.Test2.EiiImplementClass.IsNum(System.String)"" />
      <MemberSignature Language = ""C++ CX"" Value=""public:&#xA; bool IsNum(Platform::String ^ no);"" />
      <MemberSignature Language = ""C++ WINRT"" Value=""bool IsNum(winrt::hstring const &amp; no);"" />
      <MemberSignature Language = ""C++ CLI"" Value=""public:&#xA; bool IsNum(System::String ^ no);"" />
      <MemberType>Method</MemberType>
      <Implements>
        <InterfaceMember>M:mdoc.Test2.Interface_B.IsNum(System.String)</InterfaceMember>
      </Implements>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <ReturnValue>
        <ReturnType>System.Boolean</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""no"" Type=""System.String"" />
      </Parameters>
      <Docs>
        <param name = ""no"" > To be added.</param>
        <summary>To be added.</summary>
        <returns>To be added.</returns>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
    <Member MemberName = ""ItemChanged"" >  
      <MemberSignature Language= ""C#"" Value= ""public event EventHandler&lt;EventArgs&gt; ItemChanged;"" />
      <MemberSignature Language= ""ILAsm"" Value= "".event class System.EventHandler`1&lt;class System.EventArgs&gt; ItemChanged"" />  
      <MemberSignature Language= ""DocId"" Value= ""E:mdoc.Test2.EiiImplementClass.ItemChanged"" />
      <MemberSignature Language= ""C++ WINRT"" Value= ""// Register&#xA;event_token ItemChanged(EventHandler&lt;EventArgs&gt; const&amp; handler) const;&#xA;&#xA;// Revoke with event_token&#xA;void ItemChanged(event_token const* cookie) const;&#xA;&#xA;// Revoke with event_revoker&#xA;ItemChanged_revoker ItemChanged(auto_revoke_t, EventHandler&lt;EventArgs&gt; const&amp; handler) const;"" />
      <MemberSignature Language= ""C++ CLI"" Value= ""public:&#xA; virtual event EventHandler&lt;EventArgs ^&gt; ^ ItemChanged;"" />
      <MemberType > Event </MemberType >   
      <Implements>   
         <InterfaceMember>E:mdoc.Test2.Interface_A.ItemChanged</InterfaceMember>
         <InterfaceMember>E:mdoc.Test2.Interface_B.ItemChanged</InterfaceMember>
      </Implements>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <ReturnValue>
        <ReturnType>System.EventHandler&lt; System.EventArgs&gt;</ReturnType>
      </ReturnValue>
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
    <Member MemberName = ""mdoc.Test2.Interface_A.GetNo"" >
      <MemberSignature Language=""C#"" Value=""string Interface_A.GetNo ();"" />
      <MemberSignature Language = ""ILAsm"" Value="".method hidebysig newslot virtual instance string mdoc.Test2.Interface_A.GetNo() cil managed"" />
      <MemberSignature Language = ""DocId"" Value=""M:mdoc.Test2.EiiImplementClass.mdoc#Test2#Interface_A#GetNo"" />
      <MemberSignature Language = ""C++ CX"" Value="" virtual Platform::String ^ mdoc.Test2.Interface_A.GetNo() = mdoc::Test2::Interface_A::GetNo;"" />
      <MemberSignature Language = ""C++ WINRT"" Value=""winrt::hstring mdoc.Test2.Interface_A.GetNo();"" />
      <MemberSignature Language = ""C++ CLI"" Value="" virtual System::String ^ mdoc.Test2.Interface_A.GetNo() = mdoc::Test2::Interface_A::GetNo;"" />
      <MemberType>Method</MemberType>
      <Implements>
        <InterfaceMember>M:mdoc.Test2.Interface_A.GetNo</InterfaceMember>
      </Implements>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <ReturnValue>
        <ReturnType>System.String</ReturnType>
      </ReturnValue>
      <Parameters />
      <Docs>
        <summary>To be added.</summary>
        <returns>To be added.</returns>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
    <Member MemberName = ""mdoc.Test2.Interface_B.IsNum"" >
      <MemberSignature Language=""C#"" Value=""bool Interface_B.IsNum (string no);"" />
      <MemberSignature Language = ""ILAsm"" Value="".method hidebysig newslot virtual instance bool mdoc.Test2.Interface_B.IsNum(string no) cil managed"" />
      <MemberSignature Language = ""DocId"" Value=""M:mdoc.Test2.EiiImplementClass.mdoc#Test2#Interface_B#IsNum(System.String)"" />
      <MemberSignature Language = ""C++ CX"" Value="" virtual bool mdoc.Test2.Interface_B.IsNum(Platform::String ^ no) = mdoc::Test2::Interface_B::IsNum;"" />
      <MemberSignature Language = ""C++ WINRT"" Value=""bool mdoc.Test2.Interface_B.IsNum(winrt::hstring const &amp; no);"" />
      <MemberSignature Language = ""C++ CLI"" Value="" virtual bool mdoc.Test2.Interface_B.IsNum(System::String ^ no) = mdoc::Test2::Interface_B::IsNum;"" />
      <MemberType>Method</MemberType>
      <Implements>
        <InterfaceMember>M:mdoc.Test2.Interface_B.IsNum(System.String)</InterfaceMember>
      </Implements>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <ReturnValue>
        <ReturnType>System.Boolean</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""no"" Type=""System.String"" />
      </Parameters>
      <Docs>
        <param name = ""no"" > To be added.</param>
        <summary>To be added.</summary>
        <returns>To be added.</returns>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
    <Member MemberName = ""mdoc.Test2.Interface_B.ItemChanged"" >
      <MemberSignature Language= ""C#"" Value= ""event EventHandler&lt;EventArgs&gt; mdoc.Test2.Interface_B.ItemChanged;"" />   
      <MemberSignature Language= ""ILAsm"" Value= "".event class System.EventHandler`1&lt;class System.EventArgs&gt; mdoc.Test2.Interface_B.ItemChanged"" />
      <MemberSignature Language= ""DocId"" Value= ""E:mdoc.Test2.EiiImplementClass.mdoc#Test2#Interface_B#ItemChanged"" />
      <MemberSignature Language= ""C++ WINRT"" Value= ""// Register&#xA;event_token mdoc.Test2.Interface_B.ItemChanged(EventHandler&lt;EventArgs&gt; const&amp; handler) const;&#xA;&#xA;// Revoke with event_token&#xA;void mdoc.Test2.Interface_B.ItemChanged(event_token const* cookie) const;&#xA;&#xA;// Revoke with event_revoker&#xA;mdoc.Test2.Interface_B.ItemChanged_revoker mdoc.Test2.Interface_B.ItemChanged(auto_revoke_t, EventHandler&lt;EventArgs&gt; const&amp; handler) const;"" />
      <MemberType > Event </MemberType >
      <Implements >
        <InterfaceMember>E:mdoc.Test2.Interface_B.ItemChanged</InterfaceMember>
      </Implements>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <ReturnValue>
        <ReturnType>System.EventHandler&lt; System.EventArgs&gt;</ReturnType>
      </ReturnValue>
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
    <Member MemberName = ""mdoc.Test2.Interface_B.no"" >
      <MemberSignature Language=""C#"" Value=""int mdoc.Test2.Interface_B.no { get; set; }"" />
      <MemberSignature Language = ""ILAsm"" Value="".property instance int32 mdoc.Test2.Interface_B.no"" />
      <MemberSignature Language = ""DocId"" Value=""P:mdoc.Test2.EiiImplementClass.mdoc#Test2#Interface_B#no"" />
      <MemberSignature Language = ""C++ CX"" Value=""property int mdoc::Test2::Interface_B::no { int get(); void set(int value); };"" />
      <MemberSignature Language = ""C++ WINRT"" Value=""int mdoc.Test2.Interface_B.no();&#xA;&#xA;void mdoc.Test2.Interface_B.no(int value);"" />
      <MemberSignature Language = ""C++ CLI"" Value=""property int mdoc::Test2::Interface_B::no { int get(); void set(int value); };"" />
      <MemberType>Property</MemberType>
      <Implements>
        <InterfaceMember>P:mdoc.Test2.Interface_B.no</InterfaceMember>
      </Implements>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <ReturnValue>
        <ReturnType>System.Int32</ReturnType>
      </ReturnValue>
      <Docs>
        <summary>To be added.</summary>
        <value>To be added.</value>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
    <Member MemberName = ""no"" >
      <MemberSignature Language=""C#"" Value=""public int no { get; set; }"" />
      <MemberSignature Language = ""ILAsm"" Value="".property instance int32 no"" />
      <MemberSignature Language = ""DocId"" Value=""P:mdoc.Test2.EiiImplementClass.no"" />
      <MemberSignature Language = ""C++ CX"" Value=""public:&#xA; property int no { int get(); void set(int value); };"" />
      <MemberSignature Language = ""C++ WINRT"" Value=""int no();&#xA;&#xA;void no(int value);"" />
      <MemberSignature Language = ""C++ CLI"" Value=""public:&#xA; property int no { int get(); void set(int value); };"" />
      <MemberType>Property</MemberType>
      <Implements>
        <InterfaceMember>P:mdoc.Test2.Interface_A.no</InterfaceMember>
        <InterfaceMember>P:mdoc.Test2.Interface_B.no</InterfaceMember>
      </Implements>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <ReturnValue>
        <ReturnType>System.Int32</ReturnType>
      </ReturnValue>
      <Docs>
        <summary>To be added.</summary>
        <value>To be added.</value>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
    <Member MemberName = ""OnItemChanged"" >
      <MemberSignature Language=""C#"" Value=""protected virtual void OnItemChanged (EventArgs e);"" />
      <MemberSignature Language = ""ILAsm"" Value="".method familyhidebysig newslot virtual instance void OnItemChanged(class System.EventArgs e) cil managed"" />
      <MemberSignature Language = ""DocId"" Value=""M:mdoc.Test2.EiiImplementClass.OnItemChanged(System.EventArgs)"" />
      <MemberSignature Language = ""C++ CLI"" Value=""protected:&#xA; virtual void OnItemChanged(EventArgs ^ e);"" />
      <MemberType>Method</MemberType>
      <AssemblyInfo>
        <AssemblyVersion>0.0.0.0</AssemblyVersion>
      </AssemblyInfo>
      <ReturnValue>
        <ReturnType>System.Void</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""e"" Type=""System.EventArgs"" />
      </Parameters>
      <Docs>
        <param name = ""e"" > To be added.</param>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
  </Members>
</Type>
";
        #endregion
    }
}
