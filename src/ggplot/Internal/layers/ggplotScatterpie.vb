Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Imaging.d3js.scale

Namespace layers

    Public Class ggplotScatterpie : Inherits ggplotLayer

        Public Property pie As String()

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace