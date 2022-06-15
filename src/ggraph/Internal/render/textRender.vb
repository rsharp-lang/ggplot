Imports System.Drawing
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Styling.FillBrushes
Imports Microsoft.VisualBasic.Data.visualize.Network.Styling.Numeric
Imports Microsoft.VisualBasic.Imaging

Namespace ggraph.render

    Public Class textRender : Inherits ggplotLayer

        Public Property fontsize As IGetSize
        Public Property iteration As Integer = 30
        Public Property color As IGetBrush
        Public Property defaultSize As Single = 45.0!

        Private Function getFontSize(graph As NetworkGraph, dpi As Integer) As Func(Of Node, Single)
            If fontsize Is Nothing Then
                Return Function(any) FontFace.PointSizeScale(defaultSize, dpi)
            Else
                Dim map As Dictionary(Of String, Single) = fontsize _
                    .GetSize(graph.vertex) _
                    .ToDictionary(Function(n) n.Key.label,
                                  Function(n)
                                      Return n.Maps
                                  End Function)

                Return Function(n)
                           If n Is Nothing Then
                               Return FontFace.PointSizeScale(defaultSize, dpi)
                           Else
                               Return FontFace.PointSizeScale(map.TryGetValue(n.label, [default]:=45.0!), dpi)
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
                getLabelColor:=Function() Drawing.Color.Black
            )
            Dim allLabels As New List(Of LayoutLabel)
            Dim fontsize As Func(Of Node, Single) = getFontSize(stream.ggplot.data, stream.g.Dpi)

            For Each label As LayoutLabel In DirectCast(stream, graphPipeline).labels.Where(Function(lb) lb.hasGDIData)
                label.style = New Font(
                    family:=label.style.FontFamily,
                    emSize:=fontsize(label.node),
                    style:=label.style.Style
                )

                With stream.g.MeasureString(label.label.text, label.style)
                    label.label.width = .Width * 2
                    label.label.height = .Height * 2
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