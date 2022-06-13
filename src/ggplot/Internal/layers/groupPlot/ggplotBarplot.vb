Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports stdNum = System.Math

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
            Dim colors As Func(Of Object, String) = getColors(stream, allGroupData.Select(Function(i) i.name))
            Dim y As DataScaler = stream.scale
            Dim bottom As Double = stream.canvas.PlotRegion.Bottom
            Dim top As Double = stream.canvas.PlotRegion.Top
            Dim line As Pen = Stroke.TryParse(stream.theme.gridStrokeX).GDIObject

            For Each group As NamedCollection(Of Double) In allGroupData
                Dim x As Double = xscale(group.name)
                Dim mean As Double = group.Average
                Dim yi As Double = y.TranslateY(mean)
                Dim bar As New RectangleF(x - boxWidth / 2, yi, boxWidth, bottom - yi)
                Dim color As Color = colors(group.name).TranslateColor.Alpha(255 * alpha)
                Dim paint As New SolidBrush(color)
                Dim std As Double = stdNum.Abs(group.SD)

                Call g.DrawLine(line, New PointF(x, top), New PointF(x, bottom))
                Call g.FillRectangle(paint, bar)
                Call g.DrawRectangle(lineStroke, bar)

                Dim y1 As Double = y.TranslateY(mean + std)
                Dim y2 As Double = y.TranslateY(mean - std)
                Dim widthInner As Double = boxWidth / 3
                Dim line2 As New Pen(color, line.Width * 2)

                Call g.DrawLine(line2, New PointF(x - widthInner, y1), New PointF(x + widthInner, y1))
                Call g.DrawLine(line2, New PointF(x - widthInner, y2), New PointF(x + widthInner, y2))
                Call g.DrawLine(line2, New PointF(x, y1), New PointF(x, yi))
                Call g.DrawLine(line2, New PointF(x, y2), New PointF(x, yi))
            Next

            Return Nothing
        End Function
    End Class
End Namespace