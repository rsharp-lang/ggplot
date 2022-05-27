Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Namespace layers

    Public MustInherit Class ggplotGroup : Inherits ggplotLayer

        Protected Iterator Function getDataGroups(stream As ggplotPipeline) As IEnumerable(Of NamedCollection(Of Double))
            Throw New NotImplementedException
        End Function

    End Class
End Namespace