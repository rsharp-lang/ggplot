Imports System.Drawing
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.visualize.Network

Namespace ggraph.render

    Public Class textRender : Inherits ggplotLayer

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim renderLabel As New LabelRendering(
                      labelColorAsNodeColor:=True,
                      iteration:=-1,
                      showLabelerProgress:=False,
                      defaultLabelColorValue:="black",
                      labelTextStrokeCSS:=Nothing,
                      getLabelColor:=Function() Color.Black)

            Call renderLabel.drawLabels(stream.g, DirectCast(stream, graphPipeline).labels)

            Return Nothing
        End Function
    End Class
End Namespace