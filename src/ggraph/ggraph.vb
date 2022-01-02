Imports System.Drawing
Imports ggplot.ggraph.render
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime

<Package("ggraph")>
Module ggraphPkg

    <ExportAPI("geom_edge_link")>
    Public Function geom_edge_link() As edgeRender
        Return New edgeRender
    End Function

    <ExportAPI("geom_node_point")>
    Public Function geom_node_point(Optional defaultColor As Object = NameOf(Color.SteelBlue),
                                    Optional env As Environment = Nothing) As nodeRender
        Return New nodeRender With {
            .defaultColor = RColorPalette _
                .getColor(defaultColor, NameOf(Color.SteelBlue)) _
                .TranslateColor
        }
    End Function

    <ExportAPI("geom_node_text")>
    Public Function geom_node_text() As textRender
        Return New textRender
    End Function
End Module
