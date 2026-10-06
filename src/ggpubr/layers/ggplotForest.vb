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
''' The forest plot layer, which summarizes the effect sizes of a meta
''' analysis: every row is one study, the left part draws the point estimate
''' and its confidence interval, and the right part prints the numeric values.
''' </summary>
Public Class ggplotForest : Inherits ggplotGroup

    Public Property effect As Double()
    Public Property lower As Double()
    Public Property upper As Double()
    Public Property weight As Double()
    Public Property label As String()
    Public Property reference As Double = 0
    Public Property logScale As Boolean = False
    Public Property rowHeight As Double = 18
    Public Property pointSize As Double = 4

    Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
        If effect Is Nothing OrElse effect.Length = 0 Then Return Nothing

        Dim css As CSSEnvirnment = stream.g.LoadEnvironment
        Dim pen As Pen = css.GetPen(Stroke.TryParse(stream.theme.lineStroke), allowNull:=False)
        Dim font As Font = css.GetFont(CSSFont.TryParse(stream.theme.tagCSS))
        Dim region As Rectangle = stream.canvas.PlotRegion(css)
        Dim lo As Double = If(logScale, lower.Where(Function(v) v > 0).Min(), lower.Min())
        Dim hi As Double = If(logScale, upper.Max(), upper.Max())
        Dim n As Integer = effect.Length
        Dim line As Double = region.Bottom

        For i As Integer = 0 To n - 1
            Dim y As Double = region.Top + rowHeight * (i + 0.5)

            If y > region.Bottom Then Exit For

            Dim x0 As Double = scaleX(stream, lo, hi, reference, lower(i))
            Dim x1 As Double = scaleX(stream, lo, hi, reference, upper(i))
            Dim xm As Double = scaleX(stream, lo, hi, reference, effect(i))

            Call stream.g.DrawLine(pen, New PointF(CSng(x0), CSng(y)), New PointF(CSng(x1), CSng(y)))
            Call stream.g.DrawLine(pen, New PointF(CSng(x1), CSng(y - 4)), New PointF(CSng(x1), CSng(y + 4)))
            Call stream.g.FillEllipse(Brushes.SteelBlue, CSng(xm - pointSize), CSng(y - pointSize), CSng(pointSize * 2), CSng(pointSize * 2))

            If Not label Is Nothing AndAlso i < label.Length Then
                Dim text As String = $"{label(i)}  {effect(i).ToString("F2")} [{lower(i).ToString("F2")}, {upper(i).ToString("F2")}]"
                Dim size As SizeF = stream.g.MeasureString(text, font)

                Call stream.g.DrawString(text, font, Brushes.Black,
                    New PointF(CSng(region.Right - size.Width - 4), CSng(y - size.Height / 2)))
            End If
        Next

        If logScale Then
            Call stream.g.DrawLine(pen,
                New PointF(CSng(scaleX(stream, lo, hi, reference, reference)), CSng(region.Top)),
                New PointF(CSng(scaleX(stream, lo, hi, reference, reference)), CSng(region.Bottom)))
        End If

        Return Nothing
    End Function

    Private Function scaleX(stream As ggplotPipeline, lo As Double, hi As Double, reference As Double, value As Double) As Double
        Dim left As Single = stream.scale.X.rangeMin
        Dim right As Single = stream.scale.X.rangeMax

        If logScale Then
            Dim a As Double = If(reference <= 0, lo, reference)
            Dim lb As Double = Math.Log(If(lo <= 0, a / 1000, lo))
            Dim ub As Double = Math.Log(If(hi <= 0, a * 1000, hi))
            Dim v As Double = Math.Log(If(value <= 0, lb, value))

            Return left + (right - left) * (v - lb) / (ub - lb)
        End If

        Dim span As Double = hi - lo

        If span <= 0 Then Return left

        Return left + (right - left) * (value - lo) / span
    End Function

    Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As d3js.scale.OrdinalScale) As IggplotLegendElement
        Return Plot(stream)
    End Function
End Class

''' <summary>
''' The funnel plot layer, which draws the successive inclusion of the studies
''' or the subjects as a funnel, optionally with the 95% confidence interval
''' band of the proportion.
''' </summary>
Public Class ggplotFunnel : Inherits ggplotGroup

    Public Property proportion As Double()
    Public Property label As String()
    Public Property confLevel As Double = 0.95
    Public Property showBand As Boolean = True

    Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
        If proportion Is Nothing OrElse proportion.Length = 0 Then Return Nothing

        Dim css As CSSEnvirnment = stream.g.LoadEnvironment
        Dim pen As Pen = css.GetPen(Stroke.TryParse(stream.theme.lineStroke), allowNull:=False)
        Dim font As Font = css.GetFont(CSSFont.TryParse(stream.theme.tagCSS))
        Dim region As Rectangle = stream.canvas.PlotRegion(css)
        Dim n As Integer = proportion.Length
        Dim left As Single = stream.scale.X.rangeMin
        Dim right As Single = stream.scale.X.rangeMax
        Dim cx As Double = (left + right) / 2
        Dim half As Double = (right - left) / 2
        Dim total As Double = proportion.Max()

        For i As Integer = 0 To n - 1
            Dim ratio As Double = If(total <= 0, 0, proportion(i) / total)
            Dim yTop As Double = region.Top + (region.Height * i / n)
            Dim yBottom As Double = region.Top + (region.Height * (i + 1) / n)
            Dim w As Double = half * ratio
            Dim bar As New RectangleF(CSng(cx - w), CSng(yTop), CSng(w * 2), CSng(yBottom - yTop))

            Call stream.g.FillRectangle(New SolidBrush(Color.FromArgb(160, Color.SteelBlue)), bar)
            Call stream.g.DrawRectangle(pen, bar.X, bar.Y, bar.Width, bar.Height)

            If showBand AndAlso i < n - 1 Then
                Dim upper As Double = 1.96 * Math.Sqrt(ratio * (1 - ratio) / Math.Max(n - i, 1))
                Dim band As New RectangleF(CSng(cx - half * Math.Min(ratio + upper, 1)), CSng(yTop), CSng(half * 2 * Math.Min(ratio + upper, 1)), CSng(1))

                Call stream.g.DrawLine(pen, New PointF(CSng(band.X), CSng(band.Y)), New PointF(CSng(band.Right), CSng(band.Y)))
            End If

            If Not label Is Nothing AndAlso i < label.Length Then
                Dim size As SizeF = stream.g.MeasureString(label(i), font)

                Call stream.g.DrawString(label(i), font, Brushes.Black,
                    New PointF(CSng(cx - size.Width / 2), CSng(yTop + (yBottom - yTop - size.Height) / 2)))
            End If
        Next

        Return Nothing
    End Function

    Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As d3js.scale.OrdinalScale) As IggplotLegendElement
        Return Plot(stream)
    End Function
End Class
