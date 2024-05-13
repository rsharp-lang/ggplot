#Region "Microsoft.VisualBasic::4b3d2cbd2d52d93fd96292d5a58ee892, src\ggplot\Internal\layers\groupPlot\geom_bar.vb"

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

    '   Total Lines: 130
    '    Code Lines: 102
    ' Comment Lines: 7
    '   Blank Lines: 21
    '     File Size: 5.52 KB


    '     Class geom_bar
    ' 
    '         Properties: position, stat
    ' 
    '         Function: aggregate_sum, getYAxis, PlotOrdinal
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots.BarPlot
Imports Microsoft.VisualBasic.Data.ChartPlots.BarPlot.Data
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports stdNum = System.Math

Namespace layers

    Public Class geom_bar : Inherits ggplotGroup

        ''' <summary>
        ''' the bar height evaluation method, value could be 
        ''' 
        ''' 1. identity
        ''' 2. percentage
        ''' </summary>
        ''' <returns></returns>
        Public Property stat As String
        Public Property position As String

        Public Overrides Function getYAxis(y() As Double, ggplot As ggplot) As axisMap
            Dim groupName As String = ggplot.base.reader.color

            If stat = "percentage" Then
                Return axisMap.FromNumeric({0, 1})
            End If

            If TypeOf ggplot.data Is dataframe Then
                Dim groupFactors As String() = CLRVector.asCharacter(DirectCast(ggplot.data, dataframe)(groupName))
                Dim zip = y.Zip(groupFactors).GroupBy(Function(z) z.Second).ToArray
                Dim max As Double = zip.Max(Function(a) a.Sum(Function(i) i.First))
                Dim min As Double = zip.Min(Function(a) a.Sum(Function(i) i.First))

                min = stdNum.Min(0, min)

                Return axisMap.FromNumeric({min, max})
            Else
                Throw New NotImplementedException
            End If
        End Function

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As OrdinalScale) As IggplotLegendElement
            Dim groupName As String = stream.ggplot.base.reader.color
            Dim ggplot As ggplot = stream.ggplot
            Dim legends As legendGroupElement = Nothing

            If TypeOf stream.ggplot.data Is dataframe Then
                Dim groupFactors As String() = CLRVector.asCharacter(DirectCast(stream.ggplot.data, dataframe)(groupName))
                Dim colors As String() = Nothing
                Dim nsize As Integer = stream.x.Length
                Dim y As Double() = stream.y

                If useCustomColorMaps Then
                    colors = getColorSet(ggplot, LegendStyles.Rectangle, groupFactors, legends)
                ElseIf Not ggplot.base.reader.color Is Nothing Then
                    colors = ggplot.base.getColors(ggplot, legends, LegendStyles.Rectangle)
                End If

                Dim zip = groupFactors.Zip(y).Zip(CLRVector.asCharacter(stream.x)).GroupBy(Function(a) a.Second).ToArray
                Dim fill = legends.legends _
                    .Select(Function(l) New NamedValue(Of Color)(l.title, l.color.TranslateColor)) _
                    .ToArray
                Dim groupData As New List(Of BarDataSample)

                For Each group In zip
                    Dim sum = group _
                        .Select(Function(d) d.First) _
                        .GroupBy(Function(a) a.First) _
                        .ToDictionary(Function(a) a.Key,
                                      Function(a)
                                          Return aggregate_sum(a)
                                      End Function)

                    Call groupData.Add(New BarDataSample With {
                        .tag = group.Key,
                        .data = fill _
                            .Select(Function(a) sum.TryGetValue(a.Name, [default]:=0)) _
                            .ToArray
                    })
                Next

                Dim stackbars As New BarDataGroup With {
                    .Samples = groupData.ToArray,
                    .Serials = fill
                }

                Call stackbars.Samples _
                    .Select(Function(a) a.tag) _
                    .ToArray _
                    .GetJson _
                    .DoCall(AddressOf VBDebugger.EchoLine)

                If ggplot.ggplotTheme.flipAxis Then
                    Dim width = stream.canvas.PlotRegion.Height / stackbars.Samples.Length
                    width = width - width * groupWidth
                    Dim dw As Double = width / 2

                    Call StackedPercentageBarPlot.DrawStackBarsFlip(stackbars, stream.g, stream.canvas, dw)
                Else
                    Dim width = stream.canvas.PlotRegion.Width / stackbars.Samples.Length
                    width = width - width * groupWidth
                    Dim dw As Double = width / 2

                    Call StackedPercentageBarPlot.DrawStackBars(stackbars, stream.g, stream.canvas, dw)
                End If
            Else
                Throw New NotImplementedException
            End If

            If showLegend Then
                Return legends
            Else
                Return Nothing
            End If
        End Function

        Private Shared Function aggregate_sum(a As IEnumerable(Of (first$, second#))) As Double
            Return Aggregate xi In a Into Sum(xi.second)
        End Function
    End Class
End Namespace
