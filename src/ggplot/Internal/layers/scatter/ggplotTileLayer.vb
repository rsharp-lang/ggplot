Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Math
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    ' geom_rect() and geom_tile() do the same thing, but are parameterised differently:
    '
    ' geom_rect() uses the locations of the four corners (xmin, xmax, ymin and ymax), while
    ' geom_tile() uses the center of the tile and its size (x, y, width, height)

    Public Class ggplotTileLayer : Inherits ggplotLayer

        Private Function getFillData(stream As ggplotPipeline) As Double()
            If useCustomData OrElse useCustomColorMaps Then
                Return reader.getMapData(Of Double)(stream.ggplot.data, reader.color, stream.ggplot.environment)
            Else
                Return stream.y
            End If
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim x = CLRVector.asNumeric(stream.x)
            Dim y = CLRVector.asNumeric(stream.y)
            Dim diffx As Double() = NumberGroups.diff(x.OrderByDescending(Function(xi) xi).ToArray)
            Dim diffy As Double() = NumberGroups.diff(y.OrderByDescending(Function(xi) xi).ToArray)
            Dim tile_size As SizeF = stream.scale.TranslateSize(diffx.Average, diffy.Average)
            Dim rect As RectangleF
            Dim offsetx As Double = tile_size.Width / 2
            Dim offsety As Double = tile_size.Height / 2
            Dim fill As Brush
            Dim fillData As Double() = getFillData(stream)
            Dim valuerange As New DoubleRange(fillData)
            Dim ggplot = stream.ggplot
            Dim legends As IggplotLegendElement = Nothing
            Dim colors As String() = getColorSet(ggplot, stream.g, 100, LegendStyles.SolidLine, fillData, legends)
            Dim textures As Brush() = colors.Select(Function(c) c.GetBrush).ToArray
            Dim indexrange As New DoubleRange(0, 99)
            Dim offset As Integer
            Dim rxi, ryi As Double

            For i As Integer = 0 To x.Length - 1
                rxi = stream.scale.TranslateX(x(i) - offsetx)
                ryi = stream.scale.TranslateY(y(i) - offsety)
                rect = New RectangleF(New PointF(rxi, ryi), tile_size)
                offset = valuerange.ScaleMapping(fillData(i), indexrange)
                fill = textures(offset)
                stream.g.FillRectangle(fill, rect)
            Next

            Return legends
        End Function
    End Class
End Namespace