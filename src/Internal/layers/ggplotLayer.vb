Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Public MustInherit Class ggplotLayer

    Public Property reader As ggplotReader
    Public Property showLegend As Boolean = True

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