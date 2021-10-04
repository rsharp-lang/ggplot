Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Public Class ggplotHistogram : Inherits ggplotLayer

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
