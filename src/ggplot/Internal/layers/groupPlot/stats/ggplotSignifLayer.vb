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