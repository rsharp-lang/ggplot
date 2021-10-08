Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime

Public Class ggplotReader

    Public Property x As String
    Public Property y As String
    Public Property color As Object
    Public Property title As String

    Public Overrides Function ToString() As String
        Return $"{x} ~ {y}"
    End Function

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

    Public Function getMapColor(data As Object, env As Environment) As String()
        If TypeOf data Is dataframe Then
            Dim v As String() = REnv.asVector(Of String)(DirectCast(data, dataframe).getColumnVector(color))
            Dim uniqV = v.Distinct.Indexing

            If uniqV.Objects.All(Function(vi) Val(vi) > 0 AndAlso vi.IsInteger) Then
                Return v
            ElseIf uniqV.Objects.All(Function(vi)
                                         Dim isColor As Boolean = False
                                         Call vi.TranslateColor(success:=isColor, throwEx:=False)
                                         Return isColor
                                     End Function) Then
                Return v
            Else
                Dim colors = Designer.GetColors("Set1:c8", uniqV.Count).Select(Function(c) c.ToHtmlColor).ToArray

                Return v.Select(Function(vi) colors(uniqV.IndexOf(vi))).ToArray
            End If

        ElseIf TypeOf data Is list Then
            Throw New NotImplementedException
        Else
            Throw New NotImplementedException
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