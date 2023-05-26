Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging.Drawing3D

Namespace render

    Module g3d

        <Extension>
        Private Function Camera(ggplot As ggplot, plotSize As Size) As Camera
            Dim cameraVal As Object = ggplot.args.getByName("camera")

            If cameraVal Is Nothing Then
                Return New Camera With {
                    .screen = plotSize,
                    .fov = 100000,
                    .viewDistance = -75,
                    .angleX = 31.5,
                    .angleY = 65,
                    .angleZ = 125
                }
            Else
                With DirectCast(cameraVal, Camera)
                    .screen = plotSize
                    Return cameraVal
                End With
            End If
        End Function

        Private Iterator Function populateModels(g As IGraphics,
                                                 baseData As ggplotData,
                                                 x() As Double,
                                                 y() As Double,
                                                 z() As Double,
                                                 legends As List(Of IggplotLegendElement)) As IEnumerable(Of Element3D())

            Dim ppi As Integer = g.Dpi
            Dim max As Double = x.JoinIterates(y).JoinIterates(z).Max
            Dim min As Double = x.JoinIterates(y).JoinIterates(z).Min
            'Dim xTicks = x.Range.CreateAxisTicks
            'Dim yTicks = y.Range.CreateAxisTicks
            'Dim zTicks = z.Range.CreateAxisTicks
            Dim ticks = New DoubleRange(min, max).CreateAxisTicks
            Dim tickCss As String = CSSFont.TryParse(theme.axisTickCSS).SetFontColor(theme.mainTextColor).ToString

            ' 然后生成底部的网格
            Yield Grids.Grid1(ticks, ticks, (ticks(1) - ticks(0), ticks(1) - ticks(0)), ticks.Min, showTicks:=Not theme.axisTickCSS.StringEmpty, strokeCSS:=theme.gridStrokeX, tickCSS:=tickCss).ToArray
            Yield Grids.Grid2(ticks, ticks, (ticks(1) - ticks(0), ticks(1) - ticks(0)), ticks.Min, showTicks:=Not theme.axisTickCSS.StringEmpty, strokeCSS:=theme.gridStrokeX, tickCSS:=tickCss).ToArray
            Yield Grids.Grid3(ticks, ticks, (ticks(1) - ticks(0), ticks(1) - ticks(0)), ticks.Max, showTicks:=Not theme.axisTickCSS.StringEmpty, strokeCSS:=theme.gridStrokeX, tickCSS:=tickCss).ToArray

            Yield AxisDraw.Axis(
            xrange:=ticks, yrange:=ticks, zrange:=ticks,
            labelFontCss:=theme.axisLabelCSS,
            labels:=(xlabel, ylabel, zlabel),
            strokeCSS:=theme.axisStroke,
            arrowFactor:="1,2",
            labelColorVal:=theme.mainTextColor
        )

            For Each layer As ggplotLayer In layers.ToArray
                If layer.GetType.ImplementInterface(Of Ilayer3d) Then
                    Call layers.Remove(layer)

                    Yield DirectCast(layer, Ilayer3d) _
                    .populateModels(g, baseData, x, y, z, Me, theme, legends) _
                    .ToArray
                End If
            Next
        End Function

    End Module
End Namespace