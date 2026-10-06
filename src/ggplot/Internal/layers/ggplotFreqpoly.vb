Imports System.Drawing
Imports ggplot.colors
Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Plots.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    ''' <summary>
    ''' geom_freqpoly(): computes the binned frequency of the plot data and
    ''' draws the frequency counts as a line chart, which makes the shape of
    ''' the distribution much easier to read than a histogram.
    '''
    ''' + bins: the number of the equal width bins
    ''' + binwidth: the width of each bin, takes precedence over bins
    ''' + binRange: the range of the binning, defaults to the data range
    ''' </summary>
    Public Class ggplotFreqpoly : Inherits ggplotLayer

        Public Property bins As Integer = 30
        Public Property binwidth As Double = 0
        Public Property binRange As Double()
        Public Property line_width As Single = 1

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot As ggplot = stream.ggplot
            Dim g As IGraphics = stream.g
            Dim css As CSSEnvirnment = g.LoadEnvironment
            Dim reader As ggplotReader = If(useCustomData, Me.reader, ggplot.base.reader)
            Dim source As Object = If(useCustomData, dataset, ggplot.data)
            Dim y As Double() = stream.y

            If y.Length = 0 Then Return Nothing

            Dim groups As String() = resolveGroups(reader, source, y.Length, ggplot.environment)

            If colorMap Is Nothing Then
                colorMap = ggplotColorMap.CreateColorMap(map:="Paper", alpha:=alpha, env:=ggplot.environment)
            End If

            Dim pen As Pen = css.GetPen(Stroke.TryParse(stream.theme.lineStroke), allowNull:=True)

            For Each group As NamedCollection(Of Double) In ggplotGroup.getDataGroups(groups, y)
                Dim buffer = binned(group.value)

                If buffer.x.Length < 2 Then Continue For

                Dim points As PointF() = buffer.x _
                    .Select(Function(xi, i) stream.scale.Translate(New PointF(xi, buffer.y(i)))) _
                    .ToArray

                For i As Integer = 1 To points.Length - 1
                    Call g.DrawLine(pen, points(i - 1), points(i))
                Next
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' compute the bin centers and the frequency counts
        ''' </summary>
        Private Function binned(values As Double()) As (x As Double(), y As Double())
            Dim lower As Double
            Dim upper As Double

            If binRange IsNot Nothing AndAlso binRange.Length >= 2 Then
                lower = binRange(Scan0)
                upper = binRange(1)
            Else
                lower = values.Min
                upper = values.Max
            End If

            If upper <= lower Then upper = lower + 1

            Dim width As Double = If(binwidth > 0, binwidth, (upper - lower) / Math.Max(bins, 1))
            Dim n As Integer = Math.Max(CInt(Math.Ceiling((upper - lower) / width)), 1)
            Dim counts As Integer() = New Integer(n - 1) {}

            For Each v As Double In values
                Dim index As Integer = CInt(Math.Floor((v - lower) / width))

                If index < 0 Then index = 0
                If index >= n Then index = n - 1

                counts(index) += 1
            Next

            Dim centers As Double() = New Double(n - 1) {}

            For i As Integer = 0 To n - 1
                centers(i) = lower + width * (i + 0.5)
            Next

            Return (centers, counts.Select(Function(c) CDbl(c)).ToArray)
        End Function

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