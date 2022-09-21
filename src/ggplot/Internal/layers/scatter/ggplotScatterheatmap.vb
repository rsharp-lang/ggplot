Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime
Imports stdNum = System.Math

Namespace layers

    Public Class ggplotScatterheatmap : Inherits ggplotScatterLayer

        ''' <summary>
        ''' the dataframe feature name for get heatmap layer
        ''' </summary>
        ''' <returns></returns>
        Public Property layer As String
        Public Property maplevels As Integer = 120

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            ' get scatter data
            Dim data As dataframe = stream.ggplot.data
            Dim x As Double() = stream.scale.X(stream.x)
            Dim y As Double() = stream.scale.Y(stream.y)
            Dim layerdata = DirectCast(REnv.asVector(Of Double)(data.getVector(Me.layer, fullSize:=True)), Double())
            Dim cellSize = getCellsize(x, y)
            Dim colors As SolidBrush() = Designer _
                .GetColors(stream.theme.colorSet, n:=maplevels) _
                .Select(Function(c) New SolidBrush(c)) _
                .ToArray
            Dim radius As Single = stdNum.Min(cellSize.Width, cellSize.Height) / 2
            Dim valueRange As New DoubleRange(layerdata)
            Dim indexRange As New DoubleRange(0, maplevels - 1)
            Dim layerIndex As Integer() = layerdata _
                .Select(Function(xi) CInt(valueRange.ScaleMapping(xi, indexRange))) _
                .ToArray

            If radius < minCell Then
                radius = minCell
            End If

            For i As Integer = 0 To x.Length - 1
                Dim xi = x(i) - cellSize.Width / 2
                Dim yi = y(i) - cellSize.Height / 2
                Dim cell As New RectangleF(xi, yi, cellSize.Width, cellSize.Height)

                Call stream.g.FillRectangle(colors(layerIndex(i)), cell)
            Next

            Return New legendColorMapElement With {
                .colorMapLegend = New ColorMapLegend(stream.theme.colorSet, maplevels) With {
                    .title = Me.layer,
                    .tickAxisStroke = Stroke.TryParse(stream.theme.legendTickAxisStroke).GDIObject,
                    .tickFont = CSSFont.TryParse(stream.theme.legendTickCSS).GDIObject(stream.g.Dpi),
                    .format = stream.theme.legendTickFormat,
                    .ticks = layerdata.CreateAxisTicks,
                    .titleFont = CSSFont.TryParse(stream.theme.legendTitleCSS).GDIObject(stream.g.Dpi)
                },
                .width = stream.canvas.Padding.Right * 3 / 4,
                .height = stream.canvas.PlotRegion.Height
            }
        End Function
    End Class
End Namespace