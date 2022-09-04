Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Imaging.d3js.scale

Namespace layers

    Public Class ggplotScatterpie : Inherits ggplotGroup

        Public Property pie As String()

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As OrdinalScale) As IggplotLegendElement

        End Function
    End Class
End Namespace