
Imports System.Drawing
Imports ggplot.elements.legend
Imports ggplot.ggraph.layout
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Linq

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

            Call force.createLayout(graph)

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

            Call Draw2DElements(g, canvas, legends)
        End Sub
    End Class
End Namespace