Imports System.Drawing
Imports ggplot.options
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports SMRUCC.Rsharp.Runtime

Namespace ggraph.layout

    ''' <summary>
    ''' The base class of all the graph layouts.
    '''
    ''' Unlike the iterative layouts(force directed, spring force), most of the
    ''' layout algorithms of the network_layout library are one-shot calls, so
    ''' the layout is expressed as a single Layout method instead of the
    ''' createAlgorithm/Collide iteration loop of ggforce.
    '''
    ''' All the layouts write their result back into the initialPostion field of
    ''' the node data, which is the layout format that the graph renderer
    ''' consumes.
    ''' </summary>
    Public MustInherit Class ggLayout : Inherits ggplotOption

        Public Property canvasSize As SizeF

        ''' <summary>
        ''' does this layout produce a three dimensional result?
        ''' </summary>
        Public Overridable Function Produces3D() As Boolean
            Return False
        End Function

        ''' <summary>
        ''' arrange the nodes of the graph
        ''' </summary>
        ''' <param name="g">the graph to be laid out, the result is written back into it</param>
        ''' <param name="canvas">the size of the plot canvas</param>
        ''' <param name="env">the R# environment, used for the progress logging</param>
        Public Overridable Sub Layout(g As NetworkGraph, canvas As SizeF, env As Environment)
        End Sub

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            If Not ggplot.args Is Nothing Then
                ggplot.args.slots(NameOf(ggLayout)) = Me
            End If

            Return ggplot
        End Function

        ''' <summary>
        ''' the center of the plot canvas, which is the default position of
        ''' the circular and the radial layouts
        ''' </summary>
        Protected ReadOnly Property center As PointF
            Get
                Return New PointF(canvasSize.Width / 2, canvasSize.Height / 2)
            End Get
        End Property

        Protected Shared Sub log(env As Environment, message As String)
            If env Is Nothing Then Exit Sub

            env.WriteLineHandler.Invoke(message)
        End Sub
    End Class
End Namespace
