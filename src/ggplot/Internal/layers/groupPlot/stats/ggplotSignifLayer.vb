Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports SMRUCC.Rsharp.Runtime.Internal.Object

Namespace layers

    Public Class ggplotSignifLayer : Inherits ggplotStatsLayer

        Public Property comparision As list
        Public Property method As String = "t.test"

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As OrdinalScale) As IggplotLegendElement
            Select Case method
                Case "t.test"
                Case "wilcox.test"
                    Throw New NotImplementedException("wilcox.test")
                Case Else
                    Throw New NotImplementedException(method)
            End Select
        End Function
    End Class
End Namespace