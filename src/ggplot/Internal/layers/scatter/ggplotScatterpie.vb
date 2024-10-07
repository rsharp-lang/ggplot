#Region "Microsoft.VisualBasic::3adabe3b61bdef5c1f889d3711e50f12, src\ggplot\Internal\layers\scatter\ggplotScatterpie.vb"

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

    '   Total Lines: 86
    '    Code Lines: 69 (80.23%)
    ' Comment Lines: 9 (10.47%)
    '    - Xml Docs: 88.89%
    ' 
    '   Blank Lines: 8 (9.30%)
    '     File Size: 3.44 KB


    '     Class ggplotScatterpie
    ' 
    '         Properties: pie
    ' 
    '         Function: Plot
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
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports std = System.Math

Namespace layers

    ''' <summary>
    ''' this plot combine the scatter plot with the pie 
    ''' plot, each scatter point is a pie chart.
    ''' </summary>
    Public Class ggplotScatterpie : Inherits ggplotScatterLayer

        ''' <summary>
        ''' the pie group names across all scatter points data
        ''' </summary>
        ''' <returns></returns>
        Public Property pie As String()

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            ' get scatter data
            Dim data As dataframe = stream.ggplot.data
            Dim x As Double() = stream.scale.X(stream.x)
            Dim y As Double() = stream.scale.Y(stream.y)
            Dim piedata = pie _
                .ToDictionary(Function(name) name,
                              Function(name)
                                  Return CLRVector.asNumeric(data.getVector(name, fullSize:=True))
                              End Function)
            Dim cellSize = getCellsize(x, y)
            Dim colors As Color() = Designer.GetColors(stream.theme.colorSet, n:=pie.Length)
            Dim radius As Single = std.Min(cellSize.Width, cellSize.Height) / 2

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
