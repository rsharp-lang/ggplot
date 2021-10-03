Imports System.Drawing
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Plots
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Public MustInherit Class ggplotLayer

    Public Property reader As ggplotReader

    Public MustOverride Sub Plot(
        g As IGraphics,
        canvas As GraphicsRegion,
        baseData As ggplotData,
        x As Double(),
        y As Double(),
        scale As DataScaler,
        ggplot As ggplot,
        theme As Theme
    )

End Class

''' <summary>
''' 只绘制出一个基本的坐标轴
''' </summary>
Public Class ggplotBase

    Public Property reader As ggplotReader

End Class

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

        If reader Is Nothing Then
            serial = New SerialData() With {
                .color = color,
                .pointSize = size,
                .shape = shape,
                .title = $"{baseData.x} ~ {baseData.y}",
                .pts = x _
                    .Select(Function(xi, i)
                                Return New PointData(xi, y(i))
                            End Function) _
                    .ToArray
            }
        Else
            Throw New NotImplementedException
        End If

        Call Scatter2D.DrawScatter(g, serial.pts, scale, True, serial.shape, serial.pointSize, serial.BrushHandler).ToArray
    End Sub
End Class

Public Class ggplotLine : Inherits ggplotLayer

    Public Overrides Sub Plot(g As IGraphics,
                              canvas As GraphicsRegion,
                              baseData As ggplotData,
                              x() As Double,
                              y() As Double,
                              scale As DataScaler,
                              ggplot As ggplot,
                              theme As Theme)

        Throw New NotImplementedException()
    End Sub
End Class