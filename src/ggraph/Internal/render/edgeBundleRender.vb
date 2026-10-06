Imports System.Drawing
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render

Namespace ggraph.render

    ''' <summary>
    ''' The MINGLE edge bundling renderer, which replaces a fan of parallel
    ''' edges by a small number of smooth bundled curves, so that the dense
    ''' parts of a network become readable.
    '''
    ''' The intermediate control points are produced by the
    ''' <see cref="ggraph.layout.mingle"/> layout and are interpolated by a
    ''' Catmull-Rom spline, whose sample count adapts to the length of the edge
    ''' so that a long edge is smooth while a short edge stays cheap.
    ''' </summary>
    Public Class edgeBundleRender : Inherits ggplotLayer

        Public Property color As String = "black"
        Public Property alpha As Double = 0.5
        Public Property lineWidth As Single = 1
        ''' <summary>
        ''' the maximum number of the sample points of a single bundled curve
        ''' </summary>
        Public Property maxSamples As Integer = 24

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim pipeline As graphPipeline = DirectCast(stream, graphPipeline)
            Dim paths As Dictionary(Of String, PointF()) = pipeline.bundlePaths

            If paths Is Nothing OrElse paths.Count = 0 Then Return Nothing

            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim pen As Pen = css.GetPen(Stroke.TryParse($"{color}; stroke-width: {lineWidth}; stroke-opacity: {alpha}"), allowNull:=False)

            For Each path In paths
                If path.Value.Length < 2 Then Continue For

                Dim curve As PointF() = catmullRom(path.Value, maxSamples)

                For i As Integer = 1 To curve.Length - 1
                    Call stream.g.DrawLine(pen, curve(i - 1), curve(i))
                Next
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' interpolate the control points by a Catmull-Rom spline
        ''' </summary>
        ''' <param name="control">the control points of the curve</param>
        ''' <param name="maxSamples">the maximum number of the sample points</param>
        ''' <returns>the sampled polyline of the curve</returns>
        Private Shared Function catmullRom(control As PointF(), maxSamples As Integer) As PointF()
            If control.Length < 2 Then Return control

            Dim result As New List(Of PointF)
            Dim samples As Integer = Math.Max(8, Math.Min(maxSamples, control.Length * 4))

            For i As Integer = 0 To control.Length - 2
                Dim p0 As PointF = control(Math.Max(i - 1, 0))
                Dim p1 As PointF = control(i)
                Dim p2 As PointF = control(i + 1)
                Dim p3 As PointF = control(Math.Min(i + 2, control.Length - 1))
                Dim steps As Integer = Math.Max(CInt(samples / control.Length), 2)

                For j As Integer = 0 To steps - 1
                    Call result.Add(evaluate(p0, p1, p2, p3, j / steps))
                Next
            Next

            Call result.Add(control.Last)
            Return result.ToArray
        End Function

        Private Shared Function evaluate(p0 As PointF, p1 As PointF, p2 As PointF, p3 As PointF, t As Double) As PointF
            Dim t2 As Double = t * t
            Dim t3 As Double = t2 * t

            Dim x As Double = 0.5 * ((2 * p1.X) + (-p0.X + p2.X) * t +
                (2 * p0.X - 5 * p1.X + 4 * p2.X - p3.X) * t2 +
                (-p0.X + 3 * p1.X - 3 * p2.X + p3.X) * t3)
            Dim y As Double = 0.5 * ((2 * p1.Y) + (-p0.Y + p2.Y) * t +
                (2 * p0.Y - 5 * p1.Y + 4 * p2.Y - p3.Y) * t2 +
                (-p0.Y + 3 * p1.Y - 3 * p2.Y + p3.Y) * t3)

            Return New PointF(CSng(x), CSng(y))
        End Function
    End Class
End Namespace
