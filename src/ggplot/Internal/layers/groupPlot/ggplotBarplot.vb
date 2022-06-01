Imports System.Drawing
Imports ggplot.colors
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ComponentModel.DataStructures
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.MIME.Html.CSS

Namespace layers

    Public Class ggplotBarplot : Inherits ggplotGroup

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, xscale As d3js.scale.OrdinalScale) As IggplotLegendElement
            Dim g As IGraphics = stream.g
            Dim binWidth As Double = DirectCast(stream.scale.X, d3js.scale.OrdinalScale).binWidth
            Dim yscale As YScaler = stream.scale
            Dim boxWidth As Double = binWidth * groupWidth
            Dim lineStroke As Pen = Stroke.TryParse(stream.theme.lineStroke).GDIObject
            Dim labelFont As Font = CSSFont.TryParse(stream.theme.tagCSS).GDIObject(g.Dpi)
            Dim allGroupData = getDataGroups(stream).ToArray
            Dim colors = colorMap.ColorHandler(stream.ggplot, allGroupData.Select(Function(i) i.name).ToArray)
            Dim y As DataScaler = stream.scale
            Dim bottom As Double = stream.canvas.PlotRegion.Bottom

            For Each group As NamedCollection(Of Double) In allGroupData
                Dim x As Double = xscale(group.name)
                Dim mean As Double = group.Average
                Dim yi As Double = y.TranslateY(mean)
                Dim bar As New RectangleF(x - boxWidth / 2, yi, boxWidth, bottom - yi)
                Dim color As String = colors(group.name)

                Call g.FillRectangle(color.GetBrush, bar)
                Call g.DrawRectangle(lineStroke, bar)
            Next

            Return Nothing
        End Function
    End Class
End Namespace