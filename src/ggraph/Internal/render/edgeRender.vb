Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Imaging

Namespace ggraph.render

    Public Class edgeRender : Inherits ggplotLayer

        Public Property color As String

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim graph As NetworkGraph = stream.ggplot.data
            Dim scale As DoubleRange = graph.graphEdges.Select(Function(e) e.weight).Range
            Dim wscale As DoubleRange = {1, 3}
            Dim linkWidth As Func(Of Edge, Single) = Function(e) scale.ScaleMapping(e.weight, wscale)
            Dim edgeDashType As New Dictionary(Of String, DashStyle)
            Dim edgeColor As Color = Me.color.TranslateColor
            Dim engine As New EdgeRendering(
                linkWidth:=linkWidth,
                edgeDashTypes:=edgeDashType,
                scalePos:=stream.layout,
                throwEx:=False,
                edgeShadowDistance:=0,
                defaultEdgeColor:=edgeColor,
                drawEdgeBends:=False,
                drawEdgeDirection:=False
            )
            Dim labels = engine.drawEdges(stream.g, graph).ToArray

            DirectCast(stream, graphPipeline).labels.AddRange(labels)

            Return Nothing
        End Function
    End Class
End Namespace