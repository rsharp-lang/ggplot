#Region "Microsoft.VisualBasic::94f9b8fd173df7889def9207aca2c26e, ggplot\src\ggplot\Internal\layers\groupPlot\stats\ggplotSignifLayer.vb"

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

    '   Total Lines: 49
    '    Code Lines: 41
    ' Comment Lines: 0
    '   Blank Lines: 8
    '     File Size: 1.96 KB


    '     Class ggplotSignifLayer
    ' 
    '         Properties: comparision, method
    ' 
    '         Function: PlotOrdinal, ttest
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.Statistics.Hypothesis
Imports SMRUCC.Rsharp.Runtime.Internal.Object

Namespace layers

    Public Class ggplotSignifLayer : Inherits ggplotStatsLayer

        Public Property comparision As list
        Public Property method As String = "t.test"

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As OrdinalScale) As IggplotLegendElement
            Select Case method
                Case "t.test" : stats = ttest(stream).ToArray
                Case "wilcox.test"
                    Throw New NotImplementedException("wilcox.test")
                Case Else
                    Throw New NotImplementedException(method)
            End Select

            Return MyBase.PlotOrdinal(stream, x)
        End Function

        Private Iterator Function ttest(stream As ggplotPipeline) As IEnumerable(Of compare_means)
            Dim data = getDataGroups(stream) _
                .ToDictionary(Function(v) v.name,
                              Function(v)
                                  Return v.value
                              End Function)

            For Each groupKey As String In comparision.getNames
                Dim two As String() = comparision.getValue(Of String())(groupKey, stream.ggplot.environment)
                Dim group1 = data.TryGetValue(two(0))
                Dim group2 = data.TryGetValue(two(1))
                Dim test As TwoSampleResult = t.Test(group1, group2, varEqual:=True)

                Yield New compare_means With {
                    .group1 = two(0),
                    .group2 = two(1),
                    .padj = test.Pvalue,
                    .pvalue = test.Pvalue,
                    .y = ""
                }
            Next
        End Function
    End Class
End Namespace
