Imports System.Drawing
Imports ggplot
Imports ggplot.layers
Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Plots.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render

''' <summary>
''' The scatter matrix(pairs) layer, which draws a grid of the scatter plots of
''' every variable pair, with the variable names on the diagonal.
''' </summary>
Public Class ggplotPairs : Inherits ggplotGroup

    Public Property data As Double(,)
    Public Property labels As String()
    Public Property upper As Boolean = False
    Public Property pointSize As Single = 2
    Public Property maxVariables As Integer = 6

    Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
        If data Is Nothing Then Return Nothing

        Dim vars As Integer = Math.Min(data.GetLength(0), maxVariables)
        Dim css As CSSEnvirnment = stream.g.LoadEnvironment
        Dim font As Font = css.GetFont(CSSFont.TryParse(stream.theme.tagCSS))
        Dim region As Rectangle = stream.canvas.PlotRegion(css)
        Dim cell As Double = Math.Min(region.Width, region.Height) / vars

        For i As Integer = 0 To vars - 1
            For j As Integer = 0 To vars - 1
                If upper AndAlso j < i Then Continue For

                Dim x As Single = CSng(region.Left + j * cell)
                Dim y As Single = CSng(region.Top + i * cell)
                Dim pen As Pen = css.GetPen(Stroke.TryParse(stream.theme.lineStroke), allowNull:=False)

                Call stream.g.DrawRectangle(pen, x, y, CSng(cell), CSng(cell))

                If i = j Then
                    Call drawName(stream, font, labels(i), x, y, cell)
                Else
                    Call drawScatter(stream, column(data, j), column(data, i), x, y, cell)
                End If
            Next
        Next

        Return Nothing
    End Function

    Private Sub drawScatter(stream As ggplotPipeline, x As Double(), y As Double(), left As Single, top As Single, cell As Double)
        Dim n As Integer = Math.Min(x.Length, y.Length)

        If n = 0 Then Return

        Dim xmin As Double = x.Min()
        Dim xmax As Double = x.Max()
        Dim ymin As Double = y.Min()
        Dim ymax As Double = y.Max()
        Dim radius As Single = Math.Max(1, Me.pointSize)

        For i As Integer = 0 To n - 1
            Dim px As Single = CSng(left + cell * 0.05 + cell * 0.9 * scale(x(i), xmin, xmax))
            Dim py As Single = CSng(top + cell * 0.95 - cell * 0.9 * scale(y(i), ymin, ymax))

            Call stream.g.FillEllipse(Brushes.SteelBlue, px - radius, py - radius, radius * 2, radius * 2)
        Next
    End Sub

    Private Shared Sub drawName(stream As ggplotPipeline, font As Font, label As String, left As Single, top As Single, cell As Double)
        If label Is Nothing Then Return

        Dim size As SizeF = stream.g.MeasureString(label, font)

        Call stream.g.DrawString(label, font, Brushes.Black,
            New PointF(CSng(left + cell / 2 - size.Width / 2), CSng(top + cell / 2 - size.Height / 2)))
    End Sub

    Private Shared Function scale(v As Double, lo As Double, hi As Double) As Double
        If hi <= lo Then Return 0.5

        Return (v - lo) / (hi - lo)
    End Function

    Private Shared Function column(data As Double(,), index As Integer) As Double()
        Dim buffer As Double() = New Double(data.GetLength(1) - 1) {}

        For i As Integer = 0 To buffer.Length - 1
            buffer(i) = data(index, i)
        Next

        Return buffer
    End Function

    Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As d3js.scale.OrdinalScale) As IggplotLegendElement
        Return Plot(stream)
    End Function
End Class
