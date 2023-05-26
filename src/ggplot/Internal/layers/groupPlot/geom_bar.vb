Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    Public Class geom_bar : Inherits ggplotGroup

        Public Property stat As String
        Public Property position As String

        Public Overrides Function getYAxis(y() As Double, ggplot As ggplot) As axisMap

        End Function

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As OrdinalScale) As IggplotLegendElement
            Dim groupName As String = stream.ggplot.base.reader.color
            Dim ggplot As ggplot = stream.ggplot
            Dim legends As IggplotLegendElement = Nothing

            If TypeOf stream.ggplot.data Is dataframe Then
                Dim groupFactors As String() = CLRVector.asCharacter(DirectCast(stream.ggplot.data, dataframe)(groupName))
                Dim colors As String() = Nothing
                Dim nsize As Integer = stream.x.Length
                Dim y As Double() = stream.y

                If useCustomColorMaps Then
                    colors = getColorSet(ggplot, LegendStyles.Rectangle, groupFactors, legends)
                ElseIf Not ggplot.base.reader.color Is Nothing Then
                    colors = ggplot.base.getColors(ggplot, legends, LegendStyles.Rectangle)
                End If

            Else
                Throw New NotImplementedException
            End If

            Return legends
        End Function
    End Class
End Namespace