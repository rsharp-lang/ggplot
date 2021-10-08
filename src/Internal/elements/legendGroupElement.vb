Imports System.Drawing
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Public Class legendGroupElement : Inherits ggplotElement

    Public Property legends As LegendObject()

    Public Sub Draw(g As IGraphics, canvas As GraphicsRegion, x As Double, y As Double)
        Call g.DrawLegends(New PointF(x, y), legends)
    End Sub
End Class
