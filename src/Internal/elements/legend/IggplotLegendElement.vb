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