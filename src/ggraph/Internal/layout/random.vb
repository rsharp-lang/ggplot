Imports System.Drawing
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports SMRUCC.Rsharp.Runtime
Imports randf = Microsoft.VisualBasic.Math.RandomExtensions

Namespace ggraph.layout

    ''' <summary>
    ''' layout_random(): assigns a random position to every node, which is
    ''' mostly used as the initial state of the other iterative layouts and as
    ''' a baseline for the layout comparison.
    '''
    ''' The seed makes the random layout reproducible.
    ''' </summary>
    Public Class random : Inherits ggLayout

        Public Property [seed] As Integer = 0

        Public Sub New()
            canvasSize = New SizeF(1000, 1000)
        End Sub

        Public Overrides Sub Layout(g As NetworkGraph, canvas As SizeF, env As Environment)
            If [seed] <> 0 Then
                Call randf.SetSeed([seed])
            End If

            Call log(env, "generating the random layout...")

            Dim i As Integer = 0

            For Each node As Node In g.connectedNodes
                If node.data Is Nothing Then
                    node.data = New NodeData With {.label = node.label}
                End If

                node.data.initialPostion = New FDGVector2(
                    randf.seeds.NextDouble * canvas.Width,
                    randf.seeds.NextDouble * canvas.Height
                )

                i += 1
            Next

            Call log(env, $" ~done! {i} nodes")
        End Sub
    End Class
End Namespace
