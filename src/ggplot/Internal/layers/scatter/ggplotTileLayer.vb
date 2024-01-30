Imports System.Drawing
Imports System.IO
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    ' geom_rect() and geom_tile() do the same thing, but are parameterised differently:
    '
    ' geom_rect() uses the locations of the four corners (xmin, xmax, ymin and ymax), while
    ' geom_tile() uses the center of the tile and its size (x, y, width, height)

    Public Class ggplotTileLayer : Inherits ggplotLayer

        Public Property mapLevels As Integer = 100

        Private Function getFillData(stream As ggplotPipeline) As Double()
            If useCustomData OrElse useCustomColorMaps Then
                Return reader.getMapData(Of Double)(stream.ggplot.data, reader.color, stream.ggplot.environment)
            Else
                Return stream.y
            End If
        End Function

        Private Function getDataLabel(stream As ggplotPipeline) As String
            If useCustomData OrElse useCustomColorMaps Then
                Return reader.color.ToString
            Else
                Return stream.ggplot.base.reader.y
            End If
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim x = CLRVector.asNumeric(stream.x)
            Dim y = CLRVector.asNumeric(stream.y)
            Dim diffx As Vector = NumberGroups.diff(x.OrderBy(Function(xi) xi).ToArray)
            Dim diffy As Vector = NumberGroups.diff(y.OrderBy(Function(xi) xi).ToArray)
            Dim tile_size As SizeF = stream.scale.TranslateSize(diffx(diffx > 0).Average, diffy(diffy > 0).Average)
            Dim rect As RectangleF
            Dim offsetx As Double = tile_size.Width / 2
            Dim offsety As Double = tile_size.Height / 2
            Dim fill As Brush
            Dim fillData As Double() = getFillData(stream)
            Dim valuerange As New DoubleRange(fillData)
            Dim ggplot = stream.ggplot
            Dim colors As String() = getColorSet(ggplot, stream.g, mapLevels, LegendStyles.SolidLine, fillData, Nothing)
            Dim textures As Brush() = colors.Select(Function(c) c.GetBrush).ToArray
            Dim indexrange As New DoubleRange(0, mapLevels - 1)
            Dim offset As Integer
            Dim rxi, ryi As Double

            For i As Integer = 0 To x.Length - 1
                rxi = stream.scale.TranslateX(x(i)) - offsetx
                ryi = stream.scale.TranslateY(y(i)) - offsety
                rect = New RectangleF(New PointF(rxi, ryi), tile_size)
                offset = valuerange.ScaleMapping(fillData(i), indexrange)
                fill = textures(offset)
                stream.g.FillRectangle(fill, rect)
            Next

            Return New legendColorMapElement With {
                .colorMapLegend = New ColorMapLegend(stream.theme.colorSet, mapLevels) With {
                    .title = getDataLabel(stream),
                    .tickAxisStroke = Stroke.TryParse(stream.theme.legendTickAxisStroke).GDIObject,
                    .tickFont = CSSFont.TryParse(stream.theme.legendTickCSS).GDIObject(stream.g.Dpi),
                    .format = stream.theme.legendTickFormat,
                    .ticks = fillData.CreateAxisTicks,
                    .titleFont = CSSFont.TryParse(stream.theme.legendTitleCSS).GDIObject(stream.g.Dpi)
                },
                .width = stream.canvas.Padding.Right * 3 / 4,
                .height = stream.canvas.PlotRegion.Height
            }
        End Function
    End Class
End Namespace