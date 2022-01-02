Imports ggplot.ggraph
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph

Public Class zzz

    Public Shared Sub onLoad()
        Call ggplot.Register(GetType(NetworkGraph), Function(theme) New graphRender(theme))
    End Sub
End Class