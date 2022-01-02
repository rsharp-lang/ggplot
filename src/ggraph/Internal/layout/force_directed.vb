Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts.ForceDirected

Namespace ggraph.layout

    Public Class force_directed : Inherits ggforce

        Public ejectFactor As Integer = 6,
            condenseFactor As Integer = 3,
            maxtx As Integer = 4,
            maxty As Integer = 3,
            dist_threshold$ = "30,250",
            size$ = "1000,1000"

        Protected Overrides Function createAlgorithm(g As NetworkGraph) As Planner
            Return New ForceDirected.Planner(
                g:=g,
                ejectFactor:=ejectFactor,
                condenseFactor:=condenseFactor,
                maxtx:=maxtx,
                maxty:=maxty,
                dist_threshold:=dist_threshold,
                size:=size
            )
        End Function
    End Class
End Namespace