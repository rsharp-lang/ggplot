
Imports ggplot.layers
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("ggpubr")>
Module Rscript

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
