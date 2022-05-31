
Imports System.Drawing
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts

Namespace ggraph.layout

    Public Class spring_embedder : Inherits ggforce

        Public Property canvasSize As Size
        Public Property maxRepulsiveForceDistance As Double = 10
        Public Property c As Double

        Protected Overrides Function createAlgorithm(g As NetworkGraph) As IPlanner
            Return New SpringEmbedder(g, canvasSize, maxRepulsiveForceDistance, c:=c)
        End Function
    End Class
End Namespace