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
          <Parameter Name = ""a"" Type=""System.Int32"" />
          <Parameter Name = ""b"" Type=""System.String"" />
          <Parameter Name = ""c"" Type=""System.Int32"" />
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
