#Region "Microsoft.VisualBasic::a8db8b88de1a3eb88935af9a04da249e, src\ggplot\Internal\layers\groupPlot\stats\ggplotSignifLayer.vb"

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

    '   Total Lines: 54
    '    Code Lines: 41 (75.93%)
    ' Comment Lines: 5 (9.26%)
    '    - Xml Docs: 80.00%
    ' 
    '   Blank Lines: 8 (14.81%)
    '     File Size: 2.18 KB


    '     Class ggplotSignifLayer
    ' 
    '         Properties: comparision, method
    ' 
    '         Function: PlotOrdinal, ttest
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Logging
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.Statistics.Hypothesis
Imports Microsoft.VisualBasic.Math.Statistics.Hypothesis.ANOVA
Imports SMRUCC.Rsharp.Runtime.Internal.Object

Namespace layers

    Public Class ggplotSignifLayer : Inherits ggplotStatsLayer

        ''' <summary>
        ''' a tuple list of the comparision group labels
        ''' </summary>
        ''' <returns></returns>
        Public Property comparision As list
        Public Property method As String = "t.test"

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As OrdinalScale) As IggplotLegendElement
            Select Case method.ToLower
                Case "t.test", "t.test(var.equal=TRUE)"
                    stats = ttest(stream, paired:=False).ToArray
                Case "wilcox.test", "wilcox"
                    stats = wilcoxTest(stream).ToArray
                Case "anova"
                    stats = runAnovaTest(stream).ToArray
                Case "paired.t.test"
                    stats = ttest(stream, paired:=True).ToArray
                Case Else
                    Call stream.ggplot.environment.AddMessage($"the group comparision method '{method}' is not supported, the t.test will be used instead!", MSG_TYPES.WRN)
                    stats = ttest(stream, paired:=False).ToArray
            End Select

            Return MyBase.PlotOrdinal(stream, x)
        End Function

        Private Iterator Function ttest(stream As ggplotPipeline, paired As Boolean) As IEnumerable(Of compare_means)
            Dim data = getDataGroups(stream) _
                .ToDictionary(Function(v) v.name,
                              Function(v)
                                  Return v.value
                              End Function)

            For Each groupKey As String In comparision.getNames
                ' get group labels for extract sample vector for run t.test
                Dim two As String() = comparision.getValue(Of String())(groupKey, stream.ggplot.environment)
                Dim group1 = data.TryGetValue(two(0))
                Dim group2 = data.TryGetValue(two(1))
                Dim pvalue As Double

                If paired Then
                    If group1.Length <> group2.Length Then
                        Throw New InvalidDataException($"the paired t.test requires the two groups have the same sample size, but got {group1.Length} and {group2.Length}!")
                    End If

                    ' 配对样本t检验等价于对差值做单样本零假设检验
                    Dim delta As Double() = group1.Zip(group2, Function(a, b) a - b).ToArray

                    pvalue = t.Test(delta, mu:=0).Pvalue
                Else
                    pvalue = t.Test(group1, group2, varEqual:=True).Pvalue
                End If

                Yield New compare_means With {
                    .group1 = two(0),
                    .group2 = two(1),
                    .padj = pvalue,
                    .pvalue = pvalue,
                    .y = ""
                }
            Next
        End Function

        ''' <summary>
        ''' the Wilcoxon rank sum test (a.k.a the Mann-Whitney U test) which
        ''' does Not assume the normality of the group data
        ''' </summary>
        ''' <param name="stream"></param>
        ''' <returns></returns>
        Private Iterator Function wilcoxTest(stream As ggplotPipeline) As IEnumerable(Of compare_means)
            Dim data = getDataGroups(stream) _
                .ToDictionary(Function(v) v.name,
                              Function(v)
                                  Return v.value
                              End Function)

            For Each groupKey As String In comparision.getNames
                Dim two As String() = comparision.getValue(Of String())(groupKey, stream.ggplot.environment)
                Dim group1 = data.TryGetValue(two(0))
                Dim group2 = data.TryGetValue(two(1))
                Dim pvalue As Double = mannWhitney(group1, group2)

                Yield New compare_means With {
                    .group1 = two(0),
                    .group2 = two(1),
                    .padj = pvalue,
                    .pvalue = pvalue,
                    .y = ""
                }
            Next
        End Function

        ''' <summary>
        ''' the one-way ANOVA test across all the comparison groups
        ''' </summary>
        ''' <param name="stream"></param>
        ''' <returns></returns>
        Private Iterator Function runAnovaTest(stream As ggplotPipeline) As IEnumerable(Of compare_means)
            Dim groups As String() = comparision.getNames() _
                .Select(Function(groupKey)
                            Return comparision.getValue(Of String())(groupKey, stream.ggplot.environment)
                        End Function) _
                .FirstOrDefault()

            If groups.IsNullOrEmpty Then
                Throw New InvalidDataException("the ANOVA test requires at least one comparison group!")
            End If

            Dim pvalue As Double = anovaPvalue(stream)

            Yield New compare_means With {
                .group1 = groups(Scan0),
                .group2 = groups(groups.Length - 1),
                .padj = pvalue,
                .pvalue = pvalue,
                .y = ""
            }
        End Function

        ''' <summary>
        ''' the one-way ANOVA pvalue across all the comparison groups
        ''' </summary>
        ''' <param name="stream"></param>
        ''' <returns></returns>
        Private Function anovaPvalue(stream As ggplotPipeline) As Double
            Dim data = getDataGroups(stream) _
                .ToDictionary(Function(v) v.name,
                              Function(v)
                                  Return v.value
                              End Function)
            Dim samples As New List(Of Double())
            Dim names As New List(Of String)

            For Each groupKey As String In comparision.getNames
                Dim two As String() = comparision.getValue(Of String())(groupKey, stream.ggplot.environment)

                For Each label As String In two
                    If Not names.Contains(label) AndAlso data.ContainsKey(label) Then
                        Call names.Add(label)
                        Call samples.Add(data(label))
                    End If
                Next
            Next

            If samples.Count < 2 Then
                Throw New InvalidDataException("the ANOVA test requires at least two comparison groups!")
            End If

            Dim anova As New AnovaTest()

            anova.populate(samples.ToArray, type:=AnovaTest.P_FIVE_PERCENT)
            anova.findWithinGroupMeans()
            anova.setSumOfSquaresOfGroups()
            anova.setTotalSumOfSquares()
            anova.divide_by_degrees_of_freedom()

            Return anova.singlePvalue
        End Function

        ''' <summary>
        ''' the two sided pvalue of the Mann-Whitney U rank sum test
        ''' </summary>
        ''' <param name="x"></param>
        ''' <param name="y"></param>
        ''' <returns>
        ''' the normal approximated pvalue, using the continuity correction
        ''' </returns>
        Public Shared Function mannWhitney(x As Double(), y As Double()) As Double
            Dim n1 As Integer = x.Length
            Dim n2 As Integer = y.Length

            If n1 = 0 OrElse n2 = 0 Then
                Return 1
            End If

            Dim total As Integer = n1 + n2
            Dim values As Double() = New Double(total - 1) {}
            Dim fromGroup1 As Boolean() = New Boolean(total - 1) {}

            For i As Integer = 0 To n1 - 1
                values(i) = x(i)
                fromGroup1(i) = True
            Next
            For i As Integer = 0 To n2 - 1
                values(n1 + i) = y(i)
                fromGroup1(n1 + i) = False
            Next

            Dim order As Integer() = Enumerable.Range(0, total) _
                .OrderBy(Function(i) values(i)) _
                .ToArray
            Dim ranks As Double() = New Double(total - 1) {}
            Dim rank As Double = 1

            For i As Integer = 0 To total - 1
                Dim j As Integer = i

                Do While j + 1 < total AndAlso values(order(j + 1)) = values(order(i))
                    j += 1
                Loop

                ' 相同取值取平均秩
                Dim mid As Double = rank + (j - i) / 2

                For k As Integer = i To j
                    ranks(order(k)) = mid
                Next

                rank += (j - i) + 1
                i = j
            Next

            Dim r1 As Double = 0

            For i As Integer = 0 To n1 - 1
                r1 += ranks(i)
            Next

            Dim u1 As Double = r1 - n1 * (n1 + 1) / 2
            Dim u As Double = Math.Min(u1, CDbl(n1) * n2 - u1)
            Dim mu As Double = CDbl(n1) * n2 / 2
            Dim sigma As Double = Math.Sqrt(CDbl(n1) * n2 * (n1 + n2 + 1) / 12)

            If sigma <= 0 Then
                Return 1
            End If

            Dim z As Double = (u - mu + 0.5) / sigma
            Dim p As Double = 2 * (1 - normCDF(z))

            Return Math.Max(0, Math.Min(1, p))
        End Function

        ''' <summary>
        ''' the cumulative distribution function of the standard normal distribution
        ''' </summary>
        ''' <param name="z"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Abramowitz &amp; Stegun 26.2.17 的误差函数有理逼近，
        ''' 绝对误差小于 1.5e-7，对统计检验的p值而言完全足够
        ''' </remarks>
        Private Shared Function normCDF(z As Double) As Double
            If z > 8 Then Return 1
            If z < -8 Then Return 0

            Dim az As Double = Math.Abs(z)
            Dim t As Double = 1 / (1 + 0.2316419 * az)
            Dim poly As Double = t * (0.319381530 + t * (-0.356563782 + t * (1.781477937 + t * (-1.821255978 + t * 1.330274429))))
            Dim phi As Double = 0.3989422804014327 * Math.Exp(-az * az / 2)
            Dim upper As Double = phi * poly

            If z > 0 Then
                Return 1 - upper
            Else
                Return upper
            End If
        End Function
    End Class
End Namespace
