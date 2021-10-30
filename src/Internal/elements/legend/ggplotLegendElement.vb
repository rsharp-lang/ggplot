Imports System.Drawing
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors

Namespace elements.legend

    Public Class ggplotLegendElement : Inherits ggplotElement
        Implements IggplotLegendElement

        Public Property legend As LegendObject
        Public Property shapeSize As New Size(120, 45)
        Public Overrides Property layout As Layout Implements IggplotLegendElement.layout

        Public Sub Draw(g As IGraphics, canvas As GraphicsRegion, x As Double, y As Double) Implements IggplotLegendElement.Draw
            Throw New NotImplementedException()
        End Sub

        Public Function MeasureSize(g As IGraphics) As SizeF Implements IggplotLegendElement.MeasureSize
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace