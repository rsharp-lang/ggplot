Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Public Class ggplotTextLabel : Inherits ggplotLayer

    ''' <summary>
    ''' calling from the ``geom_label`` function?
    ''' </summary>
    ''' <returns></returns>
    Public Property isLabeler As Boolean

    Public Overrides Function Plot(g As IGraphics,
                                   canvas As GraphicsRegion,
                                   baseData As ggplotData,
                                   x() As Double,
                                   y() As Double,
                                   scale As DataScaler,
                                   ggplot As ggplot,
                                   theme As Theme) As legendGroupElement

        Dim legend As legendGroupElement = Nothing



        If showLegend Then
            Return legend
        Else
            Return Nothing
        End If
    End Function
End Class
