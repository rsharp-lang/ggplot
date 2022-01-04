Imports ggplot.options
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts.ForceDirected
Imports SMRUCC.Rsharp.Runtime

Namespace ggraph.layout

    Public MustInherit Class ggforce : Inherits ggplotOption

        Public Property iterations As Integer = 10000 * 2

        Protected MustOverride Function createAlgorithm(g As NetworkGraph) As Planner

        Public Sub createLayout(g As NetworkGraph, env As Environment)
            Dim algorithm As Planner = createAlgorithm(g.doRandomLayout)
            Dim println = env.WriteLineHandler

            Call println("start create layout...")

            For i As Integer = 0 To iterations
                Call algorithm.Collide()
            Next

            Call println(" ~done!")
        End Sub

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.args.slots(NameOf(ggforce)) = Me
            Return ggplot
        End Function

    End Class
End Namespace