Imports ggplot.options
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts.ForceDirected

Namespace ggraph.layout

    Public MustInherit Class ggforce : Inherits ggplotOption

        Protected MustOverride Function createAlgorithm(g As NetworkGraph) As Planner

        Public Sub createLayout(g As NetworkGraph)
            Dim algorithm As Planner = createAlgorithm(g.doRandomLayout)

            For i As Integer = 0 To 200
                Call algorithm.Collide()
            Next
        End Sub

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.args.slots(NameOf(ggforce)) = Me
            Return ggplot
        End Function

    End Class
End Namespace