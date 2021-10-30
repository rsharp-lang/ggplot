Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Namespace layers.layer3d

    Public Class ggplotScatter3d : Inherits ggplotScatter

        Public Overrides Function Plot(g As IGraphics,
                                       canvas As GraphicsRegion,
                                       baseData As ggplotData,
                                       x() As Double,
                                       y() As Double,
                                       scale As DataScaler,
                                       ggplot As ggplot,
                                       theme As Theme) As IggplotLegendElement

            Return MyBase.Plot(g, canvas, baseData, x, y, scale, ggplot, theme)
        End Function
    End Class
End Namespace