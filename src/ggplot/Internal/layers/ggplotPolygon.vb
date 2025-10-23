Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports SMRUCC.Rsharp.Runtime.Vectorization

#If NET8_0_OR_GREATER Then
Imports Brush = Microsoft.VisualBasic.Imaging.Brush
Imports Brushes = Microsoft.VisualBasic.Imaging.Brushes
Imports Pen = Microsoft.VisualBasic.Imaging.Pen
#Else
Imports Brush = System.Drawing.Brush
Imports Brushes = System.Drawing.Brushes
Imports Pen = System.Drawing.Pen
#End If

Namespace layers

    Public Class ggplotPolygon : Inherits ggplotLayer

        Public Property stroke As Stroke

        Protected Friend Overrides ReadOnly Property useCustomData As Boolean
            Get
                If reader Is Nothing OrElse reader.group Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim reader As ggplotReader = Nothing
            Dim ggplot As ggplot = stream.ggplot

            If useCustomData Then
                reader = Me.reader
            Else
                reader = stream.ggplot.base.reader
            End If

            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim group As String() = reader.getMapData(Of String)(If(dataset, ggplot.data), reader.group, ggplot.environment)
            Dim x As Double() = CLRVector.asNumeric(stream.x)
            Dim y As Double() = CLRVector.asNumeric(stream.y)
            Dim polygons As NamedValue(Of Polygon2D)() = group _
                .Select(Function(group_id, i) (group_id, xi:=x(i), yi:=y(i))) _
                .GroupBy(Function(a) a.group_id) _
                .Select(Function(poly)
                            Dim vx As Double() = poly.Select(Function(a) a.xi).ToArray
                            Dim vy As Double() = poly.Select(Function(a) a.yi).ToArray
                            Dim shape As New Polygon2D(vx, vy)

                            Return New NamedValue(Of Polygon2D)(poly.Key, shape)
                        End Function) _
                .ToArray
            Dim color As Brush = New SolidBrush(System.Drawing.Color.Red.Alpha(alpha * 255))
            Dim line As Pen = css.GetPen(stroke, allowNull:=True)

            For Each shape As NamedValue(Of Polygon2D) In polygons
                Dim points As PointF() = shape.Value.AsEnumerable.ToArray

                Call stream.g.FillPolygon(color, points)

                If line IsNot Nothing Then
                    Call stream.g.DrawPolygon(line, points)
                End If
            Next

            Return Nothing
        End Function
    End Class
End Namespace