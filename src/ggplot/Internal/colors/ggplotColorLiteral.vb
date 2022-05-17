Imports System.Drawing
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports REnv = SMRUCC.Rsharp.Runtime

Namespace colors

    Public Class ggplotColorLiteral : Inherits ggplotColorMap

        Public Overrides Function ToString() As String
            Return ToColor.ToHtmlColor
        End Function

        Public Function ToColor() As Color
            Return DirectCast(colorMap, String) _
                .TranslateColor _
                .Alpha(alpha * 255)
        End Function

        Public Overrides Function ColorHandler(ggplot As ggplot, factors As Array) As Func(Of Object, String)
            Dim literal As String = ToColor.ToHtmlColor
            Return Function(any) literal
        End Function

        Public Overrides Function TryGetFactorLegends(factors As Array, shape As LegendStyles, theme As Theme) As LegendObject()
            Return DirectCast(REnv.asVector(Of String)(factors), String()) _
                .Select(Function(name)
                            Return New LegendObject With {
                                .color = DirectCast(colorMap, String),
                                .fontstyle = theme.legendLabelCSS,
                                .style = shape,
                                .title = name
                            }
                        End Function) _
                .ToArray
        End Function
    End Class
End Namespace