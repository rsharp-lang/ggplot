
Imports ggplot.layers
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("ggpubr")>
Module Rscript

    ''' <summary>
    ''' ### Compute normal data ellipses
    ''' 
    ''' 
    ''' </summary>
    ''' <param name="data">
    ''' The data to be displayed in this layer. There are three options:
    ''' If NULL, the Default, the data Is inherited from the plot data As specified In the Call To ggplot().
    ''' A data.frame, Or other Object, will override the plot data. All objects will be fortified To produce a data frame. See fortify() For which variables will be created.
    ''' A Function will be called With a Single argument, the plot data. The Return value must be a data.frame, And will be used As the layer data. A Function can be created from a formula (e.g. ~ head(.x, 10)).
    ''' </param>
    ''' <param name="color"></param>
    ''' <param name="level">The level at which to draw an ellipse, or, if type="euclid", the radius of the circle to be drawn.</param>
    ''' <param name="alpha"></param>
    ''' <returns></returns>
    <ExportAPI("stat_ellipse")>
    Public Function stat_ellipse(<RRawVectorArgument>
                                 Optional data As Object = Nothing,
                                 Optional color As Object = Nothing,
                                 Optional level As Double = 0.95,
                                 Optional alpha As Double = 0.6) As ggplotLayer

        Return New ggplotConfidenceEllipse With {
            .level = level,
            .showLegend = False,
            .alpha = alpha
        }
    End Function
End Module
