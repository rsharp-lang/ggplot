#Region "Microsoft.VisualBasic::3bcbc3906407ec7131c6dc6b98a8b6e0, src\ggplot\Internal\layers\ggplotHistogram.vb"

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

    '   Total Lines: 81
    '    Code Lines: 68 (83.95%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 13 (16.05%)
    '     File Size: 3.02 KB


    '     Class ggplotHistogram
    ' 
    '         Properties: bins, range
    ' 
    '         Function: getColorName, Plot
    ' 
    '         Sub: configHistogram
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.colors
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.BarPlot.Histogram
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Math.Distributions.BinBox
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    ''' <summary>
    ''' A histogram is a type of bar plot that represents the distribution of numerical data. It is an estimate of the 
    ''' probability distribution of a continuous variable and was first introduced by Karl Pearson. Histograms are particularly 
    ''' useful for showcasing the frequency distribution of a dataset, which can provide insights into the underlying
    ''' properties of the data.
    ''' 
    ''' Here's how a histogram is constructed and what each of its components represents:
    ''' 
    ''' ### Construction of a Histogram:
    ''' 1. **Data Grouping:** The data range is divided into a series of intervals, also known as bins. The choice of the number of bins can affect the shape of the histogram, so it's important to choose a number that accurately represents the data.
    ''' 2. **Counting Frequencies:** For each bin, the number of data points that fall into the interval is counted. This is the frequency of the bin.
    ''' 3. **Plotting:** The frequencies are plotted on the vertical axis, and the bin intervals are plotted on the horizontal axis. Each bar's height corresponds to the frequency of data points within the interval that the bar represents.
    ''' 
    ''' ### Components of a Histogram:
    ''' - **BARS:** Each bar represents the frequency of data points within a particular interval. The taller the bar, the more data points fall into that interval.
    ''' - **X-AXIS:** This axis represents the variable being measured and the intervals (bins) into which the data has been divided.
    ''' - **Y-AXIS:** This axis represents the frequency of occurrences for each bin. It can also represent the relative frequency or density if the histogram is normalized.
    ''' - **BIN WIDTH:** The width of the bins is constant and represents the range of values that are counted for each bar.
    ''' 
    ''' ### Types of Histograms:
    ''' - **Normal Histogram:** The bars are centered around the mean value, and the distribution may be symmetric if the data is normally distributed.
    ''' - **Cumulative Histogram:** Instead of showing the frequency for each bin, it shows the cumulative frequency up to each bin.
    ''' - **Density Histogram:** In a density histogram, the height of the bar does not represent the frequency but the density of the data within each bin. The total area of all the bars sums to 1.
    ''' 
    ''' ### Uses of Histograms:
    ''' - **Data Analysis:** To understand the distribution of the data, such as whether it is symmetric, skewed, bimodal, etc.
    ''' - **Quality Control:** In manufacturing, histograms can help determine if a process is within acceptable limits.
    ''' - **Statistical Analysis:** Histograms can help in choosing appropriate statistical tests or models for data analysis.
    ''' 
    ''' ### Limitations:
    ''' - The shape of a histogram can be sensitive to the number of bins chosen.
    ''' - Histograms do not show information about individual data points; they only show the frequency of data within intervals.
    ''' </summary>
    Public Class ggplotHistogram : Inherits ggplotLayer

        Public Property bins As Integer
        Public Property range As DoubleRange

        Dim binData As DataBinBox(Of Double)()

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim color As String = getColorName(stream.ggplot)
            Dim legend As New LegendObject With {
                .color = color,
                .fontstyle = stream.theme.legendLabelCSS,
                .style = LegendStyles.Rectangle,
                .title = stream.defaultTitle
            }
            Dim histData As HistProfile = binData.NewModel(legend)
            Dim colorData As New NamedValue(Of Color) With {
                .Name = legend.title,
                .Value = color.TranslateColor
            }
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment

            Call HistogramPlot.DrawSample(stream.g, stream.canvas.PlotRegion(css), histData, colorData, stream.scale)

            Return New ggplotLegendElement With {
                .legend = legend
            }
        End Function

        Private Function getColorName(ggplot As ggplot) As String
            If useCustomColorMaps Then
                Return DirectCast(colorMap, ggplotColorLiteral).ToString
            ElseIf ggplot.base.reader.color Is Nothing Then
                Return "black"
            Else
                Return ggplot.base.reader.color.ToString
            End If
        End Function

        Friend Shared Sub configHistogram(ggplot As ggplot, hist As ggplotHistogram)
            Dim data As ggplotData

            If hist.useCustomData Then
                data = hist.reader.getMapData(ggplot.data, ggplot.environment)
            Else
                data = ggplot.base.reader.getMapData(ggplot.data, ggplot.environment)
            End If

            Dim dataX As Double() = CLRVector.asNumeric(data.x)
            Dim bins = CutBins _
                .FixedWidthBins(
                    data:=dataX,
                    k:=hist.bins,
                    eval:=Function(xi) xi,
                    range:=hist.range
                ) _
                .ToArray
            Dim y As Double() = bins.Select(Function(d) CDbl(d.Count)).ToArray

            hist.binData = bins
            ggplot.base.data.Add("y", y)

            If ggplot.ylabel.StringEmpty Then
                ggplot.ylabel = "Count"
            End If
        End Sub
    End Class
End Namespace
