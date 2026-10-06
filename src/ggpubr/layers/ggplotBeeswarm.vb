Imports System.Drawing
Imports ggplot
Imports ggplot.layers
Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Plots.Canvas
Imports Microsoft.VisualBasic.Data.Plots.Plot3D.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Linq

    ''' <summary>
    ''' The beeswarm(swarm) plot layer.
    '''
    ''' A jitter plot adds a uniform random offset to every point, which makes
    ''' the points of the same group overlap. A beeswarm instead places the
    ''' points symmetrically around the group center, so that the density of
    ''' the group becomes readable and no point is hidden.
    '''
    ''' The offsets are computed by sorting the observations of a group and
    ''' assigning each observation the smallest free slot whose distance to the
    ''' already placed points is large enough.
    ''' </summary>
    Public Class ggplotBeeswarm : Inherits ggplotGroup

        ''' <summary>
        ''' the direction of the swarm, up or down
        ''' </summary>
        Public Property direction As String = "up"
        Public Property swarmSize As Double = 6
        Public Property cex As Double = 1

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim tags As String() = stream.x
            Dim y As Double() = stream.y

            If tags.Length = 0 OrElse tags.Length <> y.Length Then Return Nothing

            Dim colors As Func(Of Object, String) = getColors(stream, tags.Distinct)
            Dim data = getDataGroups(stream).ToArray
            Dim center As Double() = stream.TranslateX()
            Dim binWidth As Double = resolveBinWidth(stream)

            For Each group As NamedCollection(Of Double) In data
                Dim index As Integer() = indexOf(tags, group.name)
                Dim order As Integer() = index.OrderBy(Function(i) y(i)).ToArray
                Dim slots As Double() = arrange(order, y, binWidth)
                Dim brush As Func(Of Object, String) = colors

                For k As Integer = 0 To order.Length - 1
                    Dim i As Integer = order(k)
                    Dim point As New PointF(
                        CSng(center(i) + slots(k) * binWidth),
                        CSng(stream.scale.TranslateY(y(i)))
                    )
                    Dim fill As Brush = brush(group.name).GetBrush
                    Dim diameter As Single = CSng(swarmSize * cex)

                    Call stream.g.FillEllipse(fill,
                                              CSng(point.X - diameter / 2),
                                              CSng(point.Y - diameter / 2),
                                              diameter,
                                              diameter)
                Next
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' arrange the observations of one group into the swarm slots
        ''' </summary>
        ''' <param name="order">the observation indexes, sorted by value</param>
        ''' <param name="y">the value data</param>
        ''' <param name="binWidth">the width of the categorical band</param>
        ''' <returns>the horizontal offset of each observation, in the units of the band</returns>
        Private Function arrange(order As Integer(), y As Double(), binWidth As Double) As Double()
            Dim n As Integer = order.Length
            Dim offsets As Double() = New Double(n - 1) {}

            If n = 0 Then Return offsets

            ' 每个观测占用的半宽，按数值间距折算到分类轴单位
            Dim used As New List(Of Tuple(Of Double, Double))
            Dim minGap As Double = 0.4

            For k As Integer = 0 To n - 1
                Dim placed As Boolean = False

                For slotIndex As Integer = 0 To n
                    Dim candidate As Double

                    Select Case direction.ToLower
                        Case "down"
                            candidate = -slotIndex / 2
                        Case Else
                            candidate = slotIndex / 2
                    End Select

                    Dim conflict As Boolean = False

                    For Each slot In used
                        If Math.Abs(slot.Item1 - candidate) < minGap Then
                            conflict = True
                            Exit For
                        End If
                    Next

                    If Not conflict Then
                        Call used.Add(Tuple.Create(candidate, y(order(k))))
                        offsets(k) = candidate
                        placed = True
                        Exit For
                    End If
                Next

                If Not placed Then
                    offsets(k) = 0
                End If
            Next

            Return offsets
        End Function

        Private Shared Function indexOf(tags As String(), name As String) As Integer()
            Dim buffer As New List(Of Integer)

            For i As Integer = 0 To tags.Length - 1
                If tags(i) = name Then Call buffer.Add(i)
            Next

            Return buffer.ToArray
        End Function

        Private Shared Function resolveBinWidth(stream As ggplotPipeline) As Double
            Dim x As d3js.scale.OrdinalScale = TryCast(stream.scale.X, d3js.scale.OrdinalScale)

            If x Is Nothing Then Return 1

            Return x.binWidth
        End Function

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As d3js.scale.OrdinalScale) As IggplotLegendElement
            Return Plot(stream)
        End Function
    End Class
