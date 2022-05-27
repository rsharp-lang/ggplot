Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Imaging

Namespace layers

    Public Class ggplotJitter : Inherits ggplotLayer

        Public Property width_jit As Double = 0.5

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            If stream.scale.xscale = d3js.scale.scalers.linear Then
                Throw New NotImplementedException
            Else
                Return PlotOrdinal(stream)
            End If
        End Function

        Private Function PlotOrdinal(stream As ggplotPipeline) As IggplotLegendElement
            Dim data As New Dictionary(Of String, List(Of Double))
            Dim tags As String() = stream.x
            Dim y As Double() = stream.y
            Dim x As Double()
            Dim g As IGraphics = stream.g

            For i As Integer = 0 To tags.Length - 1
                If Not data.ContainsKey(tags(i)) Then
                    Call data.Add(tags(i), New List(Of Double))
                End If

                Call data(tags(i)).Add(y(i))
            Next

            For Each tag As String In data.Keys
                y = data(tag).Select(AddressOf stream.scale.TranslateY).ToArray
                x = stream.scale.TranslateX(tag).Replicate(y.Length).ToArray
                x = Scatter.Jitter(x, width_jit)

                For i As Integer = 0 To x.Length - 1
                    Call g.DrawCircle(New PointF(x(i), y(i)), 5, Brushes.Red)
                Next
            Next

            Return Nothing
        End Function
    End Class
End Namespace