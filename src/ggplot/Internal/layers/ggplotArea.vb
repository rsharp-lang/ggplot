Imports System.Drawing
Imports ggplot.colors
Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.Plots.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    ''' <summary>
    ''' The implementation layer of both the ``geom_area()`` and the ``geom_ribbon()``.
    '''
    ''' + geom_area(): fills the area between the y values and a baseline
    ''' + geom_ribbon(): fills the area between the ymin and the ymax values
    '''
    ''' Both of them are drawn as a filled polygon which is closed by the
    ''' baseline(area) or by the reversed upper bound(ribbon).
    ''' </summary>
    Public Class ggplotArea : Inherits ggplotLayer

        ''' <summary>
        ''' the baseline of the area chart, value could be
        '''
        ''' 1. zero: the y=0 baseline
        ''' 2. min: the minimum y value of the plot data
        ''' 3. a numeric value
        ''' </summary>
        Public Property baseline As Object = "zero"
        ''' <summary>
        ''' draw the outline of the area/ribbon
        ''' </summary>
        Public Property outline As Boolean = False
        Public Property line_width As Single = 1

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot As ggplot = stream.ggplot
            Dim g As IGraphics = stream.g
            Dim css As CSSEnvirnment = g.LoadEnvironment
            Dim reader As ggplotReader = If(useCustomData, Me.reader, ggplot.base.reader)
            Dim source As Object = If(useCustomData, dataset, ggplot.data)
            Dim x As Double() = CLRVector.asNumeric(stream.x)
            Dim y As Double() = stream.y

            If x.Length = 0 OrElse y.Length <> x.Length Then
                Return Nothing
            End If

            Dim groups As String() = resolveGroups(reader, source, x.Length, ggplot.environment)
            Dim lower As Double() = resolveBound(reader.ymin, source, x.Length, ggplot.environment)
            Dim upper As Double() = resolveBound(reader.ymax, source, x.Length, ggplot.environment)
            Dim isRibbon As Boolean = lower IsNot Nothing AndAlso upper IsNot Nothing
            Dim base_y As Double = resolveBaseline(y)

            If colorMap Is Nothing Then
                colorMap = ggplotColorMap.CreateColorMap(map:="Paper", alpha:=alpha, env:=ggplot.environment)
            End If

            Dim colors As Func(Of Object, String) = colorMap.ColorHandler(ggplot, groups.Distinct.ToArray)
            Dim pen As Pen = css.GetPen(Stroke.TryParse(stream.theme.lineStroke), allowNull:=True)

            For Each group As NamedCollection(Of Integer) In indexGroups(groups)
                Dim order As Integer() = group.value.OrderBy(Function(i) x(i)).ToArray
                Dim polygon As PointF()

                If isRibbon Then
                    ' 上界从左到右，下界从右到左，构成一个闭合的带状多边形
                    polygon = order _
                        .Select(Function(i) New PointF(x(i), upper(i))) _
                        .Concat(order.Reverse().Select(Function(i) New PointF(x(i), lower(i)))) _
                        .Select(Function(pt) stream.scale.Translate(pt)) _
                        .ToArray
                Else
                    polygon = order _
                        .Select(Function(i) New PointF(x(i), y(i))) _
                        .Concat({New PointF(x(order.Last), base_y), New PointF(x(order.First), base_y)}) _
                        .Select(Function(pt) stream.scale.Translate(pt)) _
                        .ToArray
                End If

                If polygon.Length < 3 Then Continue For

                Dim fill As Brush = colors(group.name).GetBrush

                If TypeOf fill Is SolidBrush Then
                    fill = New SolidBrush(DirectCast(fill, SolidBrush).Color.Alpha(alpha * 255))
                End If

                Call g.FillPolygon(fill, polygon)

                If outline AndAlso pen IsNot Nothing Then
                    Call g.DrawPolygon(pen, polygon)
                End If
            Next

            Return Nothing
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

        ''' <summary>
        ''' resolve the group indexes of each group label
        ''' </summary>
        Private Shared Iterator Function indexGroups(groups As String()) As IEnumerable(Of NamedCollection(Of Integer))
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
        ''' resolve a numeric bound from the aes mapping
        ''' </summary>
        ''' <param name="mapping">the column name of the bound mapping</param>
        ''' <param name="source">the plot data source</param>
        ''' <param name="nsize">the expected size of the bound data</param>
        ''' <param name="env"></param>
        ''' <returns>nothing when the mapping is not defined or the size is not matched</returns>
        Private Shared Function resolveBound(mapping As String,
                                             source As Object,
                                             nsize As Integer,
                                             env As SMRUCC.Rsharp.Runtime.Environment) As Double()
            If mapping Is Nothing Then Return Nothing

            Dim reader As New ggplotReader With {.y = mapping}
            Dim vec As Double() = CLRVector.asNumeric(reader.getMapData(Of Object)(source, mapping, env))

            If vec.Length <> nsize Then
                Return Nothing
            End If

            Return vec
        End Function

        ''' <summary>
        ''' resolve the numeric baseline of the area chart
        ''' </summary>
        Private Function resolveBaseline(y As Double()) As Double
            If TypeOf baseline Is String Then
                Select Case DirectCast(baseline, String).ToLower
                    Case "zero" : Return 0
                    Case Else : Return y.Min
                End Select
            Else
                Return CDbl(baseline)
            End If
        End Function
    End Class
End Namespace