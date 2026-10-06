Imports System.Drawing
Imports ggplot.colors
Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Plots.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Linq
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    ''' <summary>
    ''' geom_dotplot(): a dot plot which stacks the dots within the same
    ''' categorical bin along the value axis, so that the number of the
    ''' observations is represented by the length of the dot stack instead of
    ''' the bar height.
    '''
    ''' + binwidth: the width of the categorical bin
    ''' + binaxis: x or y, the axis which is binned
    ''' + stackdir: center, up, down or none, the stacking direction
    ''' + dotsize: the diameter of a single dot
    ''' </summary>
    Public Class ggplotDotplot : Inherits ggplotLayer

        Public Property binwidth As Double = 1
        Public Property binaxis As String = "x"
        Public Property stackdir As String = "center"
        Public Property direction As String = "y"
        Public Property dotsize As Double = 3

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot As ggplot = stream.ggplot
            Dim reader As ggplotReader = If(useCustomData, Me.reader, ggplot.base.reader)
            Dim source As Object = If(useCustomData, dataset, ggplot.data)
            Dim x As Double() = CLRVector.asNumeric(stream.x)
            Dim y As Double() = stream.y

            If x.Length = 0 OrElse y.Length <> x.Length Then Return Nothing

            If colorMap Is Nothing Then
                colorMap = ggplotColorMap.CreateColorMap(map:="Paper", alpha:=alpha, env:=ggplot.environment)
            End If

            Dim groups As String() = resolveGroups(reader, source, x.Length, ggplot.environment)
            Dim colors As Func(Of Object, String) = colorMap.ColorHandler(ggplot, groups.Distinct.ToArray)
            Dim radius As Single = CSng(dotsize / 2)

            For Each bucket As NamedCollection(Of Integer) In groupIndexes(groups)
                For Each bin As NamedCollection(Of Integer) In binIndexes(x, bucket.value)
                    Dim order As Integer() = bin.value _
                        .OrderBy(Function(i) y(i)) _
                        .ToArray
                    Dim offset As Double = 0

                    For j As Integer = 0 To order.Length - 1
                        Dim index As Integer = order(j)
                        Dim yi As Double = y(index)
                        Dim xi As Double = x(index)

                        Select Case stackdir.ToLower
                            Case "up"
                                yi += offset
                                offset += dotsize
                            Case "down"
                                yi -= offset
                                offset += dotsize
                            Case Else
                                ' center: 以当前点为中心向上下两侧均分展开
                                If j Mod 2 = 0 Then
                                    yi += offset
                                Else
                                    yi -= offset
                                End If

                                If j Mod 2 = 0 Then offset += dotsize
                        End Select

                        Dim point As New PointF(stream.scale.TranslateX(xi), stream.scale.TranslateY(yi))
                        Dim fill As Brush = colors(groups(index)).GetBrush
                        Dim diameter As Single = CSng(dotsize)

                        Call stream.g.FillEllipse(
                            fill,
                            CSng(point.X - radius),
                            CSng(point.Y - radius),
                            diameter,
                            diameter
                        )
                    Next
                Next
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' group the observation indexes by the group label
        ''' </summary>
        Private Shared Iterator Function groupIndexes(groups As String()) As IEnumerable(Of NamedCollection(Of Integer))
            Dim data As New Dictionary(Of String, List(Of Integer))

            For i As Integer = 0 To groups.Length - 1
                If Not data.ContainsKey(groups(i)) Then
                    Call data.Add(groups(i), New List(Of Integer))
                End If

                Call data(groups(i)).Add(i)
            Next

            For Each group In data
                Yield New NamedCollection(Of Integer) With {
                    .name = group.Key,
                    .value = group.Value.ToArray
                }
            Next
        End Function

        ''' <summary>
        ''' bin the observation indexes along the x axis
        ''' </summary>
        Private Iterator Function binIndexes(x As Double(), indexes As Integer()) As IEnumerable(Of NamedCollection(Of Integer))
            Dim lower As Double = x.Min
            Dim width As Double = If(binwidth > 0, binwidth, 1)
            Dim data As New Dictionary(Of Integer, List(Of Integer))

            For Each i As Integer In indexes
                Dim key As Integer = CInt(Math.Floor((x(i) - lower) / width))

                If Not data.ContainsKey(key) Then
                    Call data.Add(key, New List(Of Integer))
                End If

                Call data(key).Add(i)
            Next

            For Each bin In data
                Yield New NamedCollection(Of Integer) With {
                    .name = bin.Key.ToString,
                    .value = bin.Value.ToArray
                }
            Next
        End Function

        Private Shared Function resolveGroups(reader As ggplotReader,
                                              source As Object,
                                              nsize As Integer,
                                              env As SMRUCC.Rsharp.Runtime.Environment) As String()
            Dim mapping As String = DirectCast(reader.color, String)

            If String.IsNullOrEmpty(mapping) Then
                mapping = DirectCast(reader.[class], String)
            End If

            If String.IsNullOrEmpty(mapping) Then
                mapping = DirectCast(reader.group, String)
            End If

            If mapping Is Nothing Then
                Return Enumerable.Repeat(".", nsize).ToArray
            End If

            Return reader.getMapData(Of String)(source, mapping, env)
        End Function
    End Class
End Namespace