Imports System.Drawing
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Plots
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Public Class ggplotScatter : Inherits ggplotLayer

    Public Property color As Color
    Public Property shape As LegendStyles
    Public Property size As Single

    Public Overrides Sub Plot(g As IGraphics,
                              canvas As GraphicsRegion,
                              baseData As ggplotData,
                              x() As Double,
                              y() As Double,
                              scale As DataScaler,
                              ggplot As ggplot,
                              theme As Theme)

        Dim serial As SerialData
        Dim colors As String() = Nothing

        If Not ggplot.base.reader.color Is Nothing Then
            colors = ggplot.base.getColors(ggplot)
        End If

        If Not useCustomData Then
            serial = createSerialData(baseData, x, y, colors)
        Else
            Throw New NotImplementedException
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
    End Sub

    Private Function createSerialData(baseData As ggplotData, x As Double(), y As Double(), colors As String()) As SerialData
        Return New SerialData() With {
            .color = color,
            .pointSize = size,
            .shape = shape,
            .title = $"{baseData.x} ~ {baseData.y}",
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