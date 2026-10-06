Imports System.Drawing
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts.EdgeBundling.Mingle
Imports SMRUCC.Rsharp.Runtime

Namespace ggraph.layout

    ''' <summary>
    ''' layout_mingle(): the MINGLE edge bundling, which replaces a fan of
    ''' parallel edges by a small number of smooth bundled curves, so that the
    ''' dense parts of a network become readable.
    '''
    ''' The bundling does not move the nodes, it only produces the intermediate
    ''' control points of every edge, which are stored on the node data of the
    ''' bundling graph and consumed by the edgeBundleRender layer.
    '''
    ''' + k: the number of the nearest neighbours of the edge bundling
    ''' + rounds: the number of the bundling iterations
    ''' </summary>
    Public Class mingle : Inherits ggLayout

        Public Property k As Integer = 10
        Public Property rounds As Integer = 1
        ''' <summary>
        ''' the maximum number of the edges which take part in the bundling
        ''' </summary>
        Public Property maxEdges As Integer = 5000

        Private _bundler As Bundler

        Public Overrides Sub Layout(g As NetworkGraph, canvas As SizeF, env As Environment)
            If g.connectedNodes.Length < 3 Then Return

            Call log(env, "bundling the edges...")

            Dim bundler As New Bundler()

            Call bundler.setNodes(g.connectedNodes)
            Call bundler.buildNearestNeighborGraph(k)

            For i As Integer = 1 To Math.Max(rounds, 1)
                Call bundler.MINGLE()
                Call log(env, $"round {i}/{rounds}")
            Next

            _bundler = bundler
            Call log(env, " ~done!")
        End Sub

        ''' <summary>
        ''' the intermediate control points of every bundled edge, keyed by
        ''' the source and the target node label
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property bundler As Bundler
            Get
                Return _bundler
            End Get
        End Property

        ''' <summary>
        ''' collect the bundled control points of the whole graph
        ''' </summary>
        ''' <returns>
        ''' the control point sequences, keyed by "source->target"
        ''' </returns>
        Public Function GetPaths() As Dictionary(Of String, PointF())
            Dim paths As New Dictionary(Of String, PointF())

            If _bundler Is Nothing Then Return paths

            For Each node As Node In _bundler.EnumerateNodes()
                Dim data As MingleNodeData = TryCast(node.data, MingleNodeData)

                If data Is Nothing Then Continue For
                If data.nodes Is Nothing OrElse data.nodes.Length < 2 Then Continue For

                For i As Integer = 1 To data.nodes.Length - 1
                    Call addPath(paths, data.nodes(i - 1), data.nodes(i))
                Next
            Next

            Return paths
        End Function

        Private Shared Sub addPath(paths As Dictionary(Of String, PointF()), u As Node, v As Node)
            Dim key As String = $"{u.label}->{v.label}"

            If paths.ContainsKey(key) Then Return

            Dim uu As AbstractVector = u.data.initialPostion
            Dim vv As AbstractVector = v.data.initialPostion

            If uu Is Nothing OrElse vv Is Nothing Then Return

            Call paths.Add(key, New PointF() {
                New PointF(CSng(uu.x), CSng(uu.y)),
                New PointF(CSng((uu.x + vv.x) / 2), CSng((uu.y + vv.y) / 2)),
                New PointF(CSng(vv.x), CSng(vv.y))
            })
        End Sub
    End Class
End Namespace
