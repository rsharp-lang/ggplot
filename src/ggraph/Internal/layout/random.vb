Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts.ForceDirected

Namespace ggraph.layout

    Public Class random : Inherits ggforce

        Protected Overrides Function createAlgorithm(g As NetworkGraph) As IPlanner
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace