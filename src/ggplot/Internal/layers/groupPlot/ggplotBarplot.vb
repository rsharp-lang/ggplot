Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Imaging

Namespace layers

    Public Class ggplotBarplot : Inherits ggplotGroup

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As d3js.scale.OrdinalScale) As IggplotLegendElement
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace