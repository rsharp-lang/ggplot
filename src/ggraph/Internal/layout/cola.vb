Imports System.Drawing
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports SMRUCC.Rsharp.Runtime
Imports colaNS = Microsoft.VisualBasic.Data.visualize.Network.Layouts.Cola
Imports randf = Microsoft.VisualBasic.Math.RandomExtensions

Namespace ggraph.layout

    ''' <summary>
    ''' layout_cola(): the constraint based layout of the cola library, which
    ''' minimizes the stress of the layout under the given constraints.
    '''
    ''' The layout is produced by the stress majorization, and the result is
    ''' copied back into the initialPostion field of the node data.
    ''' </summary>
    Public Class cola_layout : Inherits ggLayout

        Public Property iterations As Integer = 200
        Public Property unconstrainedIterations As Integer = 0
        Public Property constraintIterations As Integer = 0
        Public Property gridSnapIterations As Integer = 0
        Public Property avoidOverlaps As Boolean = False
        Public Property handleDisconnected As Boolean = True
        Public Property flowLayout As Boolean = False
        Public Property flowAxis As String = "y"
        Public Property centerGraph As Boolean = True

        Public Overrides Sub Layout(g As NetworkGraph, canvas As SizeF, env As Environment)
            If g.connectedNodes.Length = 0 Then Return

            Call log(env, "computing the cola constraint layout...")

            Dim engine As New colaNS.Layout()
            Dim nodes As New List(Of colaNS.Node)

            If g.connectedNodes.Length > 2000 Then
                Call g.doRandomLayout()
            End If

            For Each node As Node In g.connectedNodes
                Dim position As PointF = readPosition(node)
                Dim extent As Double = 1.0

                If Not node.data Is Nothing AndAlso Not node.data.size Is Nothing AndAlso node.data.size.Length > 0 Then
                    extent = node.data.size(Scan0) * 2
                End If

                Dim colaNode As New colaNS.Node()
                colaNode.x = position.X
                colaNode.y = position.Y
                colaNode.width = extent
                colaNode.height = extent

                Call nodes.Add(colaNode)
            Next

            Call engine.nodes(nodes.ToArray)

            If handleDisconnected Then
                Call engine.handleDisconnected()
            End If

            If flowLayout Then
                Call engine.flowLayout(CStr(flowAxis), 10.0)
            End If

            Call engine.start(unconstrainedIterations, constraintIterations, constraintIterations, gridSnapIterations, False, centerGraph)

            If avoidOverlaps Then
                Call engine.avoidOverlaps()
            End If

            Call writeBack(g, engine)
            Call log(env, " ~done!")
        End Sub

        Private Shared Sub writeBack(g As NetworkGraph, engine As colaNS.Layout)
            Dim result As colaNS.Node() = engine.nodes()

            If result Is Nothing Then Exit Sub

            Dim i As Integer = 0

            For Each node As Node In g.connectedNodes
                If i >= result.Length Then Exit For

                If node.data Is Nothing Then
                    node.data = New NodeData With {.label = node.label}
                End If

                node.data.initialPostion = New FDGVector2(result(i).x, result(i).y)
                i += 1
            Next
        End Sub

        Private Shared Function readPosition(node As Node) As PointF
            If node.data Is Nothing OrElse node.data.initialPostion Is Nothing Then
                Return New PointF(0, 0)
            End If

            Dim vector As AbstractVector = node.data.initialPostion

            Return New PointF(CSng(vector.x), CSng(vector.y))
        End Function
    End Class

    ''' <summary>
    ''' layout_cola3d(): the three dimensional variant of the cola constraint
    ''' layout, which additionally produces the z coordinate of every node.
    ''' </summary>
    Public Class cola3d_layout : Inherits ggLayout

        Public Property iterations As Integer = 100
        Public Property idealLinkLength As Double = 1

        Public Overrides Function Produces3D() As Boolean
            Return True
        End Function

        Public Overrides Sub Layout(g As NetworkGraph, canvas As SizeF, env As Environment)
            If g.connectedNodes.Length = 0 Then Return

            Call log(env, "computing the cola 3D layout...")

            Dim all As Node() = g.connectedNodes
            Dim nodes As colaNS.Node3D() = New colaNS.Node3D(all.Length - 1) {}

            For i As Integer = 0 To all.Length - 1
                Dim position As PointF = readPosition(all(i))

                If position.X = 0 AndAlso position.Y = 0 Then
                    position = New PointF(
                        CSng(randf.seeds.NextDouble * canvas.Width),
                        CSng(randf.seeds.NextDouble * canvas.Height)
                    )
                End If

                nodes(i) = New colaNS.Node3D(position.X, position.Y, 0)
            Next

            Dim index As New Dictionary(Of String, Integer)

            For i As Integer = 0 To all.Length - 1
                Call index.Add(all(i).label, i)
            Next

            Dim links As New List(Of colaNS.Link(Of Integer))

            For Each edge In g.graphEdges
                Dim link As New colaNS.Link(Of Integer)()

                link.source = index(edge.U.label)
                link.target = index(edge.V.label)

                Call links.Add(link)
            Next

            Dim engine As New colaNS.Layout3D(nodes, links.ToArray(), idealLinkLength)

            Call engine.start(iterations)

            Dim coordinates As Double()() = engine.descent.x

            If coordinates Is Nothing Then
                Call log(env, " ~no result")
                Return
            End If

            For i As Integer = 0 To all.Length - 1
                If all(i).data Is Nothing Then
                    all(i).data = New NodeData With {.label = all(i).label}
                End If

                all(i).data.initialPostion = New FDGVector3(
                    coordinates(0)(i),
                    coordinates(1)(i),
                    coordinates(2)(i)
                )
            Next

            Call log(env, " ~done!")
        End Sub

        Private Shared Function readPosition(node As Node) As PointF
            If node.data Is Nothing OrElse node.data.initialPostion Is Nothing Then
                Return New PointF(0, 0)
            End If

            Dim vector As AbstractVector = node.data.initialPostion

            Return New PointF(CSng(vector.x), CSng(vector.y))
        End Function
    End Class
End Namespace
