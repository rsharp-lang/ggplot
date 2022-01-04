Imports ggplot.options
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports Microsoft.VisualBasic.Emit.Delegates
Imports SMRUCC.Rsharp.Runtime

Namespace ggraph.layout

    Public MustInherit Class ggforce : Inherits ggplotOption

        Public Property iterations As Integer = 10000 * 2

        Protected MustOverride Function createAlgorithm(g As NetworkGraph) As IPlanner

        Public Sub createLayout(g As NetworkGraph, env As Environment)
            Dim algorithm As IPlanner = createAlgorithm(g.doRandomLayout)
            Dim println = env.WriteLineHandler

            Call println("start create layout...")

            For i As Integer = 0 To iterations
                Call algorithm.Collide(0.05)
            Next

            If algorithm.GetType.ImplementInterface(Of IDisposable) Then
                Call DirectCast(algorithm, IDisposable).Dispose()
            End If

            Call println(" ~done!")
        End Sub

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.args.slots(NameOf(ggforce)) = Me
            Return ggplot
        End Function

    End Class
End Namespace