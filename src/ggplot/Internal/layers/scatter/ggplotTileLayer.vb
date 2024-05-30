#Region "Microsoft.VisualBasic::93aaf2a6ba4912ef12b02a1828c17a88, src\ggplot\Internal\layers\scatter\ggplotTileLayer.vb"

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

    '   Total Lines: 77
    '    Code Lines: 62 (80.52%)
    ' Comment Lines: 6 (7.79%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 9 (11.69%)
    '     File Size: 3.70 KB


    '     Class ggplotTileLayer
    ' 
    '         Properties: mapLevels
    ' 
    '         Function: getDataLabel, getFillData, Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render
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
            Dim tile_size As SizeF = stream.scale.TranslateSize(diffx(diffx > 0).Min, diffy(diffy > 0).Min)
            Dim rect As RectangleF
            ' Dim offsetx As Double = tile_size.Width / 2
            ' Dim offsety As Double = tile_size.Height / 2
            Dim fill As Brush
            Dim fillData As Double() = getFillData(stream)
            Dim ggplot = stream.ggplot
            Dim colors As String() = getColorSet(ggplot, stream.g, mapLevels, LegendStyles.SolidLine, fillData, Nothing)
            Dim textures As Brush() = colors.Select(Function(c) c.GetBrush).ToArray
            Dim rxi, ryi As Double
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment

            For i As Integer = 0 To x.Length - 1
                rxi = stream.scale.TranslateX(x(i)) - tile_size.Width
                ryi = stream.scale.TranslateY(y(i)) '- offsety
                rect = New RectangleF(New PointF(rxi, ryi), tile_size)
                fill = textures(i)
                stream.g.FillRectangle(fill, rect)
            Next

            Return New legendColorMapElement With {
                .colorMapLegend = New ColorMapLegend(colorMap.ToString, mapLevels) With {
                    .title = getDataLabel(stream),
                    .tickAxisStroke = Stroke.TryParse(stream.theme.legendTickAxisStroke).GDIObject,
                    .tickFont = css.GetFont(stream.theme.legendTickCSS),
                    .format = stream.theme.legendTickFormat,
                    .ticks = fillData.CreateAxisTicks,
                    .titleFont = css.GetFont(stream.theme.legendTitleCSS)
                },
                .width = stream.canvas.Padding.Right * 3 / 4,
                .height = stream.canvas.PlotRegion.Height
            }
        End Function
    End Class
End Namespace
