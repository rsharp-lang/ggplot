Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Plot3D.Device
Imports Microsoft.VisualBasic.Imaging

Namespace layers.layer3d

    Public Interface Ilayer3d

        Function populateModels(g As IGraphics,
                                baseData As ggplotData,
                                x() As Double,
                                y() As Double,
                                z() As Double,
                                ggplot As ggplot,
                                theme As Theme) As IEnumerable(Of Element3D)

    End Interface
End Namespace