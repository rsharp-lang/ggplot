Imports System.Drawing
Imports ggplot.elements.legend

Namespace layers

    Public Class ggplotRaster : Inherits ggplotLayer

        ''' <summary>
        ''' the layout and position of the raster image plot placed at, nothing means overlaps the entire plot region
        ''' </summary>
        ''' <returns></returns>
        Public Property layout As RectangleF = Nothing
        ''' <summary>
        ''' the raster image
        ''' </summary>
        ''' <returns></returns>
        Public Property image As Image

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            If layout.IsEmpty Then

            End If

            Return Nothing
        End Function
    End Class
End Namespace