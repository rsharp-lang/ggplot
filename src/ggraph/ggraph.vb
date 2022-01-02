
Imports ggplot.ggraph
Imports ggplot.ggraph.layout
Imports ggplot.ggraph.render
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData

<Package("ggraph")>
Module ggraphPkg

    <ExportAPI("geom_edge_link")>
    Public Function geom_edge_link() As edgeRender
        Return New edgeRender
    End Function

    <ExportAPI("geom_node_point")>
    Public Function geom_node_point() As nodeRender
        Return New nodeRender
    End Function

    <ExportAPI("geom_node_text")>
    Public Function geom_node_text() As textRender
        Return New textRender
    End Function

    <ExportAPI("layout_random")>
    Public Function layout_random() As ggforce
        Return New layout.random
    End Function

    <ExportAPI("layout_forcedirected")>
    Public Function layout_forcedirected() As force_directed
        Return New force_directed
    End Function
End Module
