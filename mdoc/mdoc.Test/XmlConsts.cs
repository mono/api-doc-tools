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
    }
}
