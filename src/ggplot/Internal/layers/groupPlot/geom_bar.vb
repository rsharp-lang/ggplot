Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Imaging.d3js.scale

Namespace layers

    Public Class geom_bar : Inherits ggplotGroup

        Public Property stat As String
        Public Property position As String

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As OrdinalScale) As IggplotLegendElement

        End Function
    End Class
End Namespace