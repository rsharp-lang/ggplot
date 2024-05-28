#Region "Microsoft.VisualBasic::8da274990e528c4e9fb31f38075be62f, src\ggplot\Internal\layers\groupPlot\ggplotBarplot.vb"

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

    '   Total Lines: 55
    '    Code Lines: 47 (85.45%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 8 (14.55%)
    '     File Size: 2.83 KB


    '     Class ggplotBarplot
    ' 
    '         Function: PlotOrdinal
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports stdNum = System.Math

Namespace layers

    Public Class ggplotBarplot : Inherits ggplotGroup

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, xscale As d3js.scale.OrdinalScale) As IggplotLegendElement
            Dim g As IGraphics = stream.g
            Dim binWidth As Double = DirectCast(stream.scale.X, d3js.scale.OrdinalScale).binWidth
            Dim yscale As YScaler = stream.scale
            Dim boxWidth As Double = binWidth * groupWidth
            Dim lineStroke As Pen = Stroke.TryParse(stream.theme.lineStroke).GDIObject
            Dim labelFont As Font = CSSFont.TryParse(stream.theme.tagCSS).GDIObject(g.Dpi)
            Dim allGroupData = getDataGroups(stream).ToArray
            Dim colors As Func(Of Object, String) = getColors(stream, allGroupData.Select(Function(i) i.name))
            Dim y As DataScaler = stream.scale
            Dim bottom As Double = stream.canvas.PlotRegion.Bottom
            Dim top As Double = stream.canvas.PlotRegion.Top
            Dim line As Pen = Stroke.TryParse(stream.theme.gridStrokeX).GDIObject

            For Each group As NamedCollection(Of Double) In allGroupData
                Dim x As Double = xscale(group.name)
                Dim mean As Double = group.Average
                Dim yi As Double = y.TranslateY(mean)
                Dim bar As New RectangleF(x - boxWidth / 2, yi, boxWidth, bottom - yi)
                Dim color As Color = colors(group.name).TranslateColor.Alpha(255 * alpha)
                Dim paint As New SolidBrush(color)
                Dim std As Double = stdNum.Abs(group.SD)

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
