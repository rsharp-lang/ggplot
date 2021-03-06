Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.MIME.Html.CSS

Namespace layers

    Public Class ggplotViolin : Inherits ggplotGroup

        Public Property splineDegree As Single = 2
        Public Property showStats As Boolean = False

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, xscale As d3js.scale.OrdinalScale) As IggplotLegendElement
            Dim g As IGraphics = stream.g
            Dim binWidth As Double = DirectCast(stream.scale.X, d3js.scale.OrdinalScale).binWidth
            Dim yscale As YScaler = stream.scale
            Dim semiWidth As Double = binWidth / 2 * groupWidth
            Dim lineStroke As Pen = Stroke.TryParse(stream.theme.lineStroke).GDIObject
            Dim labelFont As Font = CSSFont.TryParse(stream.theme.tagCSS).GDIObject(g.Dpi)
            Dim allGroupData = getDataGroups(stream).ToArray
            Dim colors As Func(Of Object, String) = getColors(stream, allGroupData.Select(Function(i) i.name))
            Dim gridPen As Pen = Stroke.TryParse(stream.theme.gridStrokeX)
            Dim bottom = stream.canvas.PlotRegion.Bottom
            Dim top = stream.canvas.PlotRegion.Top

            For Each group As NamedCollection(Of Double) In allGroupData
                Dim x As Double = xscale(group.name)
                Dim color As Color = colors(group.name).TranslateColor.Alpha(alpha * 255)

                Call g.DrawLine(gridPen, New PointF(x, top), New PointF(x, bottom))
                Call Violin.PlotViolin(
                    group:=group,
                    x:=x,
                    yscale:=yscale,
                    semiWidth:=semiWidth,
                    splineDegree:=splineDegree,
                    polygonStroke:=lineStroke,
                    showStats:=showStats,
                    labelFont:=labelFont,
                    color:=color,
                    g:=g,
                    canvas:=stream.canvas,
                    theme:=stream.theme
                )
            Next

            Return Nothing
        End Function
    End Class
End Namespace