Imports ggplot
Imports ggplot.elements.legend
Imports ggplot.layers

Public Class ggplotConfidenceEllipse : Inherits ggplotLayer

    Public Property level As Double = 0.95

    Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement

    End Function
End Class
