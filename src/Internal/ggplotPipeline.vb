Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Public Class ggplotPipeline

    Friend g As IGraphics
    Friend canvas As GraphicsRegion
    Friend x As Double()
    Friend y As Double()
    Friend scale As DataScaler
    Friend ggplot As ggplot

    Public ReadOnly Property theme As Theme
        Get
            Return ggplot.ggplotTheme
        End Get
    End Property

    Public ReadOnly Property defaultTitle As String
        Get
            Return ggplot.base.reader.ToString
        End Get
    End Property

End Class
