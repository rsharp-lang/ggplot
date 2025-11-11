Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.Plots
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports Microsoft.VisualBasic.Math.Interpolation

Namespace layers

    Public Class ggplotPrincipalCurve : Inherits ggplotLine

        Public Property bandwidth As Double = 1.0
        Public Property maxIterations As Integer = 100
        Public Property tolerance As Double = 0.001

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim legends As IggplotLegendElement = Nothing
            Dim serials As SerialData() = GetData(stream, legends)

            For Each serial As SerialData In serials
                Dim principalCurveData As Vector2D() = PrincipalCurve _
                    .Fit(serial.pts, bandwidth, maxIterations, tolerance) _
                    .ToArray
                Dim curve As SerialData = serial

                curve.pts = principalCurveData _
                    .Select(Function(a)
                                Return New PointData With {
                                    .pt = New PointF(a.x, a.y)
                                }
                            End Function) _
                    .ToArray

                LinePlot2D.DrawLine(
                    stream.g, stream.canvas, stream.scale,
                    serial,
                    interplot:=If(bspline, Splines.B_Spline, Splines.None)
                )
            Next

            Return legends
        End Function

    End Class
End Namespace