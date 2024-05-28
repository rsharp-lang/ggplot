#Region "Microsoft.VisualBasic::4354ac27a3b9e8aff78877cae739b3d5, src\ggplot\Internal\layers\groupPlot\stats\ggplotStatPvalue.vb"

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

    '   Total Lines: 95
    '    Code Lines: 78 (82.11%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 17 (17.89%)
    '     File Size: 3.98 KB


    '     Class ggplotStatPvalue
    ' 
    '         Properties: hide_ns, method, ref_group
    ' 
    '         Function: PlotOrdinal
    ' 
    '         Sub: plotAnova, plotTtest
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.Statistics.Hypothesis
Imports Microsoft.VisualBasic.Math.Statistics.Hypothesis.ANOVA
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports SMRUCC.Rsharp.Runtime.Internal.Invokes

Namespace layers

    Public Class ggplotStatPvalue : Inherits ggplotGroup

        Public Property method As String = "anova"
        Public Property ref_group As String = ".all."
        Public Property hide_ns As Boolean = True

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As OrdinalScale) As IggplotLegendElement
            Select Case method.ToLower
                Case "anova" : Call plotAnova(stream, x)
                Case "t.test" : Call plotTtest(stream, x)
                Case Else
                    Throw New NotImplementedException(method)
            End Select

            Return Nothing
        End Function

        Private Sub plotTtest(stream As ggplotPipeline, xscale As OrdinalScale)
            Dim data = getDataGroups(stream).ToArray
            Dim ref As Double()
            Dim font As Font = CSSFont.TryParse(stream.theme.tagCSS).GDIObject(stream.g.Dpi)
            Dim lbsize As SizeF

            If ref_group = ".all." Then
                ref = data _
                    .Select(Function(v) v.value) _
                    .IteratesALL _
                    .ToArray
            Else
                ref = data _
                    .Where(Function(v) v.name = ref_group) _
                    .FirstOrDefault _
                    .value

                If ref Is Nothing Then
                    Throw New EntryPointNotFoundException($"missing the reference group data: '{ref_group}'!")
                End If
            End If

            For Each group As NamedCollection(Of Double) In data
                Dim p As TwoSampleResult = t.Test(group, ref, varEqual:=True)
                Dim pvalue As New compare_means With {.pvalue = p.Pvalue}
                Dim sig As String = pvalue.psignif

                If hide_ns AndAlso (sig = compare_means.ns OrElse sig = ".") Then
                    Continue For
                Else
                    lbsize = stream.g.MeasureString(sig, font)
                End If

                Dim x As Double = xscale(group.name) - lbsize.Width / 2
                Dim y As Double = getLabelPosY(group, stream.scale) - lbsize.Height * 1.125

                Call stream.g.DrawString(sig, font, Brushes.Black, New PointF(x, y))
            Next
        End Sub

        Private Sub plotAnova(stream As ggplotPipeline, x As OrdinalScale)
            Dim data = getDataGroups(stream).ToArray
            Dim observations As Double()() = data _
                .Select(Function(v) v.value) _
                .ToArray
            Dim anova As New AnovaTest()

            anova.populate(observations, type:=AnovaTest.P_FIVE_PERCENT)
            anova.findWithinGroupMeans()
            anova.setSumOfSquaresOfGroups()
            anova.setTotalSumOfSquares()
            anova.divide_by_degrees_of_freedom()

            Call base.cat("\n", env:=stream.ggplot.environment)
            Call base.cat(anova.ToString, env:=stream.ggplot.environment)

            Dim pvalue As Double = anova.singlePvalue
            Dim ptag As String = If(pvalue.ToString = "0", "<1e-100", "=" & pvalue.ToString("G3"))
            Dim tagStr As String = $"ANOVA, p-value{ptag}"
            Dim font As Font = CSSFont.TryParse(stream.theme.tagCSS).GDIObject(stream.g.Dpi)
            Dim pos As New PointF(stream.scale.X.rangeMin + 10, stream.canvas.Padding.Top)

            Call stream.g.DrawString(tagStr, font, Brushes.Black, pos)
        End Sub
    End Class
End Namespace
