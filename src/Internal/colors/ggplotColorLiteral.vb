Imports System.Drawing
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging

Namespace colors

    Public Class ggplotColorLiteral : Inherits ggplotColorMap

        Public Overrides Function ToString() As String
            Return ToColor.ToHtmlColor
        End Function

        Public Function ToColor() As Color
            Return DirectCast(colorMap, String).TranslateColor
        End Function

        Public Overrides Function ColorHandler(ggplot As ggplot, factors As Array) As Func(Of Object, String)
            Throw New NotImplementedException()
        End Function

        Public Overrides Function TryGetFactorLegends(factors As Array, shape As LegendStyles, theme As Theme) As LegendObject()
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace