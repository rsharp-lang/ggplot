
Imports System.Drawing
Imports ggplot.elements.legend
Imports ggplot.ggraph.layout
Imports ggplot.ggraph.render
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Styling
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.Html.CSS

Namespace ggraph

    Public Class graphRender : Inherits ggplot

        Public Sub New(theme As Theme)
            MyBase.New(theme)
        End Sub

        Protected Overrides Sub PlotInternal(ByRef g As IGraphics, canvas As GraphicsRegion)
            Call g.Clear(theme.background.TranslateColor)
            Call plotGraph(g, canvas)
        End Sub

        Private Sub plotGraph(ByRef g As IGraphics, canvas As GraphicsRegion)
            Dim force As ggforce = args.getValue(Of ggforce)(NameOf(ggforce), environment, New force_directed)
            Dim graph As NetworkGraph = DirectCast(data, NetworkGraph)
            Dim layers As New Queue(Of ggplotLayer)(
                collection:=If(UnionGgplotLayers Is Nothing, Me.layers, UnionGgplotLayers(Me.layers))
            )
            Dim nodeLayer As nodeRender = layers _
                .Where(Function(l) TypeOf l Is nodeRender) _
                .FirstOrDefault

            Call force.createLayout(graph, environment)

            If nodeLayer IsNot Nothing AndAlso Not nodeLayer.fill Is Nothing Then
                graph = graph.SetNodeFill(nodeLayer.fill)
            End If

            ' 先進行佈局計算，再
            ' 获取得到当前的这个网络对象相对于图像的中心点的位移值
            Dim scalePos As Dictionary(Of String, PointF) = CanvasScaler.CalculateNodePositions(graph, g.Size, canvas.Padding)
            Dim legends As New List(Of IggplotLegendElement)
            Dim stream As New graphPipeline With {
                .ggplot = Me,
                .canvas = canvas,
                .g = g,
                .scale = Nothing,
                .x = Nothing,
                .y = Nothing,
                .layout = scalePos
            }

            Do While layers.Count > 0
                Call layers _
                    .Dequeue _
                    .Plot(stream) _
                    .DoCall(AddressOf legends.Add)
            Loop

            Dim drawLegends As Boolean = theme.drawLegend

            theme.drawLegend = False

            Call Draw2DElements(g, canvas, legends)

            If drawLegends Then
                Dim nodeStyle As nodeRender = Me.layers.Where(Function(l) TypeOf l Is nodeRender).FirstOrDefault
                Dim edgeStyle As edgeRender = Me.layers.Where(Function(l) TypeOf l Is edgeRender).FirstOrDefault

                Call drawGraphLegends(g, graph, canvas, nodeStyle, edgeStyle, legends)
            End If
        End Sub

        Private Sub drawGraphLegends(g As IGraphics, graph As NetworkGraph,
                                     canvas As GraphicsRegion,
                                     nodeStyle As nodeRender,
                                     edgeStyle As edgeRender,
                                     legends As IEnumerable(Of IggplotLegendElement))

            Dim legendList = (From group As IggplotLegendElement
                              In legends.SafeQuery
                              Where Not group Is Nothing).ToArray
            Dim x As Double
            Dim y As Double
            Dim line As Pen = Stroke.TryParse(theme.axisStroke)
            Dim labelFont As Font = CSSFont.TryParse(theme.legendLabelCSS).GDIObject(g.Dpi)

            If Not nodeStyle Is Nothing Then
                ' draw node radius
                Dim radius = nodeStyle.getRadius(graph)
                Dim radiusRange = graph.vertex.Select(Function(v) CDbl(radius(v))).Range
                Dim degree = graph.vertex.Select(Function(v) CDbl(v.degree.In + v.degree.Out)).Range

                x = canvas.PlotRegion.Right + radiusRange.Max * 1.125
                y = canvas.PlotRegion.Top + radiusRange.Min * 1.125

                Dim ytop As Double = y
                Dim ybottom As Double
                Dim r As Double

                For Each d As Double In degree.Enumerate(4)
                    r = degree.ScaleMapping(d, radiusRange)
                    y += r + 30
                    ybottom = y
                    g.DrawCircle(New PointF(x, y), r, Brushes.Black)
                Next

                ' draw radius axis
                Call g.DrawLine(line, New PointF(x + r, ytop), New PointF(x + r, ybottom))
                Call g.DrawString(degree.Min.ToString("F0"), labelFont, Brushes.Black, New PointF(x + r, ytop))
                Call g.DrawString(degree.Max.ToString("F0"), labelFont, Brushes.Black, New PointF(x + r, ybottom))

                ' draw node shape type
                y += r * 2
                r *= 0.85

                Dim shapes = nodeStyle.getShapes(graph)
                Dim nodeShapes = graph.vertex _
                    .Select(Function(v) (v.data("shape"), shapes(v))) _
                    .Where(Function(d) Not d.Item1.StringEmpty) _
                    .GroupBy(Function(d) d.Item2) _
                    .Select(Function(l)
                                Return New LegendObject With {
                                    .color = "black",
                                    .fontstyle = theme.legendLabelCSS,
                                    .style = l.Key,
                                    .title = l.First.Item1
                                }
                            End Function) _
                    .ToArray

                For Each shape In nodeShapes
                    Call Legend.DrawLegend(g, New PointF(x, y), New SizeF(r, r), shape)
                    y += r * 2
                Next
            End If

            If Not edgeStyle Is Nothing Then
                ' draw edge width
            End If

            If Not legendList.IsNullOrEmpty Then
                ' draw other legends
                Call DrawLegends(legendList, g, canvas, New PointF(x, y))
            End If
        End Sub
    End Class
End Namespace