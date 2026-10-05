Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.Plots
Imports Microsoft.VisualBasic.Data.Plots.Canvas
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
            Dim curves As New List(Of SerialData)

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

                Call curves.Add(curve)
            Next

            ' 委派给新引擎的 LinePlot：共享画布 + 跨图层联合坐标
            Call LayerRender.DrawLines(
                g:=stream.g,
                scaler:=stream.scale,
                theme:=stream.theme,
                serials:=curves,
                smooth:=bspline
            )

            If showLegend Then
                Return legends
            Else
                Return Nothing
            End If
        End Function

    End Class
End Namespace
