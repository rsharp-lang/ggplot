Imports System.Drawing
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Shapes

Public Class ggplotABLine : Inherits ggplotLayer

    Public Property abline As Line

    Public Overrides Function Plot(g As IGraphics,
                                   canvas As GraphicsRegion,
                                   baseData As ggplotData,
                                   x() As Double,
                                   y() As Double,
                                   scale As DataScaler,
                                   ggplot As ggplot,
                                   theme As Theme) As IggplotLegendElement

        Dim a As PointF = constraint(abline.A, scale)
        Dim b As PointF = constraint(abline.B, scale)

        a = scale.Translate(a)
        b = scale.Translate(b)

        Call g.DrawLine(abline.Stroke, a, b)

        Return Nothing
    End Function

    Private Shared Function constraint(pf As PointF, scale As DataScaler) As PointF
        Dim x As Single = If(pf.X < scale.xmin, scale.xmin, pf.X)
        Dim y As Single = If(pf.Y < scale.ymin, scale.ymin, pf.Y)

        Return New PointF With {
            .X = If(x > scale.xmax, scale.xmax, x),
            .Y = If(y > scale.ymax, scale.ymax, y)
        }
    End Function
End Class
