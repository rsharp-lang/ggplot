Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Plots
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime

Public Class ggplotScatter : Inherits ggplotLayer

    Public Property shape As LegendStyles
    Public Property size As Single

    Public Overrides Function Plot(
        g As IGraphics,
        canvas As GraphicsRegion,
        baseData As ggplotData,
        x As Double(),
        y As Double(),
        scale As DataScaler,
        ggplot As ggplot,
        theme As Theme
    ) As legendGroupElement

        Dim serial As SerialData
        Dim colors As String() = Nothing

        If useCustomColorMaps Then
            Dim factors As String() = REnv.asVector(Of String)(DirectCast(ggplot.data, dataframe).getColumnVector(reader.color))
            Dim maps = DirectCast(colorMap, ggplotColorFactorMap).ColorHandler(ggplot)

            colors = factors.Select(Function(factor) maps(factor)).ToArray
        ElseIf Not ggplot.base.reader.color Is Nothing Then
            colors = ggplot.base.getColors(ggplot)
        End If

        If Not useCustomData Then
            serial = createSerialData($"{baseData.x} ~ {baseData.y}", x, y, colors)
        Else
            With reader.getMapData(ggplot.data, ggplot.environment)
                serial = createSerialData(reader.ToString, .x, .y, colors)
            End With
        End If

        Call Scatter2D.DrawScatter(
            g:=g,
            scatter:=serial.pts,
            scaler:=scale,
            fillPie:=True,
            shape:=serial.shape,
            pointSize:=serial.pointSize,
            getPointBrush:=serial.BrushHandler
        ) _
        .ToArray
    End Function

    Private Function createSerialData(legend As String, x As Double(), y As Double(), colors As String()) As SerialData
        Return New SerialData() With {
            .color = If(colors Is Nothing, DirectCast(colorMap, ggplotColorLiteral).ToColor, Nothing),
            .pointSize = size,
            .shape = shape,
            .title = legend,
            .pts = x _
                .Select(Function(xi, i)
                            Return New PointData(xi, y(i)) With {
                                .color = If(colors Is Nothing, Nothing, colors(i))
                            }
                        End Function) _
                .ToArray
        }
    End Function
End Class