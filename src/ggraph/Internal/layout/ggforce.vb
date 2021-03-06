Imports ggplot.options
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports Microsoft.VisualBasic.Emit.Delegates
Imports SMRUCC.Rsharp.Runtime

Namespace ggraph.layout

    Public MustInherit Class ggforce : Inherits ggplotOption

        Public Property iterations As Integer = 10000 * 2
        Public Property [step] As Double = 0.001

        Protected MustOverride Function createAlgorithm(g As NetworkGraph) As IPlanner

        Public Sub createLayout(ByRef g As NetworkGraph, env As Environment)
            Dim algorithm As IPlanner = createAlgorithm(g.doRandomLayout)
            Dim println = env.WriteLineHandler
            Dim delta As Integer = iterations / 10

            Call println("start create layout...")

            For i As Integer = 0 To iterations
                Call algorithm.Collide(timeStep:=[step])

                If i Mod delta = 0 Then
                    Call println($"[{(i / iterations * 100).ToString("F0")}%] ... {i}/{iterations}")
                End If
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