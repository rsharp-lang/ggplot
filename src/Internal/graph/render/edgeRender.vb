Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Namespace ggraph.render

    Public Class edgeRender : Inherits ggplotLayer

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim graph As NetworkGraph = stream.ggplot.data
            Dim linkWidth As Func(Of Edge, Single) = Function() 5
            Dim edgeDashType As New Dictionary(Of String, DashStyle)
            Dim labels = NetworkVisualizer.drawEdges(
                g:=stream.g,
                net:=graph,
                linkWidth:=linkWidth,
                edgeDashTypes:=edgeDashType,
                scalePos:=stream.layout,
                throwEx:=False,
                edgeShadowDistance:=0,
                defaultEdgeColor:=Color.LightGray,
                drawEdgeBends:=False,
                drawEdgeDirection:=False
            ).ToArray

            Return Nothing
        End Function
    End Class
End Namespace