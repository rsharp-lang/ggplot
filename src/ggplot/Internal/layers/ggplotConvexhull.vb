#Region "Microsoft.VisualBasic::ccd3ed9020f2e80c0b541c5489beb9bd, ggplot\src\ggplot\Internal\layers\ggplotConvexhull.vb"

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
    '    Code Lines: 99
    ' Comment Lines: 11
    '   Blank Lines: 16
    '     File Size: 5.19 KB


    '     Class ggplotConvexhull
    ' 
    '         Properties: polygonShape, scale, spline, stroke_width
    ' 
    '         Function: createColorMaps, getClassTags, Plot, renderPolygon
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Math2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Math2D.ConvexHull
Imports Microsoft.VisualBasic.Language
Imports any = Microsoft.VisualBasic.Scripting

Namespace layers

    Public Class ggplotConvexhull : Inherits ggplotLayer

        Public Property spline As Double = 0
        Public Property stroke_width As Single = 3
        ''' <summary>
        ''' the polygon size scale factor
        ''' </summary>
        ''' <returns></returns>
        Public Property scale As Double = 1.25
        Public Property polygonShape As LegendStyles = LegendStyles.RoundRectangle

        Protected Overridable Function getClassTags(stream As ggplotPipeline) As String()
            Return reader.getMapData(Of String)(
                data:=stream.ggplot.data,
                map:=reader.class,
                env:=stream.ggplot.environment
            )
        End Function

        Protected Overridable Function createColorMaps(class_tags As String(), stream As ggplotPipeline, ngroups As Integer) As Dictionary(Of String, Color)
            Dim colors = Designer.GetColors(any.ToString(reader.color), ngroups)
            Dim maps As New Dictionary(Of String, Color)

            For i As Integer = 0 To class_tags.Length - 1
                Call maps.Add(class_tags(i), colors(i))
            Next

            Return maps
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim class_tags As String() = getClassTags(stream)
            Dim y As Double() = stream.layout _
                .Select(Function(v) CDbl(v.Value.Y)) _
                .ToArray
            Dim points As PointF() = stream.layout _
                .Select(Function(xi, i) New PointF(xi.Value.X, y(i))) _
                .ToArray
            Dim polygons As NamedCollection(Of PointF)() = class_tags _
                .Select(Function(tag, i)
                            Return (tag, points(i))
                        End Function) _
                .GroupBy(Function(a) a.tag) _
                .OrderBy(Function(t) t.Key) _
                .Select(Function(tag)
                            Return New NamedCollection(Of PointF) With {
                                .name = tag.Key,
                                .value = tag _
                                    .Select(Function(t) t.Item2) _
                                    .ToArray
                            }
                        End Function) _
                .ToArray
            Dim legendGroup As New List(Of LegendObject)
            Dim colors = createColorMaps(class_tags, stream, ngroups:=polygons.Length)
            Dim legend As LegendObject

            For Each polygon As NamedCollection(Of PointF) In polygons
                legend = renderPolygon(stream, colors(polygon.name), polygon)

                If Not legend Is Nothing Then
                    Call legendGroup.Add(legend)
                End If
            Next

            Return New legendGroupElement With {.legends = legendGroup.ToArray}
        End Function

        Private Function renderPolygon(stream As ggplotPipeline, fillcolor As Color, polygon As NamedCollection(Of PointF)) As LegendObject
            ' 20220524 where filter will cause a mis-ordered
            ' color maps skip of rendering polygn at here instead
            ' of do where filter for the tag groups
            '
            ' .Where(Function(group) group.Count > 3) _
            '
            If polygon.Length <= 3 Then
                Return Nothing
            ElseIf alpha = 0 AndAlso stroke_width = 0 Then
                ' just not display data object?
                Return New LegendObject With {
                    .title = polygon.name,
                    .style = polygonShape,
                    .fontstyle = stream.theme.legendLabelCSS,
                    .color = fillcolor.ToHtmlColor
                }
            End If

            Dim hull As PointF() = polygon.JarvisMatch.Enlarge(scale)
            Dim color As String = fillcolor.ToHtmlColor

            If hull.Length <= 2 Then
                Return Nothing
            End If

            Call HullPolygonDraw.DrawHullPolygon(
                g:=stream.g,
                polygon:=hull,
                color:=fillcolor,
                alpha:=alpha * 255,
                convexHullCurveDegree:=spline,
                strokeWidth:=stroke_width
            )

            Return New LegendObject With {
                .title = polygon.name,
                .style = polygonShape,
                .fontstyle = stream.theme.legendLabelCSS,
                .color = color
            }
        End Function
    End Class
End Namespace
