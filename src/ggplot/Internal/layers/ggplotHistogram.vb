#Region "Microsoft.VisualBasic::e8d7f72998928a2e9acadec57a9efd9d, ggplot\src\ggplot\Internal\layers\ggplotHistogram.vb"

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

    '   Total Lines: 73
    '    Code Lines: 60
    ' Comment Lines: 0
    '   Blank Lines: 13
    '     File Size: 2.74 KB


    '     Class ggplotHistogram
    ' 
    '         Properties: bins
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
Imports REnv = SMRUCC.Rsharp.Runtime

Namespace layers

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

            Call HistogramPlot.DrawSample(stream.g, stream.canvas.PlotRegion, histData, colorData, stream.scale)

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

            Dim dataX As Double() = REnv.asVector(Of Double)(data.x)
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
