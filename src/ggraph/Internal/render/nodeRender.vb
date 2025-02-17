#Region "Microsoft.VisualBasic::f4cd83b34f4164297e45a373586d5a5b, src\ggraph\Internal\render\nodeRender.vb"

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

'   Total Lines: 126
'    Code Lines: 108 (85.71%)
' Comment Lines: 0 (0.00%)
'    - Xml Docs: 0.00%
' 
'   Blank Lines: 18 (14.29%)
'     File Size: 5.18 KB


'     Class nodeRender
' 
'         Properties: defaultColor, fill, radius, shape
' 
'         Function: getFontSize, getRadius, getShapes, Plot
' 
' 
' /********************************************************************************/

#End Region

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
Imports Microsoft.VisualBasic.MIME.Html.Render

Namespace ggraph.render

    Public Class nodeRender : Inherits ggplotLayer

        Public Property defaultColor As Color = Color.SteelBlue
        Public Property fill As IGetBrush
        Public Property radius As IGetSize
        Public Property shape As IGetShape

        Private Function getFontSize(node As Node) As Single
            Return 8
        End Function

        Friend Function getRadius(graph As NetworkGraph) As Func(Of Node, Single())
            If radius Is Nothing Then
                Return Function(any) {45.0!}
            Else
                Dim map As Dictionary(Of String, Single) = radius _
                    .GetSize(graph.vertex) _
                    .ToDictionary(Function(n) n.Key.label,
                                  Function(n)
                                      Return n.Maps
                                  End Function)

                Return Function(n)
                           Dim r As Single = map.TryGetValue(n.label, [default]:=45.0!)

                           If r <= 0 OrElse r.IsNaNImaginary Then
                               Return {45}
                           Else
                               Return {r}
                           End If
                       End Function
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

        Private Class Drawer

            Public graph As NetworkGraph
            Public shapeAs As Func(Of Node, LegendStyles)

            Public Function DrawNodeShape(id As String, g As IGraphics, brush As Brush, radius As Single(), center As PointF) As RectangleF
                Dim v As Node = graph.GetElementByID(id)
                Dim legendStyle As LegendStyles = shapeAs(v)
                Dim size As SizeF

                If radius.Length = 1 Then
                    size = New SizeF(radius(0), radius(0))
                Else
                    size = New SizeF(radius(0), radius(1))
                End If

                center = New PointF(center.X - size.Width / 2, center.Y - size.Height / 2)
                g.DrawLegendShape(center, size, legendStyle, brush)

                Return New RectangleF(center, size)
            End Function
        End Class

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim graph As NetworkGraph = stream.ggplot.data
            Dim stroke As Pen = Pens.White
            Dim shapeAs = getShapes(stream.ggplot.data)
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim baseFont As Font = css.GetFont(stream.theme.tagCSS)
            Dim drawer As New Drawer With {.graph = graph, .shapeAs = shapeAs}
            Dim renderNode As New NodeRendering(
                graph,
                radiusValue:=getRadius(graph),
                fontSizeValue:=AddressOf getFontSize,
                defaultColor:=defaultColor,
                stroke:=stroke,
                baseFontCss:=baseFont,
                scalePos:=stream.layout,
                throwEx:=False,
                getDisplayLabel:=Function(n) n.data.label,
                drawNodeShape:=AddressOf drawer.DrawNodeShape,
                getLabelPosition:=Nothing,
                labelWordWrapWidth:=-1,
                nodeWidget:=Nothing,
                drawShape:=Nothing
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
