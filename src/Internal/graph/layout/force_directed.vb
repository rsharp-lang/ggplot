Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts.ForceDirected

Namespace ggraph.layout

    Public Class force_directed : Inherits ggforce

        Protected Overrides Function createAlgorithm(g As NetworkGraph) As Planner
            Return New ForceDirected.Planner(g)
        End Function
    End Class
End Namespace