Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Imaging.d3js.scale

Namespace layers

    Public Class ggplotPie : Inherits ggplotGroup

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As OrdinalScale) As IggplotLegendElement
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace