Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render

Namespace layers

    ''' <summary>
    ''' A special version of geom_raster() which is optimised for the static
    ''' annotations that are the same in every plot panel.
    '''
    ''' These annotations will not affect the scales (i.e. the x and y axes will
    ''' not grow to cover the range of the raster), which makes it useful for
    ''' adding a bitmap image onto an existing ggplot object.
    '''
    ''' The annotation region is described by the relative coordinates
    ''' <see cref="xmin"/>/<see cref="xmax"/>/<see cref="ymin"/>/<see cref="ymax"/>,
    ''' whose values are in the range [0,1] and are relative to the plot region.
    ''' </summary>
    Public Class ggplotAnnotationRaster : Inherits ggplotRaster

        Public Property xmin As Double = 0
        Public Property xmax As Double = 1
        Public Property ymin As Double = 0
        Public Property ymax As Double = 1

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            If image Is Nothing Then Return Nothing

            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim plotRect As Rectangle = stream.canvas.PlotRegion(css)

            If plotRect.Width <= 0 OrElse plotRect.Height <= 0 Then
                Return Nothing
            End If

            Dim rect As New RectangleF(
                plotRect.Left + plotRect.Width * xmin,
                plotRect.Top + plotRect.Height * ymin,
                plotRect.Width * (xmax - xmin),
                plotRect.Height * (ymax - ymin)
            )

            Call stream.g.DrawImage(image, rect)

            Return Nothing
        End Function
    End Class
End Namespace