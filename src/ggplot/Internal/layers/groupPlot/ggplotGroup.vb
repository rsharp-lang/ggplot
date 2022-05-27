Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Imaging

Namespace layers

    Public MustInherit Class ggplotGroup : Inherits ggplotLayer

        Protected Iterator Function getDataGroups(stream As ggplotPipeline) As IEnumerable(Of NamedCollection(Of Double))
            Throw New NotImplementedException
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