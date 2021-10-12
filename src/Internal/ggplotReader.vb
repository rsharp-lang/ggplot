Imports Microsoft.VisualBasic.ComponentModel.Collection
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
    Public Property label As String
    Public Property args As list

    Public Overrides Function ToString() As String
        Return $"{x} ~ {y}"
    End Function

    Public Function getMapData(data As Object, env As Environment) As ggplotData
        Return New ggplotData With {
            .x = unifySource(data, x, env),
            .y = unifySource(data, y, env)
        }
    End Function

    Private Shared Function unifySource(data As Object, source As String, env As Environment) As Array
        If TypeOf data Is dataframe Then
            Return DirectCast(data, dataframe).getColumnVector(source)
        ElseIf TypeOf data Is list Then
            Throw New NotImplementedException
        Else
            Throw New NotImplementedException
        End If
    End Function

    Public Function getMapColor(data As Object, env As Environment) As String()
        Dim v As String() = REnv.asVector(Of String)(unifySource(data, color, env))
        Dim uniqV As Index(Of String) = v.Distinct.Indexing

        If uniqV.Objects.All(Function(vi) Val(vi) > 0 AndAlso vi.IsInteger) Then
            Return v
        ElseIf uniqV.Objects _
            .All(Function(vi)
                     Dim isColor As Boolean = False
                     Call vi.TranslateColor(success:=isColor, throwEx:=False)
                     Return isColor
                 End Function) Then

            Return v
        Else
            Dim colors = Designer _
                .GetColors("Set1:c8", uniqV.Count) _
                .Select(Function(c) c.ToHtmlColor) _
                .ToArray

            Return v _
                .Select(Function(vi) colors(uniqV.IndexOf(vi))) _
                .ToArray
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