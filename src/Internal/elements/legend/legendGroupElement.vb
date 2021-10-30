Imports System.Drawing
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors

Namespace elements.legend

    Public Class legendGroupElement : Inherits ggplotElement
        Implements IggplotLegendElement

        Public Overrides Property layout As Layout Implements IggplotLegendElement.layout
        Public Property legends As LegendObject()
        Public Property shapeSize As New Size(120, 45)

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

End Namespace