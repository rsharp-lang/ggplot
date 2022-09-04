Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports SMRUCC.Rsharp.Runtime.Internal.Object

Namespace layers

    Public Class ggplotScatterpie : Inherits ggplotLayer

        Public Property pie As String()

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim data As dataframe = stream.ggplot.data
            Dim x As Double() = stream.scale.X(stream.x)
            Dim y As Double() = stream.scale.Y(stream.y)


            Throw New NotImplementedException()
        End Function
    End Class
End Namespace