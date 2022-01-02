
Imports ggplot.ggraph
Imports ggplot.ggraph.layout
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData

<Package("ggforce")>
Module ggforcePkg

    <ExportAPI("layout_random")>
    Public Function layout_random() As ggforce
        Return New layout.random
    End Function

    <ExportAPI("layout_forcedirected")>
    Public Function layout_forcedirected() As force_directed
        Return New force_directed
    End Function
End Module
