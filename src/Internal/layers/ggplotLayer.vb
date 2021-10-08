Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Public MustInherit Class ggplotLayer

    Public Property reader As ggplotReader
    Public Property showLegend As Boolean = True

    Protected ReadOnly Property useCustomData As Boolean
        Get
            Return (Not reader Is Nothing) AndAlso Not reader.y.StringEmpty
        End Get
    End Property

    Protected ReadOnly Property useCustomColorMaps As Boolean
        Get
            Return (Not reader Is Nothing) AndAlso Not reader.color Is Nothing
        End Get
    End Property

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