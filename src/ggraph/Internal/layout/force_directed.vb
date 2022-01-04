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

        ''' <summary>
        ''' force_directed|degree_weighted|group_weighted|edge_weighted
        ''' </summary>
        ''' <returns></returns>
        Public Property algorithm As String = "naive"

        Protected Overrides Function createAlgorithm(g As NetworkGraph) As IPlanner
            If algorithm = "naive" OrElse algorithm = "force_directed" Then
                Return New ForceDirected.Planner(
                    g:=g,
                    ejectFactor:=ejectFactor,
                    condenseFactor:=condenseFactor,
                    maxtx:=maxtx,
                    maxty:=maxty,
                    dist_threshold:=dist_threshold,
                    size:=size
                )
            ElseIf algorithm = "group_weighted" Then
                Return New ForceDirected.GroupPlanner(
                    g:=g,
                    ejectFactor:=ejectFactor,
                    condenseFactor:=condenseFactor,
                    maxtx:=maxtx,
                    maxty:=maxty,
                    dist_threshold:=dist_threshold,
                    size:=size,
                    groupAttraction:=10
                )
            Else
                Throw New NotImplementedException
            End If
        End Function
    End Class
End Namespace