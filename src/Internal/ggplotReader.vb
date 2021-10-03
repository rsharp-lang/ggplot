Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object

Public Class ggplotReader

    Public Property x As String
    Public Property y As String

    Public Function getMapData(data As Object, env As Environment) As ggplotData
        If TypeOf data Is dataframe Then
            Return New ggplotData With {
                .x = DirectCast(data, dataframe).getColumnVector(x),
                .y = DirectCast(data, dataframe).getColumnVector(y)
            }
        ElseIf TypeOf data Is list Then
            Return Internal.debug.stop(New NotImplementedException, env)
        Else
            Return Internal.debug.stop(New NotImplementedException, env)
        End If
    End Function

End Class

Public Class ggplotData

    Public Property x As Array
    Public Property y As Array
    Public Property [error] As Message

    Public Shared Widening Operator CType(ex As Message) As ggplotData
        Return New ggplotData With {.[error] = ex}
    End Operator

End Class