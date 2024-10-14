#Region "Microsoft.VisualBasic::501f4388e3664a0f0201cf4eac5db45b, src\ggplot\Internal\layers\groupPlot\stats\ggplotStatPvalue.vb"

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

    '   Total Lines: 126
    '    Code Lines: 108 (85.71%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 18 (14.29%)
    '     File Size: 5.39 KB


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
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports SMRUCC.Rsharp.Runtime.Internal.Invokes

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
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim font As Font = css.GetFont(CSSFont.TryParse(stream.theme.tagCSS))
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

            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim pvalue As Double = anova.singlePvalue
            Dim ptag As String = If(pvalue.ToString = "0", "<1e-100", "=" & pvalue.ToString("G3"))
            Dim tagStr As String = $"ANOVA, p-value{ptag}"
            Dim font As Font = css.GetFont(CSSFont.TryParse(stream.theme.tagCSS))
            Dim pos As New PointF(stream.scale.X.rangeMin + 10, stream.canvas.Padding.Top)

            Call stream.g.DrawString(tagStr, font, Brushes.Black, pos)
        End Sub
    End Class
End Namespace
