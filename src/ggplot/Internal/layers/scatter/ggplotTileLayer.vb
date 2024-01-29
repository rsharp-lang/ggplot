Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    Public Class ggplotTileLayer : Inherits ggplotLayer

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim x As New DoubleRange(CLRVector.asNumeric(stream.x))
            Dim y As New DoubleRange(CLRVector.asNumeric(stream.y))


        End Function
    End Class
End Namespace