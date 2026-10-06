Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.Plots.Canvas
Imports Microsoft.VisualBasic.Data.Plots.Plot3D.Legend
Imports Microsoft.VisualBasic.Imaging
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    ''' <summary>
    ''' geom_path() connects the observations in the order in which they
    ''' appear in the data.
    '''
    ''' The group aesthetic determines which cases are connected together, so
    ''' when no group/color mapping is defined, all the observations will be
    ''' connected as a single path.
    '''
    ''' ### Difference against geom_line()
    '''
    ''' + geom_line(): sorts the observations by the x axis value before connecting them
    ''' + geom_path(): keeps the original row order of the data
    '''
    ''' Because the ggplot rendering pipeline keeps the raw row order of the
    ''' mapped data, the path layer shares the whole line drawing logic of the
    ''' <see cref="ggplotLine"/> layer.
    ''' </summary>
    Public Class ggplotPath : Inherits ggplotLine

        Public Property lineend As String
        Public Property linejoin As String

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Return MyBase.Plot(stream)
        End Function
    End Class

End Namespace