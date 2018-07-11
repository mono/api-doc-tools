Namespace CustomNamespace
    Public NotInheritable Class ClassEnumerator
        Implements CustomInterface

        Public Shared Sub Main(ByVal cmdArgs() As String)

        End Sub
        Private Property CustomProp1 As Integer Implements CustomInterface.Prop1
            Get
                Return 0
            End Get
            Set(value As Integer)

            End Set
        End Property
        Private ReadOnly Property CustomProp2 As Object Implements CustomInterface.Prop2
            Get
                Return 0
            End Get
        End Property
    End Class

    Public Interface CustomInterface
        Property Prop1 As Int32
        ReadOnly Property Prop2 As Object

    End Interface
End Namespace
