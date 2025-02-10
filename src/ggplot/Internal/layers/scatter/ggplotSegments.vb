Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    Public Class ggplotSegments : Inherits ggplotScatterLayer

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim xend As Single(), yend As Single()
            Dim x As Single() = CLRVector.asFloat(stream.x)
            Dim y As Single() = CLRVector.asFloat(stream.y)

            If useCustomData Then
                xend = data.x.ToFloat
                yend = data.y.ToFloat
            Else
                xend = CLRVector.asFloat(stream.x)
                yend = CLRVector.asFloat(stream.y)
            End If

            For i As Integer = 0 To x.Length - 1
                Call RenderShape.RenderBoid(stream.g, x(i), y(i), xend(i), yend(i), Color.Black, l:=minCell)
            Next

            If showLegend Then
                Return Nothing
            Else
                Return Nothing
            End If
        End Function
    End Class
End Namespace