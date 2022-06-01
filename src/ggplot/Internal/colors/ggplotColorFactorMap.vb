Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization.JSON
Imports any = Microsoft.VisualBasic.Scripting

Namespace colors

    ''' <summary>
    ''' used the field value as color factor
    ''' </summary>
    Public Class ggplotColorFactorMap : Inherits ggplotColorMap

        Private Iterator Function GetLegends(shape As LegendStyles, cssfont As String) As IEnumerable(Of LegendObject)
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
            Dim alphaColors As New Dictionary(Of String, String)

            For Each factor In colorMap
                alphaColors(factor.Key) = factor _
                    .Value _
                    .TranslateColor _
                    .Alpha(alpha * 255) _
                    .ARGBExpression
            Next

            Return Function(keyObj) alphaColors.TryGetValue(any.ToString(keyObj), [default]:="black")
        End Function

        Public Overrides Function TryGetFactorLegends(factors As Array, shape As LegendStyles, theme As Theme) As LegendObject()
            Return GetLegends(shape, theme.legendLabelCSS).ToArray
        End Function
    End Class
End Namespace