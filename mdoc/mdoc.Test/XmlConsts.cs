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
    }
}
