Imports System.Drawing
Imports ggplot.colors
Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.Plots.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    ''' <summary>
    ''' The implementation layer of both the geom_errorbar() (vertical) and
    ''' the geom_errorbarh() (horizontal).
    '''
    ''' The error bar is described by the ymin/ymax aes mappings for the
    ''' vertical orientation, and the xmin/xmax mappings for the horizontal
    ''' one. When the bounds are not mapped explicitly, they are derived from
    ''' the y/x value and the height/width of the symmetric error.
    ''' </summary>
    Public Class ggplotErrorbar : Inherits ggplotLayer

        Public Property width As Double = 0.2
        Public Property height As Double = 0.1
        Public Property orientation As String = "vertical"
        Public Property line_width As Single = 1
        Public Property lineend As String = "butt"

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot As ggplot = stream.ggplot
            Dim g As IGraphics = stream.g
            Dim css As CSSEnvirnment = g.LoadEnvironment
            Dim reader As ggplotReader = If(useCustomData, Me.reader, ggplot.base.reader)
            Dim source As Object = If(useCustomData, dataset, ggplot.data)
            Dim center As Double() = stream.TranslateX()

            If center.Length = 0 Then Return Nothing

            If colorMap Is Nothing Then
                colorMap = ggplotColorMap.CreateColorMap(map:="black", alpha:=alpha, env:=ggplot.environment)
            End If

            Dim pen As Pen = css.GetPen(Stroke.TryParse(stream.theme.lineStroke), allowNull:=True)

            If orientation.TextEquals("horizontal") Then
                Call plotHorizontal(stream, reader, source, pen, center)
            Else
                Call plotVertical(stream, reader, source, pen, center)
            End If

            Return Nothing
        End Function

        Private Sub plotVertical(stream As ggplotPipeline,
                                 reader As ggplotReader,
                                 source As Object,
                                 pen As Pen,
                                 center As Double())
            Dim y As Double() = stream.y
            Dim lower As Double() = resolveBound(reader.ymin, source, y.Length, stream.ggplot.environment)
            Dim upper As Double() = resolveBound(reader.ymax, source, y.Length, stream.ggplot.environment)

            If lower Is Nothing Then lower = y.Select(Function(v) v - height).ToArray
            If upper Is Nothing Then upper = y.Select(Function(v) v + height).ToArray

            Dim cap As Double = resolveCap(stream)

            For i As Integer = 0 To center.Length - 1
                Dim y0 As Double = stream.scale.TranslateY(lower(i))
                Dim y1 As Double = stream.scale.TranslateY(upper(i))
                Dim xc As Double = center(i)

                Call stream.g.DrawLine(pen, New PointF(xc, y0), New PointF(xc, y1))
                Call stream.g.DrawLine(pen, New PointF(xc - cap, y0), New PointF(xc + cap, y0))
                Call stream.g.DrawLine(pen, New PointF(xc - cap, y1), New PointF(xc + cap, y1))
            Next
        End Sub

        Private Sub plotHorizontal(stream As ggplotPipeline,
                                   reader As ggplotReader,
                                   source As Object,
                                   pen As Pen,
                                   center As Double())
            Dim x As Double() = CLRVector.asNumeric(stream.x)
            Dim lower As Double() = resolveBound(reader.xmin, source, x.Length, stream.ggplot.environment)
            Dim upper As Double() = resolveBound(reader.xmax, source, x.Length, stream.ggplot.environment)

            If lower Is Nothing Then lower = x.Select(Function(v) v - height).ToArray
            If upper Is Nothing Then upper = x.Select(Function(v) v + height).ToArray

            Dim cap As Double = resolveCap(stream)

            For i As Integer = 0 To center.Length - 1
                Dim x0 As Double = stream.scale.TranslateX(lower(i))
                Dim x1 As Double = stream.scale.TranslateX(upper(i))
                Dim yc As Double = center(i)

                Call stream.g.DrawLine(pen, New PointF(x0, yc), New PointF(x1, yc))
                Call stream.g.DrawLine(pen, New PointF(x0, yc - cap), New PointF(x0, yc + cap))
                Call stream.g.DrawLine(pen, New PointF(x1, yc - cap), New PointF(x1, yc + cap))
            Next
        End Sub

        Private Function resolveCap(stream As ggplotPipeline) As Double
            If stream.scale.xscale = scalers.linear Then
                Return (stream.scale.X.rangeMax - stream.scale.X.rangeMin) * width / 2
            End If

            Return DirectCast(stream.scale.X, OrdinalScale).binWidth * width / 2
        End Function

        Private Shared Function resolveBound(mapping As String,
                                             source As Object,
                                             nsize As Integer,
                                             env As SMRUCC.Rsharp.Runtime.Environment) As Double()
            If mapping Is Nothing Then Return Nothing

            Dim reader As New ggplotReader With {.y = mapping}
            Dim vec As Double() = CLRVector.asNumeric(reader.getMapData(Of Object)(source, mapping, env))

            If vec.Length <> nsize Then Return Nothing

            Return vec
        End Function
    End Class
End Namespace