Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports ggplot
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.ChartPlots.Statistics.PCA
Imports Microsoft.VisualBasic.Imaging.Math2D

Public Class ggplotConfidenceEllipse : Inherits ggplotLayer

    Public Property level As Double = 0.95

    Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
        Dim x As Double()
        Dim y As Double()
        Dim group As New Polygon2D(x, y)
        Dim shape As Ellipse = Ellipse.ConfidenceEllipse(group, level)
        Dim path As GraphicsPath = shape.BuildPath
        Dim fill As Brush

        Call stream.g.FillPath(fill, path)

        Return Nothing
    End Function
End Class
