Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Language.Vectorization
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine

Public MustInherit Class ggplotLayer

    Public Property reader As ggplotReader
    Public Property colorMap As ggplotColorMap
    Public Property showLegend As Boolean = True
    Public Property which As Expression

    Protected ReadOnly Property useCustomData As Boolean
        Get
            If reader Is Nothing Then Return False

            Return Not (reader.y.StringEmpty AndAlso reader.label.StringEmpty)
        End Get
    End Property

    Protected ReadOnly Property useCustomColorMaps As Boolean
        Get
            Return (Not reader Is Nothing) AndAlso Not reader.color Is Nothing
        End Get
    End Property

    Public MustOverride Function Plot(
        g As IGraphics,
        canvas As GraphicsRegion,
        baseData As ggplotData,
        x As Double(),
        y As Double(),
        scale As DataScaler,
        ggplot As ggplot,
        theme As Theme
    ) As legendGroupElement

    Public Function getFilter(ggplot As ggplot) As BooleanVector

    End Function

End Class