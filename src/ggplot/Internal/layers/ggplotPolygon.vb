Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports Microsoft.VisualBasic.Linq
Imports SMRUCC.Rsharp.Runtime.Vectorization

#If NET8_0_OR_GREATER Then
Imports Brush = Microsoft.VisualBasic.Imaging.Brush
Imports Brushes = Microsoft.VisualBasic.Imaging.Brushes
#Else
Imports Brush = System.Drawing.Brush
Imports Brushes = System.Drawing.Brushes
#End If

Namespace layers

    Public Class ggplotPolygon : Inherits ggplotLayer

        Public Property stroke As String

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
            Dim color As Brush = Brushes.Red

            For Each shape As NamedValue(Of Polygon2D) In polygons
                Call stream.g.FillPolygon(color, shape.Value.AsEnumerable.ToArray)
            Next

            Return Nothing
        End Function
    End Class
End Namespace