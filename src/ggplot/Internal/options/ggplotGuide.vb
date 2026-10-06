Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.Plots.Plot3D.Legend
Imports Microsoft.VisualBasic.Linq

Namespace options

    ''' <summary>
    ''' The legend guide of a ggplot plot.
    '''
    ''' + guide_legend(): merge the legends of all the layers into a single legend
    ''' + guide_none(): drop the legend of the plot
    '''
    ''' The guide also controls the layout of the merged legend: the merge
    ''' behavior, the direction, the number of the rows/columns, the title
    ''' override and the item order.
    ''' </summary>
    Public MustInherit Class ggplotGuide : Inherits ggplotOption

        Public Property name As String = "legend"
        Public Property merge As Boolean = True
        Public Property direction As String = "vertical"
        Public Property nrow As Integer
        Public Property ncol As Integer
        Public Property title As String
        Public Property reverse As Boolean = False

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.guide = Me

            Return ggplot
        End Function

        ''' <summary>
        ''' organize the legends which are collected from the plot layers
        ''' </summary>
        ''' <param name="legends">the legends collected from the plot layers</param>
        ''' <returns>the legends which should be drawn</returns>
        Public Overridable Function Arrange(legends As IEnumerable(Of IggplotLegendElement)) As IEnumerable(Of IggplotLegendElement)
            Dim all As IggplotLegendElement() = legends _
                .Where(Function(l) Not l Is Nothing) _
                .ToArray

            If all.Length = 0 Then Return all

            If Not merge OrElse Not all.All(Function(l) TypeOf l Is legendGroupElement) Then
                Return all
            End If

            Dim items As LegendObject() = all _
                .SelectMany(Function(l) DirectCast(l, legendGroupElement).legends) _
                .ToArray

            If items.IsNullOrEmpty Then Return all

            If reverse Then
                items = items.Reverse.ToArray
            End If

            Dim max As Integer = nrow * ncol

            If max > 0 AndAlso items.Length > max Then
                items = items.Take(max).ToArray
            End If

            If Not title Is Nothing Then
                For Each item As LegendObject In items
                    item.title = title
                Next
            End If

            Return New IggplotLegendElement() {New legendGroupElement With {.legends = items}}
        End Function
    End Class

    ''' <summary>
    ''' guide_legend(): the default legend guide, merges the legends of all
    ''' the layers into a single legend.
    ''' </summary>
    Public Class ggGuideLegend : Inherits ggplotGuide

        Public Sub New()
            name = "legend"
        End Sub
    End Class

    ''' <summary>
    ''' guide_none(): drops the legend of the plot
    ''' </summary>
    Public Class ggGuideNone : Inherits ggplotGuide

        Public Sub New()
            name = "none"
            merge = False
        End Sub

        Public Overrides Function Arrange(legends As IEnumerable(Of IggplotLegendElement)) As IEnumerable(Of IggplotLegendElement)
            Return New IggplotLegendElement() {}
        End Function
    End Class
End Namespace
