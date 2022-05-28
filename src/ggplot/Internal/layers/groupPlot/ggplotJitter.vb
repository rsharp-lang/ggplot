Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Imaging

Namespace layers

    Public Class ggplotJitter : Inherits ggplotGroup

        Public Property radius As Double = 10

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, xscale As d3js.scale.OrdinalScale) As IggplotLegendElement
            Dim x As Double()
            Dim y As Double()
            Dim g As IGraphics = stream.g
            Dim binWidth As Double = DirectCast(stream.scale.X, d3js.scale.OrdinalScale).binWidth
            Dim color As Color

            For Each group As NamedCollection(Of Double) In getDataGroups(stream)
                y = group.Select(AddressOf stream.scale.TranslateY).ToArray
                x = xscale(group.name) _
                    .Replicate(y.Length) _
                    .ToArray
                x = Scatter.Jitter(x, width_jit:=groupWidth * binWidth)
                color = Color.Black.Alpha(alpha * 255)

                For i As Integer = 0 To x.Length - 1
                    Call g.DrawCircle(New PointF(x(i), y(i)), radius, New SolidBrush(color))
                Next
            Next

            Return Nothing
        End Function
    End Class
End Namespace