Imports System.Drawing
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Styling
Imports Microsoft.VisualBasic.Data.visualize.Network.Styling.CSS
Imports Microsoft.VisualBasic.Data.visualize.Network.Styling.FillBrushes
Imports Microsoft.VisualBasic.Data.visualize.Network.Styling.Numeric
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.MIME.Html.CSS

Namespace ggraph.render

    Public Class nodeRender : Inherits ggplotLayer

        Public Property defaultColor As Color = Color.SteelBlue
        Public Property fill As IGetBrush
        Public Property radius As IGetSize

        Private Function getFontSize(node As Node) As Single
            Return 8
        End Function

        Private Function getRadius(graph As NetworkGraph) As Func(Of Node, Single)
            If radius Is Nothing Then
                Return Function(any) 45.0!
            Else
                Dim map As Dictionary(Of String, Single) = radius _
                    .GetSize(graph.vertex) _
                    .ToDictionary(Function(n) n.Key.label,
                                  Function(n)
                                      Return n.Maps
                                  End Function)

                Return Function(n) map.TryGetValue(n.label, [default]:=45.0!)
            End If
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim graph As NetworkGraph = stream.ggplot.data
            Dim stroke As Pen = Pens.White
            Dim baseFont As Font = CSSFont.TryParse(stream.theme.tagCSS).GDIObject(stream.g.Dpi)
            Dim renderNode As New NodeRendering(
                radiusValue:=getRadius(graph),
                fontSizeValue:=AddressOf getFontSize,
                defaultColor:=defaultColor,
                stroke:=stroke,
                baseFont:=baseFont,
                scalePos:=stream.layout,
                throwEx:=False,
                getDisplayLabel:=Function(n) n.data.label,
                drawNodeShape:=Nothing,
                getLabelPosition:=Nothing,
                labelWordWrapWidth:=-1,
                nodeWidget:=Nothing
            )

            If Not fill Is Nothing Then
                graph = graph.SetNodeFill(fill)
            End If

            Dim labels = renderNode.RenderingVertexNodes(stream.g, graph.vertex.ToArray).ToArray

            DirectCast(stream, graphPipeline).labels.AddRange(labels)

            Return Nothing
        End Function
    End Class
End Namespace