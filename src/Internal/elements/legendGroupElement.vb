Imports System.Drawing
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors

Public Interface IggplotLegendElement

    Property layout As Layout

    Sub Draw(g As IGraphics, canvas As GraphicsRegion, x As Double, y As Double)
    Function MeasureSize(g As IGraphics) As SizeF

End Interface

Public Class legendGroupElement : Inherits ggplotElement
    Implements IggplotLegendElement

    Public Overrides Property layout As Layout Implements IggplotLegendElement.layout
    Public Property legends As LegendObject()
    Public Property shapeSize As Size

    Public Sub Draw(g As IGraphics, canvas As GraphicsRegion, x As Double, y As Double) Implements IggplotLegendElement.Draw
        Call g.DrawLegends(New PointF(x, y), legends, $"{shapeSize.Width},{shapeSize.Height}")
    End Sub

    Public Function MeasureSize(g As IGraphics) As SizeF Implements IggplotLegendElement.MeasureSize
        Dim maxSizeLabel As String = legends.Select(Function(l) l.title).MaxLengthString
        Dim maxSize As SizeF = g.MeasureString(maxSizeLabel, legends(Scan0).GetFont(g.Dpi))

        maxSize = New SizeF(shapeSize.Width + maxSize.Width, maxSize.Height)
        maxSize = New SizeF(maxSize.Width, maxSize.Height * (legends.Length + 1))

        Return maxSize
    End Function
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

    Public Function MeasureSize(g As IGraphics) As SizeF Implements IggplotLegendElement.MeasureSize
        Dim titleSize = g.MeasureString(colorMapLegend.title, colorMapLegend.titleFont)
        Dim maxWidth As Single = {titleSize.Width, width}.Max

        Return New SizeF(maxWidth, height)
    End Function
End Class