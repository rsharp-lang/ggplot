#Region "Microsoft.VisualBasic::d1d614ff61e5590b66359922d80fbd74, src\ggplot\Internal\layers\groupPlot\ggplotPie.vb"

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

    '   Total Lines: 72
    '    Code Lines: 66
    ' Comment Lines: 0
    '   Blank Lines: 6
    '     File Size: 3.11 KB


    '     Class ggplotPie
    ' 
    '         Properties: valueLabel
    ' 
    '         Function: PlotOrdinal
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Fractions
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports stdNum = System.Math

Namespace layers

    Public Class ggplotPie : Inherits ggplotGroup

        Public Property valueLabel As ValueLabels = ValueLabels.Percentage

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As OrdinalScale) As IggplotLegendElement
            Dim allGroupData = getDataGroups(stream).ToArray
            Dim colors As Func(Of Object, String) = getColors(stream, allGroupData.Select(Function(i) i.name))
            Dim colorSet = allGroupData _
                .Select(Function(group)
                            Return colors(group.name) _
                                .TranslateColor _
                                .Alpha(alpha * 255)
                        End Function) _
                .ToArray
            Dim sumAll = Aggregate group In allGroupData Into Sum(group.First)
            Dim pie As FractionData() = allGroupData _
                .Select(Function(group, i)
                            Return New FractionData With {
                                .Color = colorSet(i),
                                .Name = group.name,
                                .Value = group.First,
                                .Percentage = group.First / sumAll
                            }
                        End Function) _
                .ToArray
            Dim labelFont As Font = CSSFont.TryParse(stream.theme.tagCSS).GDIObject(stream.g.Dpi)
            Dim r As Single = stdNum.Min(stream.canvas.PlotRegion.Height, stream.canvas.PlotRegion.Width) / 2
            Dim topLeft As New Point With {
                .X = stream.canvas.Padding.Left,
                .Y = stream.canvas.Padding.Top
            }

            Call stream.g.PlotPie(
                topLeft, pie,
                valueLabelFont:=labelFont,
                font:=labelFont,
                layoutRect:=Nothing,
                r:=r,
                shadowAngle:=0,
                shadowDistance:=0,
                valueLabel:=valueLabel,
                legendAlt:=Nothing
            )

            Return New legendGroupElement With {
                .layout = Nothing,
                .legends = pie _
                    .Select(Function(a)
                                Return New LegendObject With {
                                    .color = a.Color.ToHtmlColor,
                                    .fontstyle = stream.theme.legendLabelCSS,
                                    .style = LegendStyles.Rectangle,
                                    .title = a.Name
                                }
                            End Function) _
                    .ToArray,
                .shapeSize = Nothing
            }
        End Function
    End Class
End Namespace
