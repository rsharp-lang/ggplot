Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports SMRUCC.Rsharp.Runtime.Components

Public Class ggplotData : Inherits ggplotAdapter

    Public Property x As Array
    Public Property y As Array
    Public Property z As Array

    Public ReadOnly Property nsize As Integer
        Get
            If x Is Nothing Then
                Return 0
            Else
                Return x.Length
            End If
        End Get
    End Property

    Public ReadOnly Property xscale As scalers
        Get
            Dim type As Type = x.GetType.GetElementType

            If type Is GetType(String) Then
                Return scalers.ordinal
            Else
                Return scalers.linear
            End If
        End Get
    End Property

    Public Shared Widening Operator CType(ex As Message) As ggplotData
        Return New ggplotData With {.[error] = ex}
    End Operator

End Class

Public Class ggplotAdapter

    Public Property [error] As Message

End Class