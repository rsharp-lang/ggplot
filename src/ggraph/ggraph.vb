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
End Module
