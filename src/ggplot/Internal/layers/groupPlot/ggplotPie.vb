#Region "Microsoft.VisualBasic::e52dd5b6c91af8561b3212d70c66ba96, src\ggplot\Internal\layers\groupPlot\ggplotPie.vb"

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

    '   Total Lines: 102
    '    Code Lines: 95 (93.14%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 7 (6.86%)
    '     File Size: 4.46 KB


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
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports std = System.Math

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
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim padding As PaddingLayout = PaddingLayout.EvaluateFromCSS(css, stream.canvas.Padding)
            Dim labelFont As Font = css.GetFont(CSSFont.TryParse(stream.theme.tagCSS))
            Dim plotRegion As Rectangle = stream.canvas.PlotRegion(css)
            Dim r As Single = std.Min(plotRegion.Height, plotRegion.Width) / 2
            Dim topLeft As New Point With {
                .X = padding.Left,
                .Y = padding.Top
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
