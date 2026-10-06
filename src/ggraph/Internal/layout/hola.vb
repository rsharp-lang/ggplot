Imports System.Drawing
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts.Orthogonal
Imports SMRUCC.Rsharp.Runtime
Imports holaNS = Microsoft.VisualBasic.Data.visualize.Network.Layouts.Hola
Imports orthoNS = Microsoft.VisualBasic.Data.visualize.Network.Layouts.Orthogonal

Namespace ggraph.layout

    ''' <summary>
    ''' layout_hola(): the incremental HOLA layout, which relaxes the node
    ''' positions towards the alignment, the level scan and the spread
    ''' constraints step by step, so that the layout of a large graph can be
    ''' updated incrementally.
    '''
    ''' + nodeGap: the minimum distance between two nodes
    ''' + desiredEdgeLength: the preferred length of an edge
    ''' + alignEpsilon: the threshold of the alignment constraint
    ''' + convergeEpsilon: the convergence threshold
    ''' + maxIterations: the maximum number of the relaxation iterations
    ''' </summary>
    Public Class hola_layout : Inherits ggLayout

        Public Property nodeGap As Double = 30
        Public Property desiredEdgeLength As Double = 60
        Public Property alignEpsilon As Double = 4
        Public Property convergeEpsilon As Double = 0.01
        Public Property maxIterations As Integer = 200

        Public Overrides Sub Layout(g As NetworkGraph, canvas As SizeF, env As Environment)
            If g.connectedNodes.Length = 0 Then Return

            Call log(env, "computing the HOLA incremental layout...")

            Dim options As New holaNS.HolaOptions With {
                .nodeGap = nodeGap,
                .desiredEdgeLength = desiredEdgeLength,
                .alignEpsilon = alignEpsilon,
                .convergeEpsilon = convergeEpsilon,
                .maxIterations = maxIterations
            }

            Dim layouter As New holaNS.HolaLayouter()
            Dim result As NetworkGraph = layouter.Layout(g, options)

            Call log(env, " ~done!")
        End Sub
    End Class

    ''' <summary>
    ''' layout_orthogonal(): routes the edges as orthogonal polylines, so that
    ''' the edges never cross a node and never overlap each other.
    '''
    ''' The routed bends are stored in the bends field of the edge data, which
    ''' the orthEdgeRender layer consumes.
    ''' </summary>
    Public Class orthogonal_layout : Inherits ggLayout

        Public Property simplify As Boolean = True
        Public Property fixNonOrthogonal As Boolean = True

        Public Overrides Sub Layout(g As NetworkGraph, canvas As SizeF, env As Environment)
            If g.graphEdges.Count = 0 Then Return

            Call log(env, "routing the orthogonal edges...")

            Call g.AstarRouter()

            Call log(env, " ~done!")
        End Sub
    End Class

    ''' <summary>
    ''' layout_force3d(): the three dimensional spring force layout, which
    ''' additionally produces the z coordinate of every node.
    ''' </summary>
    Public Class force3d : Inherits ggforce

        Public Property stiffness As Double = 50000
        Public Property repulsion As Double = 100
        Public Property damping As Double = 0.9

        Public Overrides Function Produces3D() As Boolean
            Return True
        End Function

        Protected Overrides Function createAlgorithm(g As NetworkGraph) As IPlanner
            Return New SpringForce.ForceDirected3D(g, stiffness, repulsion, damping)
        End Function
    End Class
End Namespace
