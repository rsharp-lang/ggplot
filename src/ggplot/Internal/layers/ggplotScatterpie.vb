#Region "Microsoft.VisualBasic::c03324da998616fa47408fbec3e97394, ggplot\src\ggplot\Internal\layers\ggplotScatterpie.vb"

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
    '    Code Lines: 79
    ' Comment Lines: 14
    '   Blank Lines: 9
    '     File Size: 4.11 KB


    '     Class ggplotScatterpie
    ' 
    '         Properties: minCell, pie
    ' 
    '         Function: getMeanCell, Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Fractions
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Math
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime
Imports stdNum = System.Math

Namespace layers

    ''' <summary>
    ''' this plot combine the scatter plot with the pie 
    ''' plot, each scatter point is a pie chart.
    ''' </summary>
    Public Class ggplotScatterpie : Inherits ggplotLayer

        ''' <summary>
        ''' the pie group names across all scatter points data
        ''' </summary>
        ''' <returns></returns>
        Public Property pie As String()
        ''' <summary>
        ''' the min cell width/height
        ''' </summary>
        ''' <returns></returns>
        Public Property minCell As Integer = 16

        Private Function getMeanCell(data As Double(), top_n As Integer) As Double
            Return NumberGroups.diff(data.OrderBy(Function(xi) xi).ToArray) _
                .OrderByDescending(Function(a) a) _
                .Take(top_n) _
                .Average
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            ' get scatter data
            Dim data As dataframe = stream.ggplot.data
            Dim x As Double() = stream.scale.X(stream.x)
            Dim y As Double() = stream.scale.Y(stream.y)
            Dim piedata = pie _
                .ToDictionary(Function(name) name,
                              Function(name)
                                  Return DirectCast(REnv.asVector(Of Double)(data.getVector(name, fullSize:=True)), Double())
                              End Function)
            ' evaluate cells grid
            Dim topN As Integer = stdNum.Min(x.Length / 5, 100)
            Dim cellWidth = getMeanCell(x, topN)
            Dim cellHeight = getMeanCell(y, topN)
            Dim colors As Color() = Designer.GetColors(stream.theme.colorSet, n:=pie.Length)
            Dim radius As Single = stdNum.Min(cellWidth, cellHeight) / 2

            If radius < minCell Then
                radius = minCell
            End If

            For i As Integer = 0 To x.Length - 1
                Dim xi = x(i) - radius
                Dim yi = y(i) - radius
                Dim pie As FractionData() = Me.pie _
                    .Select(Function(name, pi)
#Disable Warning
                                Return New FractionData() With {
                                    .Name = name,
                                    .Color = colors(pi),
                                    .Value = piedata(name)(i)
                                }
#Enable Warning
                            End Function) _
                    .ToArray

                Call stream.g.PlotPie(
                    topLeft:=New Point(xi, yi),
                    data:=pie,
                    valueLabelFont:=Nothing,
                    font:=Nothing,
                    layoutRect:=Nothing,
                    r:=radius,
                    shadowAngle:=0,
                    shadowDistance:=0,
                    valueLabel:=ValueLabels.None,
                    legendAlt:=Nothing
                )
            Next

            Return New legendGroupElement With {
                .legends = pie _
                    .Select(Function(name, i)
                                Return New LegendObject With {
                                    .color = colors(i).ToHtmlColor,
                                    .fontstyle = stream.theme.legendLabelCSS,
                                    .style = LegendStyles.Rectangle,
                                    .title = name
                                }
                            End Function) _
                    .ToArray
            }
        End Function
    End Class
End Namespace
