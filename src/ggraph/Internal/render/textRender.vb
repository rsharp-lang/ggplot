Imports System.Drawing
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Styling.Numeric

Namespace ggraph.render

    Public Class textRender : Inherits ggplotLayer

        Public Property fontsize As IGetSize
        Public Property iteration As Integer = 30

        Private Function getFontSize(graph As NetworkGraph) As Func(Of Node, Single)
            If fontsize Is Nothing Then
                Return Function(any) 45.0!
            Else
                Dim map As Dictionary(Of String, Single) = fontsize _
                    .GetSize(graph.vertex) _
                    .ToDictionary(Function(n) n.Key.label,
                                  Function(n)
                                      Return n.Maps
                                  End Function)

                Return Function(n)
                           If n Is Nothing Then
                               Return 45.0!
                           Else
                               Return map.TryGetValue(n.label, [default]:=45.0!)
                           End If
                       End Function
            End If
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim renderLabel As New LabelRendering(
                labelColorAsNodeColor:=True,
                iteration:=iteration,
                showLabelerProgress:=False,
                defaultLabelColorValue:="black",
                labelTextStrokeCSS:=Nothing,
                getLabelColor:=Function() Color.Black
            )
            Dim allLabels As New List(Of LayoutLabel)
            Dim fontsize As Func(Of Node, Single) = getFontSize(stream.ggplot.data)

            For Each label As LayoutLabel In DirectCast(stream, graphPipeline).labels.Where(Function(lb) lb.hasGDIData)
                label.style = New Font(
                    family:=label.style.FontFamily,
                    emSize:=fontsize(label.node),
                    style:=label.style.Style
                )

                With stream.g.MeasureString(label.label.text, label.style)
                    label.label.width = .Width
                    label.label.height = .Height
                End With

                Call allLabels.Add(label)
            Next

            Call renderLabel.renderLabels(
                g:=stream.g,
                labelList:=allLabels
            )

            Return Nothing
        End Function
    End Class
End Namespace