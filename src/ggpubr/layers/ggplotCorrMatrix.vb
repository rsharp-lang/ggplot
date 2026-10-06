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
''' The correlation matrix layer, which draws the pairwise correlation
''' coefficients as a coloured square matrix, so that the structure of a
''' multivariable dataset becomes visible at a glance.
''' </summary>
Public Class ggplotCorrMatrix : Inherits ggplotGroup

    Public Property data As Double(,)
    Public Property method As String = "pearson"
    Public Property showLabel As Boolean = True
    Public Property upper As Boolean = False

    Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
        If data Is Nothing Then Return Nothing

        Dim n As Integer = data.GetLength(0)

        If n = 0 OrElse data.GetLength(1) <> n Then Return Nothing

        Dim r As Double(,) = correlate(data)
        Dim css As CSSEnvirnment = stream.g.LoadEnvironment
        Dim font As Font = css.GetFont(CSSFont.TryParse(stream.theme.tagCSS))
        Dim region As Rectangle = stream.canvas.PlotRegion(css)
        Dim cell As Double = Math.Min(region.Width, region.Height) / n

        For i As Integer = 0 To n - 1
            For j As Integer = 0 To n - 1
                If upper AndAlso j < i Then Continue For

                Dim v As Double = Math.Max(-1, Math.Min(1, r(i, j)))
                Dim shade As Integer = CSng(Math.Abs(v) * 255)
                Dim fill As Color = If(v >= 0, Color.FromArgb(shade, 60, 60), Color.FromArgb(shade, 60, 60, 200))
                Dim x As Single = CSng(region.Left + j * cell)
                Dim y As Single = CSng(region.Top + i * cell)

                Call stream.g.FillRectangle(New SolidBrush(fill), x, y, CSng(cell), CSng(cell))

                If showLabel AndAlso i <> j Then
                    Dim text As String = v.ToString("F2")
                    Dim size As SizeF = stream.g.MeasureString(text, font)
                    Dim brush As Brush = If(Math.Abs(v) > 0.6, Brushes.White, Brushes.Black)

                    Call stream.g.DrawString(text, font, brush,
                        New PointF(CSng(x + cell / 2 - size.Width / 2), CSng(y + cell / 2 - size.Height / 2)))
                End If
            Next
        Next

        Return Nothing
    End Function

    Private Function correlate(data As Double(,)) As Double(,)
        Dim n As Integer = data.GetLength(0)
        Dim result As Double(,) = New Double(n - 1, n - 1) {}

        For i As Integer = 0 To n - 1
            Call result.SetValue(1.0, i, i)

            For j As Integer = i + 1 To n - 1
                Dim v As Double = pearson(column(data, i), column(data, j))

                Call result.SetValue(v, i, j)
                Call result.SetValue(v, j, i)
            Next
        Next

        Return result
    End Function

    Private Shared Function column(data As Double(,), index As Integer) As Double()
        Dim buffer As Double() = New Double(data.GetLength(1) - 1) {}

        For i As Integer = 0 To buffer.Length - 1
            buffer(i) = data(index, i)
        Next

        Return buffer
    End Function

    Private Shared Function pearson(a As Double(), b As Double()) As Double
        Dim n As Integer = Math.Min(a.Length, b.Length)

        If n < 2 Then Return 0

        Dim ma As Double = 0
        Dim mb As Double = 0

        For i As Integer = 0 To n - 1
            ma += a(i)
            mb += b(i)
        Next

        ma /= n
        mb /= n

        Dim cov As Double = 0
        Dim va As Double = 0
        Dim vb As Double = 0

        For i As Integer = 0 To n - 1
            Dim da As Double = a(i) - ma
            Dim db As Double = b(i) - mb

            cov += da * db
            va += da * da
            vb += db * db
        Next

        If va <= 0 OrElse vb <= 0 Then Return 0

        Return cov / Math.Sqrt(va * vb)
    End Function

    Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As d3js.scale.OrdinalScale) As IggplotLegendElement
        Return Plot(stream)
    End Function
End Class
