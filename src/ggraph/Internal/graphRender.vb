#Region "Microsoft.VisualBasic::422a1249ce0a83601940ebdcafb01ea6, src\ggraph\Internal\graphRender.vb"

    ' Author:
    ' 
    '       xieguigang (I@xieguigang.me)
    ' 
    ' Copyright (c) 2021 R# language
    ' 
    ' 
    ' MIT License
    ' 
    ' 
    ' Permission is hereby granted, free of charge, to any person obtaining a copy
    ' of this software and associated documentation files (the "Software"), to deal
    ' in the Software without restriction, including without limitation the rights
    ' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    ' copies of the Software, and to permit persons to whom the Software is
    ' furnished to do so, subject to the following conditions:
    ' 
    ' The above copyright notice and this permission notice shall be included in all
    ' copies or substantial portions of the Software.
    ' 
    ' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    ' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    ' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    ' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    ' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    ' SOFTWARE.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 218
    '    Code Lines: 170 (77.98%)
    ' Comment Lines: 9 (4.13%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 39 (17.89%)
    '     File Size: 9.54 KB


    '     Class graphRender
    ' 
    '         Properties: edgeWeightTitle
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Sub: drawGraphLegends, plotGraph, PlotInternal
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports ggplot.ggraph.layout
Imports ggplot.ggraph.render
Imports ggplot.layers
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
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
Imports Microsoft.VisualBasic.MIME.Html.Render

Namespace ggraph

    Public Class graphRender : Inherits ggplot

        Public ReadOnly Property edgeWeightTitle As String
            Get
                Return args.getValue(Of String)("edge_weight.title", environment, [default]:="Edge Weight")
            End Get
        End Property

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
            Dim css As CSSEnvirnment = g.LoadEnvironment
            Dim line As Pen = css.GetPen(Stroke.TryParse(theme.axisStroke))
            Dim labelFont As Font = css.GetFont(CSSFont.TryParse(theme.legendLabelCSS))
            Dim ytop As Double = y
            Dim ybottom As Double
            Dim A As SizeF = g.MeasureString("A", labelFont)

            If Not nodeStyle Is Nothing Then
                ' draw node radius
                Dim radius = nodeStyle.getRadius(graph)
                Dim radiusRange As DoubleRange = graph.vertex _
                    .Select(Function(v) radius(v)) _
                    .IteratesALL _
                    .Select(Function(s) CDbl(s)) _
                    .Range
                Dim degree = graph.vertex.Select(Function(v) CDbl(v.degree.In + v.degree.Out)).Range
                Dim r As Single = degree.ScaleMapping(degree.Min, radiusRange)
                Dim rmax As Double = degree.ScaleMapping(degree.Max, radiusRange)

                x = canvas.PlotRegion.Right + radiusRange.Max * 1.5
                y = canvas.PlotRegion.Top + radiusRange.Min * 1.125
                ytop = y

                Call g.DrawString("Node Degree", labelFont, Brushes.Black, New PointF(x - rmax / 2, ytop - A.Height * 1.5))

                For Each d As Double In degree.Enumerate(3)
                    r = degree.ScaleMapping(d, radiusRange) * 0.65
                    y += r
                    g.DrawCircle(New PointF(x, y), r, Brushes.Black)
                    ybottom = y + 20
                    y = ybottom
                Next

                ybottom -= 40

                ' draw radius axis
                Call g.DrawLine(line, New PointF(x + r * 1.25, ytop + r / 3), New PointF(x + r * 1.25, ybottom + r / 3))
                Call g.DrawString(degree.Min.ToString("F0"), labelFont, Brushes.Black, New PointF(x + r * 1.5, ytop))
                Call g.DrawString(degree.Max.ToString("F0"), labelFont, Brushes.Black, New PointF(x + r * 1.5, ybottom))

                ' draw node shape type
                y += r * 2
                r *= 0.85
                x = canvas.PlotRegion.Right + radiusRange.Max * 1.125

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

                Call g.DrawString("Shape Types", labelFont, Brushes.Black, New PointF(x, y - r * 1.5))

                Dim pos As PointF

                For Each shape In nodeShapes
                    pos = New PointF(x, y)

                    ' there is a lyaout bug about circle
                    If shape.style = LegendStyles.Circle Then
                        pos = New PointF(x - r / 2, y)
                        shape.title = " " & shape.title
                    End If

                    Try
                        Legend.DrawLegend(g, pos, New SizeF(r, r), shape)
                    Catch ex As Exception
                        Call Console.WriteLine(New RectangleF(pos, New SizeF(r, r)))
                    End Try

                    y += r * 1.5
                Next
            End If

            If Not edgeStyle Is Nothing Then
                ' draw edge width
                Dim lineWidth = edgeStyle.getWeightScale(graph)
                Dim widths = graph.graphEdges.Select(Function(l) CDbl(lineWidth(l))).Range
                Dim weights = graph.graphEdges.Select(Function(l) l.weight).Range
                Dim w As Double
                Dim deltaX As Double = 50

                y += deltaX

                Call g.DrawString(edgeWeightTitle, labelFont, Brushes.Black, New PointF(x, y))

                ytop = y + w + 30

                For Each d As Double In weights.Enumerate(4)
                    w = weights.ScaleMapping(d, widths)
                    y += w + 30
                    ybottom = y
                    g.DrawLine(New Pen(Brushes.Black, w), New PointF(x, y), New PointF(x + deltaX - 10, y))
                Next

                ' draw radius axis
                Call g.DrawLine(line, New PointF(x + deltaX, ytop), New PointF(x + deltaX, ybottom))
                Call g.DrawString(weights.Min.ToString("F3"), labelFont, Brushes.Black, New PointF(x + deltaX * 1.25, ytop))
                Call g.DrawString(weights.Max.ToString("F3"), labelFont, Brushes.Black, New PointF(x + deltaX * 1.25, ybottom - A.Height / 2))

                y += deltaX * 1.5
            End If

            If Not legendList.IsNullOrEmpty Then
                ' draw other legends
                Call DrawLegends(legendList, g, canvas, New PointF(x, y))
            End If
        End Sub
    End Class
End Namespace
