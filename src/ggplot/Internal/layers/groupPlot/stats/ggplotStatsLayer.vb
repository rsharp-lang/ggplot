#Region "Microsoft.VisualBasic::3082b13f4afc0687f581ac474796ab9f, src\ggplot\Internal\layers\groupPlot\stats\ggplotStatsLayer.vb"

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

    '   Total Lines: 64
    '    Code Lines: 47 (73.44%)
    ' Comment Lines: 8 (12.50%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 9 (14.06%)
    '     File Size: 3.05 KB


    '     Class ggplotStatsLayer
    ' 
    '         Properties: stats
    ' 
    '         Function: PlotOrdinal
    ' 
    '         Sub: drawLayer
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports std = System.Math

Namespace layers

    Public Class ggplotStatsLayer : Inherits ggplotGroup

        ' compare_means(len~dose, data=ToothGrowth)
        ' ## A tibble: 3 x 8
        ' #  .y.   group1 group2           p    p.adj p.format p.signif
        ' #  <chr> <chr>  <chr>        <dbl>    <dbl> <chr>    <chr>   
        ' #1 len   0.5    1          7.02e-6   1.4e-5 7.0e-06  ****    
        ' #2 len   0.5    2          8.41e-8   2.5e-7 8.4e-08  ****    
        ' #3 len   1      2          1.77e-4   1.8e-4 0.00018  ***     
        ' ## … with 1 more variable: method <chr>
        Public Property stats As compare_means()

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As OrdinalScale) As IggplotLegendElement
            Call drawLayer(stream, x)
            Return Nothing
        End Function

        Private Sub drawLayer(stream As ggplotPipeline, x As OrdinalScale)
            Dim data As Dictionary(Of String, Double()) = getDataGroups(stream) _
                .ToDictionary(Function(v) v.name,
                              Function(v)
                                  Return v.value
                              End Function)
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim line As Pen = css.GetPen(Stroke.TryParse(stream.theme.lineStroke))
            Dim font As Font = css.GetFont(CSSFont.TryParse(stream.theme.tagCSS))
            Dim lbsize As SizeF
            Dim pos As PointF
            Dim h As Double

            For Each compare As compare_means In stats
                Dim group1 As Double() = data.TryGetValue(compare.group1)
                Dim group2 As Double() = data.TryGetValue(compare.group2)
                Dim x1 As Double = x(compare.group1)
                Dim x2 As Double = x(compare.group2)
                Dim len As Double = std.Abs(x1 - x2)
                Dim left As Double = std.Min(x1, x2)
                Dim y1 As Double = getLabelPosY(group1, stream.scale, factor:=1.025)
                Dim y2 As Double = getLabelPosY(group2, stream.scale, factor:=1.025)
                Dim y As Double = std.Min(y1, y2)
                Dim siglab As String = compare.psignif

                lbsize = stream.g.MeasureString(siglab, font)
                pos = New PointF(left + (len - lbsize.Width) / 2, y - lbsize.Height * 1.125)
                h = lbsize.Height / 3

                Call stream.g.DrawLine(line, New PointF(x1, y), New PointF(x2, y))
                Call stream.g.DrawLine(line, New PointF(x1, y), New PointF(x1, y + h))
                Call stream.g.DrawLine(line, New PointF(x2, y), New PointF(x2, y + h))

                Call stream.g.DrawString(siglab, font, Brushes.Black, pos)
            Next
        End Sub
    End Class
End Namespace
