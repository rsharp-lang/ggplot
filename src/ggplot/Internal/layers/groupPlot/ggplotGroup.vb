Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Imaging

Namespace layers

    Public MustInherit Class ggplotGroup : Inherits ggplotLayer

        Public Property groupWidth As Double = 0.5

        Public Shared Iterator Function getDataGroups(tags As String(), y As Double()) As IEnumerable(Of NamedCollection(Of Double))
            Dim data As New Dictionary(Of String, List(Of Double))

            For i As Integer = 0 To tags.Length - 1
                If Not data.ContainsKey(tags(i)) Then
                    Call data.Add(tags(i), New List(Of Double))
                End If

                Call data(tags(i)).Add(y(i))
            Next

            For Each group In data
                Yield New NamedCollection(Of Double) With {
                    .name = group.Key,
                    .value = group.Value.ToArray
                }
            Next
        End Function

        Protected Function getDataGroups(stream As ggplotPipeline) As IEnumerable(Of NamedCollection(Of Double))
            Dim tags As String() = stream.x
            Dim y As Double() = stream.y

            Return getDataGroups(tags, y)
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            If stream.scale.xscale = d3js.scale.scalers.linear Then
                Throw New NotImplementedException
            Else
                Return PlotOrdinal(stream)
            End If
        End Function

        Protected MustOverride Function PlotOrdinal(stream As ggplotPipeline) As IggplotLegendElement

    End Class
End Namespace