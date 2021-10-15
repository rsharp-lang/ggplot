Imports System.Drawing
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors

Public Class legendColorMapElement : Inherits ggplotElement
    Implements IggplotLegendElement

    Public Overrides Property layout As Layout Implements IggplotLegendElement.layout
    Public Property colorMapLegend As ColorMapLegend
    Public Property width As Single
    Public Property height As Single

    Public Sub Draw(g As IGraphics, canvas As GraphicsRegion, x As Double, y As Double) Implements IggplotLegendElement.Draw
        Call colorMapLegend.Draw(g, New Rectangle(x, y, width, height))
    End Sub

    Public Function MeasureSize(g As IGraphics) As SizeF Implements IggplotLegendElement.MeasureSize
        Dim titleSize = g.MeasureString(colorMapLegend.title, colorMapLegend.titleFont)
        Dim maxWidth As Single = {titleSize.Width, width}.Max

        Return New SizeF(maxWidth, height)
    End Function
End Class