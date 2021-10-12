Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime
Imports any = Microsoft.VisualBasic.Scripting
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model

Public MustInherit Class ggplotColorMap

    Public Property colorMap As Object

    Public MustOverride Function ColorHandler(ggplot As ggplot, factors As Array) As Func(Of Object, String)

    Public Shared Function CreateColorMap(map As Object, env As Environment) As ggplotColorMap
        If TypeOf map Is String Then
            Return stringMap(DirectCast(map, String))
        ElseIf map.GetType.IsArray Then
            Dim strArray As String() = REnv.asVector(Of String)(map)

            If strArray.Length = 1 Then
                Return stringMap(strArray(Scan0))
            Else
                Return directMap(strArray)
            End If
        ElseIf TypeOf map Is list Then
            Return New ggplotColorFactorMap With {
                .colorMap = DirectCast(map, list).AsGeneric(Of String)(env)
            }
        Else
            Throw New NotImplementedException(map.GetType.FullName)
        End If
    End Function

    Private Shared Function directMap(maps As String()) As ggplotColorMap
        Throw New NotImplementedException
    End Function

    Private Shared Function stringMap(map As String) As ggplotColorMap
        Dim isColor As Boolean = False

        Call map.TranslateColor(throwEx:=False, success:=isColor)

        If isColor Then
            Return New ggplotColorLiteral With {.colorMap = map}
        ElseIf TypeOf map Is String Then
            Return New ggplotColorPalette With {.colorMap = map}
        Else
            Return New ggplotColorFactorMap With {.colorMap = map}
        End If
    End Function
End Class

Public Class ggplotColorLiteral : Inherits ggplotColorMap

    Public Function ToColor() As Color
        Return DirectCast(colorMap, String).TranslateColor
    End Function

    Public Overrides Function ColorHandler(ggplot As ggplot, factors As Array) As Func(Of Object, String)
        Throw New NotImplementedException()
    End Function
End Class

Public Class ggplotColorPalette : Inherits ggplotColorMap

    Public Overrides Function ColorHandler(ggplot As ggplot, factors As Array) As Func(Of Object, String)
        If factors.GetType.GetRTypeCode.IsNumeric Then
            ' level mapping
            Dim colors As String() = Designer _
                .GetColors(any.ToString(colorMap), n:=100) _
                .Select(Function(c) c.ToHtmlColor) _
                .ToArray
            Dim valueRange As New DoubleRange(data:=DirectCast(REnv.asVector(Of Double)(factors), Double()))
            Dim indexRange As New DoubleRange({0, 100})

            Return Function(any)
                       Dim dbl As Double = CDbl(any)
                       Dim i As Integer = CInt(valueRange.ScaleMapping(dbl, indexRange))

                       Return colors(i)
                   End Function
        Else
            ' factor mapping
            Dim factorList As Index(Of String) = factors _
                .AsObjectEnumerator _
                .Select(AddressOf any.ToString) _
                .Distinct _
                .Indexing
            Dim colors As String() = Designer _
                .GetColors(any.ToString(colorMap), n:=factorList.Count) _
                .Select(Function(c) c.ToHtmlColor) _
                .ToArray

            Return Function(any)
                       Dim factor As String = any.ToString(any)
                       Dim i As Integer = factorList.IndexOf(factor)

                       Return colors(i)
                   End Function
        End If
    End Function
End Class

''' <summary>
''' used the field value as color factor
''' </summary>
Public Class ggplotColorFactorMap : Inherits ggplotColorMap

    Public Iterator Function GetLegends(shape As LegendStyles, cssfont As String) As IEnumerable(Of LegendObject)
        For Each [class] In DirectCast(colorMap, Dictionary(Of String, String))
            Yield New LegendObject With {
                .color = [class].Value,
                .style = shape,
                .title = [class].Key,
                .fontstyle = cssfont
            }
        Next
    End Function

    Public Overrides Function ToString() As String
        Return DirectCast(colorMap, Dictionary(Of String, String)).GetJson
    End Function

    Public Overrides Function ColorHandler(ggplot As ggplot, factors As Array) As Func(Of Object, String)
        Dim colorMap As Dictionary(Of String, String) = Me.colorMap
        Return Function(keyObj) colorMap.TryGetValue(any.ToString(keyObj), [default]:="black")
    End Function
End Class