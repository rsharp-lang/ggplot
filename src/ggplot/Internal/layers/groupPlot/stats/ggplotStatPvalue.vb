Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports Microsoft.VisualBasic.Math.Statistics.Hypothesis.ANOVA
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports SMRUCC.Rsharp.Runtime.Internal.Invokes

Namespace layers

    Public Class ggplotStatPvalue : Inherits ggplotGroup

        Public Property method As String = "anova"

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As OrdinalScale) As IggplotLegendElement
            Select Case method.ToLower
                Case "anova" : Call plotAnova(stream, x)
                Case Else
                    Throw New NotImplementedException(method)
            End Select

            Return Nothing
        End Function

        Private Sub plotAnova(stream As ggplotPipeline, x As OrdinalScale)
            Dim data = getDataGroups(stream).ToArray
            Dim observations As Double()() = data _
                .Select(Function(v) v.value) _
                .ToArray
            Dim anova As New AnovaTest()

            anova.populate(observations, type:=AnovaTest.P_FIVE_PERCENT)
            anova.findWithinGroupMeans()
            anova.setSumOfSquaresOfGroups()
            anova.setTotalSumOfSquares()
            anova.divide_by_degrees_of_freedom()

            Call base.cat("\n", env:=stream.ggplot.environment)
            Call base.cat(anova.ToString, env:=stream.ggplot.environment)

            Dim pvalue As Double = anova.singlePvalue
            Dim tagStr As String = $"ANOVA, p-value={pvalue.ToString("G3")}"
            Dim font As Font = CSSFont.TryParse(stream.theme.tagCSS).GDIObject(stream.g.Dpi)
            Dim pos As New PointF(stream.scale.X.rangeMin + 10, stream.canvas.Padding.Top)

            Call stream.g.DrawString(tagStr, font, Brushes.Black, pos)
        End Sub
    End Class
End Namespace