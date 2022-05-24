Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Math2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Math2D.ConvexHull
Imports Microsoft.VisualBasic.Language

Namespace layers

    Public Class ggplotConvexhull : Inherits ggplotLayer

        Public Property alpha As Double = 1

        Protected Overridable Function getClassTags(stream As ggplotPipeline) As String()
            Return reader.getMapData(Of String)(
                data:=stream.ggplot.data,
                map:=reader.class,
                env:=stream.ggplot.environment
            )
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim class_tags As String() = getClassTags(stream)
            Dim y As Double() = stream.y
            Dim points As PointF() = stream.x _
                .Select(Function(xi, i) New PointF(xi, y(i))) _
                .ToArray
            Dim polygons As NamedCollection(Of PointF)() = class_tags _
                .Select(Function(tag, i)
                            Return (tag, points(i))
                        End Function) _
                .GroupBy(Function(a) a.tag) _
                .Where(Function(group) group.Count > 2) _
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
            Dim colors = reader.getMapColor(reader.color, LegendStyles.RoundRectangle, stream.theme, stream.ggplot.environment)
            Dim idx As i32 = 0

            For Each polygon As NamedCollection(Of PointF) In polygons
                Call legendGroup.Add(renderPolygon(stream, colors.htmlColors(++idx), polygon))
            Next

            Return New legendGroupElement With {.legends = legendGroup.ToArray}
        End Function

        Private Function renderPolygon(stream As ggplotPipeline, color As String, polygon As NamedCollection(Of PointF)) As LegendObject
            Dim hull As PointF() = polygon.JarvisMatch
            Dim fillColor As Color = color.TranslateColor

            Call HullPolygonDraw.DrawHullPolygon(stream.g, hull, fillColor, alpha:=alpha * 255)

            Return New LegendObject With {
                .title = polygon.name,
                .style = LegendStyles.RoundRectangle,
                .fontstyle = stream.theme.legendLabelCSS,
                .color = color
            }
        End Function
    End Class
End Namespace