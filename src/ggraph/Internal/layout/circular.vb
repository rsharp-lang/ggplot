Imports System.Drawing
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports SMRUCC.Rsharp.Runtime
Imports circularNS = Microsoft.VisualBasic.Data.visualize.Network.Layouts.Circular
Imports radialNS = Microsoft.VisualBasic.Data.visualize.Network.Layouts.Radial

Namespace ggraph.layout

    ''' <summary>
    ''' layout_circular(): places the nodes on a circle, and optionally
    ''' minimizes the number of the edge crossings by swapping the nodes.
    '''
    ''' + radius: the radius of the circle, defaults to the half of the canvas
    ''' + sortByDegree: sort the nodes by their degree
    ''' + crossingOptimization: minimize the number of the edge crossings
    ''' + maxSwaps: the maximum number of the swap attempts
    ''' </summary>
    Public Class circular_layout : Inherits ggLayout

        Public Property radius As Double = Double.NaN
        Public Property sortByDegree As Boolean = True
        Public Property crossingOptimization As Boolean = False
        Public Property maxSwaps As Integer = 1000

        Public Overrides Sub Layout(g As NetworkGraph, canvas As SizeF, env As Environment)
            If g.connectedNodes.Length = 0 Then Return

            Call log(env, "computing the circular layout...")

            Dim r As Double = If(Double.IsNaN(radius), Math.Min(canvas.Width, canvas.Height) / 2, radius)
            Dim params As New circularNS.CircularLayoutParameters With {
                .Radius = r,
                .CenterX = canvas.Width / 2,
                .CenterY = canvas.Height / 2,
                .SortByDegree = sortByDegree,
                .OptimizeCrossing = crossingOptimization,
                .MaxSwaps = maxSwaps
            }

            Dim result As NetworkGraph = circularNS.CircularLayout.LayoutNodes(g, params)

            Call log(env, " ~done!")
        End Sub
    End Class

    ''' <summary>
    ''' layout_radial(): places the nodes on concentric circles, the distance
    ''' from the center reflects the shortest path distance from the root node,
    ''' which makes the hierarchical structure of the network visible.
    '''
    ''' + radius: the radius of the outermost circle
    ''' </summary>
    Public Class radial_layout : Inherits ggLayout

        Public Property radius As Double = Double.NaN

        Public Overrides Sub Layout(g As NetworkGraph, canvas As SizeF, env As Environment)
            If g.connectedNodes.Length = 0 Then Return

            Call log(env, "computing the radial layout...")

            Dim r As Double = If(Double.IsNaN(radius), Math.Min(canvas.Width, canvas.Height) / 2, radius)
            Dim params As New radialNS.RadialLayoutParameters With {.Radius = r}

            Dim result As NetworkGraph = radialNS.RadialLayout.LayoutNodes(g, params)

            Call log(env, " ~done!")
        End Sub
    End Class
End Namespace
