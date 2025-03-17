#Region "Microsoft.VisualBasic::8242551e125dbbbc8076b0076b454116, src\ggplot\Internal\layers\ggplotLine.vb"

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

'   Total Lines: 61
'    Code Lines: 51 (83.61%)
' Comment Lines: 0 (0.00%)
'    - Xml Docs: 0.00%
' 
'   Blank Lines: 10 (16.39%)
'     File Size: 2.79 KB


'     Class ggplotLine
' 
'         Properties: line_width
' 
'         Function: Plot
' 
' 
' /********************************************************************************/

#End Region

Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Plots
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Math.Interpolation
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    ''' <summary>
    ''' A line chart, also known as a line plot or line graph, is a type of chart used to display the trend, progress, 
    ''' or change over time or between different conditions or categories. It is one of the most common and simple 
    ''' methods for visualizing data sequences. Here's an introduction to the components and purposes of a line chart:
    ''' 
    ''' ### Components of a Line Chart:
    ''' 
    ''' 1. **Horizontal Axis (X-axis):** This axis typically represents the independent variable, often time, which is placed on the horizontal line. Each point on the axis corresponds to a specific point in time or a category.
    ''' 2. **Vertical Axis (Y-axis):** This axis represents the dependent variable, which is the data that is being measured or tracked. The scale on the y-axis can be linear or logarithmic, depending on the range and distribution of the data.
    ''' 3. **Lines:** These are the curves or straight lines that connect the data points. Each line usually represents a different data series or category.
    ''' 4. **Data Points:** These are the individual markers on the chart that indicate the value of the dependent variable at a specific point on the x-axis. They can be represented by dots, squares, or other symbols.
    ''' 5. **Legend:** When multiple data series are represented on the same chart, a legend is used to explain what each line represents.
    ''' 
    ''' ### Purposes of a Line Chart:
    ''' 
    ''' - **Show Trends:** Line charts are ideal for showing trends over time, such as the stock market performance, temperature changes, or website traffic.
    ''' - **Compare Data Series:** They can be used to compare the performance of different groups or categories over the same period.
    ''' - **Highlight Patterns:** Line charts can reveal patterns such as cycles, trends, seasonality, or irregularities in the data.
    ''' - **Forecasting:** By identifying trends, line charts can be used as a tool for making predictions about future data points.
    ''' 
    ''' Line charts are versatile and can be used in a wide range of fields, including business, economics, science, and
    ''' education, to present data in a clear and concise manner.
    ''' </summary>
    Public Class ggplotLine : Inherits ggplotLayer

        Public Property line_width As Single = 5
        Public Property bspline As Boolean = False

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim serials As SerialData()
            Dim colors As String() = Nothing
            Dim legends As IggplotLegendElement = Nothing
            Dim ggplot As ggplot = stream.ggplot
            Dim g As IGraphics = stream.g
            Dim multiple_groups As Boolean

            If Not useCustomData Then
                Dim x = CLRVector.asFloat(stream.x)
                Dim y = stream.y
                Dim nsize As Integer = x.Length

                If useCustomColorMaps Then
                    colors = getColorSet(ggplot, g, nsize, LegendStyles.SolidLine, y, legends)
                ElseIf Not ggplot.base.reader.color Is Nothing Then
                    colors = ggplot.base.getColors(ggplot, legends, LegendStyles.SolidLine)
                End If

                multiple_groups = ggplotBase.checkMultipleLegendGroup(legends)
                serials = ggplotScatter.createSerialData(
                    ggplot.base.reader.ToString,
                    x, CLRVector.asFloat(y), colors,
                    line_width,
                    TryCast(legends, legendGroupElement), LegendStyles.SolidLine, colorMap).ToArray
            Else
                With Me.data
                    If useCustomColorMaps Then
                        colors = getColorSet(ggplot, g, .nsize, LegendStyles.SolidLine, .y, legends)
                    ElseIf Not ggplot.base.reader.color Is Nothing Then
                        colors = ggplot.base.getColors(ggplot, legends, LegendStyles.SolidLine)
                    Else
                        colors = (++ggplot.colors).Replicate(.nsize).ToArray
                        legends = New ggplotLegendElement With {
                            .legend = New LegendObject With {
                                .color = colors(Scan0),
                                .fontstyle = stream.theme.legendLabelCSS,
                                .style = LegendStyles.SolidLine,
                                .title = reader.getLegendLabel
                            }
                        }
                    End If

                    multiple_groups = ggplotBase.checkMultipleLegendGroup(legends)
                    serials = ggplotScatter.createSerialData(
                        reader.ToString,
                        .x.ToFloat, .y.ToFloat, colors,
                        line_width,
                        TryCast(legends, legendGroupElement), LegendStyles.SolidLine, colorMap).ToArray
                End With
            End If

            For Each serial As SerialData In serials
                Call LinePlot2D.DrawLine(
                    stream.g, stream.canvas, stream.scale,
                    serial,
                    interplot:=If(bspline, Splines.B_Spline, Splines.None)
                )
            Next

            Return legends
        End Function
    End Class
End Namespace
