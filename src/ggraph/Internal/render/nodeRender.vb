Imports System.Drawing
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.FileStream.Generic
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Styling
Imports Microsoft.VisualBasic.Data.visualize.Network.Styling.FillBrushes
Imports Microsoft.VisualBasic.Data.visualize.Network.Styling.Numeric
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.MIME.Html.CSS

Namespace ggraph.render

    Public Class nodeRender : Inherits ggplotLayer

        Public Property defaultColor As Color = Color.SteelBlue
        Public Property fill As IGetBrush
        Public Property radius As IGetSize
        Public Property shape As IGetShape

        Private Function getFontSize(node As Node) As Single
            Return 8
        End Function

        Friend Function getRadius(graph As NetworkGraph) As Func(Of Node, Single)
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

        Friend Function getShapes(graph As NetworkGraph) As Func(Of Node, LegendStyles)
            If shape Is Nothing Then
                Return Function(any) LegendStyles.Circle
            Else
                Dim map As Dictionary(Of String, LegendStyles) = shape _
                    .GetShapes(graph.vertex) _
                    .ToDictionary(Function(n) n.Key.label,
                                  Function(n)
                                      Return n.Maps
                                  End Function)

                Return Function(n) map.TryGetValue(n.label, [default]:=LegendStyles.Circle)
            End If
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim graph As NetworkGraph = stream.ggplot.data
            Dim stroke As Pen = Pens.White
            Dim shapeAs = getShapes(stream.ggplot.data)
            Dim baseFont As Font = CSSFont.TryParse(stream.theme.tagCSS).GDIObject(stream.g.Dpi)
            Dim drawNodeShape As DrawNodeShape =
                Function(id As String,
                         g As IGraphics,
                         brush As Brush,
                         radius As Single,
                         center As PointF) As RectangleF

                    Dim v As Node = graph.GetElementByID(id)
                    Dim legendStyle As LegendStyles = shapeAs(v)

                    center = New PointF(center.X - radius / 2, center.Y - radius / 2)
                    g.DrawLegendShape(center, New SizeF(radius, radius), legendStyle, brush)
                End Function

            Dim renderNode As New NodeRendering(
                radiusValue:=getRadius(graph),
                fontSizeValue:=AddressOf getFontSize,
                defaultColor:=defaultColor,
                stroke:=stroke,
                baseFont:=baseFont,
                scalePos:=stream.layout,
                throwEx:=False,
                getDisplayLabel:=Function(n) n.data.label,
                drawNodeShape:=drawNodeShape,
                getLabelPosition:=Nothing,
                labelWordWrapWidth:=-1,
                nodeWidget:=Nothing
            )

            Dim vertex As Node() = graph.vertex _
                .OrderBy(Function(a)
                             Return If(a.data(NamesOf.REFLECTION_ID_MAPPING_NODETYPE), "")
                         End Function) _
                .ToArray
            Dim labels = renderNode _
                .RenderingVertexNodes(stream.g, vertex) _
                .ToArray

            DirectCast(stream, graphPipeline).labels.AddRange(labels)

            Return Nothing
        End Function
    End Class
End Namespace