Imports System.Drawing
Imports ggplot.colors
Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Plots.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.Quantile
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    ''' <summary>
    ''' geom_density(): computes a kernel density estimate of the plot data
    ''' and draws it as a filled area with an optional outline.
    '''
    ''' ### Bandwidth selection
    '''
    ''' + bw = "scott": the Scott's rule, 1.06 * sd * n^(-1/5)
    ''' + bw = "silverman": the Silverman's robust rule,
    '''   0.9 * min(sd, IQR/1.34) * n^(-1/5)
    ''' + a numeric value: use the given bandwidth directly
    '''
    ''' The bandwidth is further multiplied by the <see cref="adjust"/> factor.
    ''' </summary>
    Public Class ggplotDensity : Inherits ggplotLayer

        ''' <summary>
        ''' the number of the density evaluation grid points
        ''' </summary>
        Public Property bins As Integer = 512
        Public Property bw As Object = "scott"
        Public Property adjust As Double = 1
        ''' <summary>
        ''' extend the density curve beyond the data range by cut * bw
        ''' </summary>
        Public Property cut As Double = 3
        Public Property trim As Boolean = False
        ''' <summary>
        ''' draw the outline of the density curve
        ''' </summary>
        Public Property outline As Boolean = True
        Public Property line_width As Single = 1

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot As ggplot = stream.ggplot
            Dim g As IGraphics = stream.g
            Dim css As CSSEnvirnment = g.LoadEnvironment
            Dim reader As ggplotReader = If(useCustomData, Me.reader, ggplot.base.reader)
            Dim source As Object = If(useCustomData, dataset, ggplot.data)
            Dim y As Double() = CLRVector.asNumeric(stream.x).Where(Function(v) Not Double.IsNaN(v)).ToArray

            If y.Length < 2 Then Return Nothing

            Dim groups As String() = resolveGroups(reader, source, y.Length, ggplot.environment)

            If colorMap Is Nothing Then
                colorMap = ggplotColorMap.CreateColorMap(map:="Paper", alpha:=alpha, env:=ggplot.environment)
            End If

            Dim colors As Func(Of Object, String) = colorMap.ColorHandler(ggplot, groups.Distinct.ToArray)
            Dim pen As Pen = css.GetPen(Stroke.TryParse(stream.theme.lineStroke), allowNull:=True)

            For Each group As NamedCollection(Of Double) In ggplotGroup.getDataGroups(groups, y)
                Dim values As Double() = group.value

                If values.Length < 2 Then Continue For

                Dim bandwidth As Double = resolveBandwidth(values)
                Dim grid As Double() = createGrid(values, bandwidth)
                Dim density As Double() = kernelDensity(values, grid, bandwidth)

                Dim polygon As PointF() = grid _
                    .Select(Function(xi, i) New PointF(xi, density(i))) _
                    .Concat({New PointF(grid.Last, 0), New PointF(grid(Scan0), 0)}) _
                    .Select(Function(pt) stream.scale.Translate(pt)) _
                    .ToArray

                If polygon.Length < 3 Then Continue For

                Dim fill As Brush = colors(group.name).GetBrush

                If TypeOf fill Is SolidBrush Then
                    fill = New SolidBrush(DirectCast(fill, SolidBrush).Color.Alpha(alpha * 255))
                End If

                Call g.FillPolygon(fill, polygon)

                If outline AndAlso pen IsNot Nothing Then
                    Dim line As PointF() = grid _
                        .Select(Function(xi, i) stream.scale.Translate(New PointF(xi, density(i)))) _
                        .ToArray

                    For i As Integer = 1 To line.Length - 1
                        Call g.DrawLine(pen, line(i - 1), line(i))
                    Next
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' resolve the kernel bandwidth of the given data
        ''' </summary>
        Private Function resolveBandwidth(values As Double()) As Double
            Dim n As Double = values.Length
            Dim mean As Double = values.Average
            Dim variance As Double = values.Select(Function(v) (v - mean) * (v - mean)).Average
            Dim sd As Double = Math.Sqrt(variance)
            Dim h As Double

            If TypeOf bw Is String Then
                Dim quartile As DataQuartile = values.Quartile
                Dim iqrSpread As Double = quartile.IQR / 1.349

                Select Case DirectCast(bw, String).ToLower
                    Case "silverman", "nrd0"
                        h = 0.9 * Math.Min(sd, iqrSpread) * Math.Pow(n, -0.2)
                    Case "nrd"
                        h = 1.06 * Math.Min(sd, iqrSpread) * Math.Pow(n, -0.2)
                    Case Else
                        h = 1.06 * sd * Math.Pow(n, -0.2)
                End Select
            Else
                h = CDbl(bw)
            End If

            h *= adjust

            If h <= 0 Then h = Math.Max(Math.Abs(mean), 1) * 1E-3

            Return h
        End Function

        ''' <summary>
        ''' create the density evaluation grid
        ''' </summary>
        Private Function createGrid(values As Double(), bandwidth As Double) As Double()
            Dim lower As Double = values.Min
            Dim upper As Double = values.Max

            If Not trim Then
                lower -= cut * bandwidth
                upper += cut * bandwidth
            End If

            Return linspace(lower, upper, bins)
        End Function

        Private Shared Function linspace(lower As Double, upper As Double, n As Integer) As Double()
            Dim buffer As Double() = New Double(n - 1) {}
            Dim delta As Double = (upper - lower) / (n - 1)

            For i As Integer = 0 To n - 1
                buffer(i) = lower + delta * i
            Next

            Return buffer
        End Function

        ''' <summary>
        ''' the gaussian kernel density estimation
        ''' </summary>
        Private Shared Function kernelDensity(values As Double(), grid As Double(), h As Double) As Double()
            Dim n As Double = values.Length
            Dim res As Double() = New Double(grid.Length - 1) {}
            Dim norm As Double = 1 / (n * h * Math.Sqrt(2 * Math.PI))
            Dim scale As Double = 1 / h

            For i As Integer = 0 To grid.Length - 1
                Dim sum As Double = 0

                For j As Integer = 0 To values.Length - 1
                    Dim z As Double = (grid(i) - values(j)) * scale
                    sum += Math.Exp(-0.5 * z * z)
                Next

                res(i) = sum * norm
            Next

            Return res
        End Function

        ''' <summary>
        ''' resolve the group label of each observation
        ''' </summary>
        Private Shared Function resolveGroups(reader As ggplotReader,
                                              source As Object,
                                              nsize As Integer,
                                              env As SMRUCC.Rsharp.Runtime.Environment) As String()
            Dim mapping As String = If(reader.color,
                                       DirectCast(reader.[class], String),
                                       DirectCast(reader.group, String))

            If mapping Is Nothing Then
                Return Enumerable.Repeat(".", nsize).ToArray
            End If

            Return reader.getMapData(Of String)(source, mapping, env)
        End Function
    End Class
End Namespace