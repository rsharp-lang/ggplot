Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts.SpringForce

Namespace ggraph.layout

    Public Class spring_force : Inherits ggforce

        Public Property stiffness As Double = 80
        Public Property repulsion As Double = 4000
        Public Property damping As Double = 0.83

        Protected Overrides Function createAlgorithm(g As NetworkGraph) As IPlanner
            Return New ForceDirected2D(g, stiffness, repulsion, damping)
        End Function
    End Class
End Namespace