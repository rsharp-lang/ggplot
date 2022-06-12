Imports ggplot.colors
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports Microsoft.VisualBasic.Math.Quantile

Namespace layers

    Public MustInherit Class ggplotGroup : Inherits ggplotLayer

        Public Property groupWidth As Double = 0.5

        Protected Function getColors(stream As ggplotPipeline, groupNames As IEnumerable(Of String)) As Func(Of Object, String)
            If colorMap Is Nothing Then
                colorMap = ggplotColorMap.CreateColorMap("Paper", alpha, stream.ggplot.environment)
            End If

            Return colorMap.ColorHandler(stream.ggplot, groupNames.ToArray)
        End Function

        Public Shared Iterator Function getDataGroups(tags As String(), y As Double()) As IEnumerable(Of NamedCollection(Of Double))
            Dim data As New Dictionary(Of String, List(Of Double))

            For i As Integer = 0 To tags.Length - 1
                If Not data.ContainsKey(tags(i)) Then
                    Call data.Add(tags(i), New List(Of Double))
                End If

                Call data(tags(i)).Add(y(i))
            Next

            For Each group In data
                Yield New NamedCollection(Of Double) With {
                    .name = group.Key,
                    .value = group.Value.ToArray
                }
            Next
        End Function

        Protected Function getLabelPosY(group As IEnumerable(Of Double), y As DataScaler, Optional factor As Double = 1) As Double
            Dim groupVal As Double() = group.ToArray
            Dim quartile = groupVal.Quartile
            Dim outlier = groupVal.AsVector.Outlier(quartile)

            If Not outlier.outlier.IsNullOrEmpty Then
                quartile = outlier.normal.Quartile
            End If

            ' max
            Return y.TranslateY(quartile.range.Max * factor)
        End Function

        Protected Function getDataGroups(stream As ggplotPipeline) As IEnumerable(Of NamedCollection(Of Double))
            Dim tags As String() = stream.x
            Dim y As Double() = stream.y

            Return getDataGroups(tags, y)
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            If stream.scale.xscale = d3js.scale.scalers.linear Then
                Throw New NotImplementedException
            Else
                Return PlotOrdinal(stream, stream.scale.X)
            End If
        End Function

        Protected MustOverride Function PlotOrdinal(stream As ggplotPipeline, x As d3js.scale.OrdinalScale) As IggplotLegendElement

    End Class
End Namespace