Imports System.Drawing
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.MIME.Html.CSS

Namespace ggraph.render

    Public Class nodeRender : Inherits ggplotLayer

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim graph As NetworkGraph = stream.ggplot.data
            Dim radius As Func(Of Node, Single) = Function() 20
            Dim fontsize As Func(Of Node, Single) = Function() 10
            Dim defaultColor As Color = Color.Blue
            Dim stroke As Pen = Pens.White
            Dim baseFont As Font = CSSFont.TryParse(CSSFont.Win7Normal).GDIObject(stream.g.Dpi)
            Dim nodelabel As Func(Of Node, String) = Function(n) n.data.label
            Dim renderNode As New NodeRendering(
                radiusValue:=radius,
                fontSizeValue:=fontsize,
                defaultColor:=defaultColor,
                stroke:=stroke,
                baseFont:=baseFont,
                scalePos:=stream.layout,
                throwEx:=False,
                getDisplayLabel:=nodelabel,
                drawNodeShape:=Nothing,
                getLabelPosition:=Nothing,
                labelWordWrapWidth:=-1,
                nodeWidget:=Nothing
            )

            Dim labels = renderNode.drawVertexNodes(stream.g, graph.vertex.ToArray).ToArray

            DirectCast(stream, graphPipeline).labels.AddRange(labels)

            Return Nothing
        End Function
    End Class
End Namespace