#Region "Microsoft.VisualBasic::69312eeadc35fefa1d988d22c5aedef6, src\ggplot\Internal\layers\groupPlot\ggplotBarplot.vb"

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

    '   Total Lines: 85
    '    Code Lines: 76 (89.41%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 9 (10.59%)
    '     File Size: 4.15 KB


    '     Class ggplotBarplot
    ' 
    '         Function: PlotOrdinal
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Math
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render

#If NET48 Then
Imports Pen = System.Drawing.Pen
Imports Pens = System.Drawing.Pens
Imports Brush = System.Drawing.Brush
Imports Font = System.Drawing.Font
Imports Brushes = System.Drawing.Brushes
Imports SolidBrush = System.Drawing.SolidBrush
Imports DashStyle = System.Drawing.Drawing2D.DashStyle
Imports Image = System.Drawing.Image
Imports Bitmap = System.Drawing.Bitmap
Imports GraphicsPath = System.Drawing.Drawing2D.GraphicsPath
Imports LineCap = System.Drawing.Drawing2D.LineCap
Imports TextureBrush = System.Drawing.TextureBrush
#Else
Imports Pen = Microsoft.VisualBasic.Imaging.Pen
Imports Pens = Microsoft.VisualBasic.Imaging.Pens
Imports Brush = Microsoft.VisualBasic.Imaging.Brush
Imports Font = Microsoft.VisualBasic.Imaging.Font
Imports Brushes = Microsoft.VisualBasic.Imaging.Brushes
Imports SolidBrush = Microsoft.VisualBasic.Imaging.SolidBrush
Imports DashStyle = Microsoft.VisualBasic.Imaging.DashStyle
Imports Image = Microsoft.VisualBasic.Imaging.Image
Imports Bitmap = Microsoft.VisualBasic.Imaging.Bitmap
Imports GraphicsPath = Microsoft.VisualBasic.Imaging.GraphicsPath
Imports LineCap = Microsoft.VisualBasic.Imaging.LineCap
Imports TextureBrush = Microsoft.VisualBasic.Imaging.TextureBrush
#End If

Namespace layers

    Public Class ggplotBarplot : Inherits ggplotGroup

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, xscale As d3js.scale.OrdinalScale) As IggplotLegendElement
            Dim g As IGraphics = stream.g
            Dim binWidth As Double = DirectCast(stream.scale.X, d3js.scale.OrdinalScale).binWidth
            Dim yscale As YScaler = stream.scale
            Dim boxWidth As Double = binWidth * groupWidth
            Dim css As CSSEnvirnment = g.LoadEnvironment
            Dim lineStroke As Pen = css.GetPen(Stroke.TryParse(stream.theme.lineStroke))
            Dim labelFont As Font = css.GetFont(stream.theme.tagCSS)
            Dim allGroupData = getDataGroups(stream).ToArray
            Dim colors As Func(Of Object, String) = getColors(stream, allGroupData.Select(Function(i) i.name))
            Dim y As DataScaler = stream.scale
            Dim plotRegion = stream.canvas.PlotRegion(css)
            Dim bottom As Double = plotRegion.Bottom
            Dim top As Double = plotRegion.Top
            Dim line As Pen = css.GetPen(Stroke.TryParse(stream.theme.lineStroke))

            For Each group As NamedCollection(Of Double) In allGroupData
                Dim x As Double = xscale(group.name)
                Dim mean As Double = group.Average
                Dim yi As Double = y.TranslateY(mean)
                Dim bar As New RectangleF(x - boxWidth / 2, yi, boxWidth, bottom - yi)
                Dim color As Color = colors(group.name).TranslateColor.Alpha(255 * alpha)
                Dim paint As New SolidBrush(color)
                Dim std As Double = Abs(group.SD)

                Call g.DrawLine(line, New PointF(x, top), New PointF(x, bottom))
                Call g.FillRectangle(paint, bar)
                Call g.DrawRectangle(lineStroke, bar)

                Dim y1 As Double = y.TranslateY(mean + std)
                Dim y2 As Double = y.TranslateY(mean - std)
                Dim widthInner As Double = boxWidth / 3
                Dim line2 As New Pen(color, line.Width * 2)

                Call g.DrawLine(line2, New PointF(x - widthInner, y1), New PointF(x + widthInner, y1))
                Call g.DrawLine(line2, New PointF(x - widthInner, y2), New PointF(x + widthInner, y2))
                Call g.DrawLine(line2, New PointF(x, y1), New PointF(x, yi))
                Call g.DrawLine(line2, New PointF(x, y2), New PointF(x, yi))
            Next

            Return Nothing
        End Function
    End Class
End Namespace
