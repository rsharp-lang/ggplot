Imports System.Drawing
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors

Public Interface IggplotLegendElement

    Property layout As Layout

    Sub Draw(g As IGraphics, canvas As GraphicsRegion, x As Double, y As Double)

End Interface

Public Class legendGroupElement : Inherits ggplotElement
    Implements IggplotLegendElement

    Public Overrides Property layout As Layout Implements IggplotLegendElement.layout
    Public Property legends As LegendObject()

    Public Sub Draw(g As IGraphics, canvas As GraphicsRegion, x As Double, y As Double) Implements IggplotLegendElement.Draw
        Call g.DrawLegends(New PointF(x, y), legends)
    End Sub
End Class

Public Class legendColorMapElement : Inherits ggplotElement
    Implements IggplotLegendElement

    Public Overrides Property layout As Layout Implements IggplotLegendElement.layout
    Public Property colorMapLegend As ColorMapLegend
    Public Property width As Single
    Public Property height As Single

    Public Sub Draw(g As IGraphics, canvas As GraphicsRegion, x As Double, y As Double) Implements IggplotLegendElement.Draw
        Call colorMapLegend.Draw(g, New Rectangle(x, y, width, height))
    End Sub
End Class