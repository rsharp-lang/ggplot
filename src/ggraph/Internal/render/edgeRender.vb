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
        Public Property width As DoubleRange = {2, 5}

        Friend Function getWeightScale(graph As NetworkGraph) As Func(Of Edge, Single)
            Dim scale As DoubleRange = graph.graphEdges.Select(Function(e) e.weight).Range
            Dim wscale As DoubleRange = width
            Dim linkWidth As Func(Of Edge, Single) =
                Function(e)
                    Dim w As Single = scale.ScaleMapping(e.weight, wscale)

                    If w <= 0 OrElse w.IsNaNImaginary Then
                        Return width.Min
                    Else
                        Return w
                    End If
                End Function

            Return linkWidth
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim graph As NetworkGraph = stream.ggplot.data
            Dim linkWidth As Func(Of Edge, Single) = getWeightScale(graph)
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