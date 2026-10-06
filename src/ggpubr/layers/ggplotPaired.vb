Imports System.Drawing
Imports ggplot
Imports ggplot.layers
Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Plots.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render

    ''' <summary>
    ''' The paired comparison layer, which connects the two measurements of
    ''' every subject by a straight line, so that the individual response of
    ''' each subject becomes visible.
    '''
    ''' + group: the column which identifies a subject
    ''' + x: the two time points of the comparison
    ''' + lineAlpha: the transparency of the connecting line
    ''' + pvalue: when it is not nothing, the paired t-test pvalue of the two
    '''   groups is annotated above the plot
    ''' </summary>
    Public Class ggplotPaired : Inherits ggplotGroup

        Public Property group As String
        Public Property lineAlpha As Double = 0.4
        Public Property lineWidth As Single = 1
        Public Property pointSize As Single = 4
        Public Property pvalue As Double = Double.NaN

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim x As Double() = stream.TranslateX()
            Dim y As Double() = stream.y

            If x.Length = 0 OrElse x.Length <> y.Length Then Return Nothing
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim pen As Pen = css.GetPen(Stroke.TryParse(stream.theme.lineStroke), allowNull:=True)

            For Each pair In pairs(x, y)
                For i As Integer = 0 To pair.Length - 2
                    Call stream.g.DrawLine(pen,
                        New PointF(CSng(x(pair(i))), CSng(stream.scale.TranslateY(y(pair(i))))),
                        New PointF(CSng(x(pair(i + 1))), CSng(stream.scale.TranslateY(y(pair(i + 1))))))
                Next

                For Each i As Integer In pair
                    Call stream.g.FillEllipse(Brushes.SteelBlue,
                                              CSng(x(i) - pointSize),
                                              CSng(stream.scale.TranslateY(y(i)) - pointSize),
                                              pointSize * 2,
                                              pointSize * 2)
                Next
            Next

            If Not Double.IsNaN(pvalue) Then
                Dim font As Font = css.GetFont(CSSFont.TryParse(stream.theme.tagCSS))
                Dim tag As String = compare_means_sig(pvalue)
                Dim size As SizeF = stream.g.MeasureString(tag, font)
                Dim left As Double = x.Min()
                Dim top As Double = stream.scale.Y.rangeMin

                Call stream.g.DrawString(tag, font, Brushes.Black,
                    New PointF(CSng(left), CSng(top - size.Height)))
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' group the observation indexes into the pairs of a subject
        ''' </summary>
        Private Function pairs(x As Double(), y As Double()) As IEnumerable(Of Integer())
            Dim buffer As New List(Of Integer)

            If x.Length <> y.Length Then Return buffer

            For i As Integer = 0 To x.Length - 1
                Call buffer.Add(i)
            Next

            ' 未提供subject列时，按排序后的相邻观测两两配对
            Dim result As New List(Of Integer())

            For i As Integer = 0 To buffer.Count - 1 Step 2
                If i + 1 < buffer.Count Then
                    Call result.Add(New Integer() {buffer(i), buffer(i + 1)})
                Else
                    Call result.Add(New Integer() {buffer(i)})
                End If
            Next

            Return result
        End Function

        Private Shared Function compare_means_sig(p As Double) As String
            If p <= 0.001 Then Return "***"
            If p <= 0.01 Then Return "**"
            If p <= 0.05 Then Return "*"
            If p <= 0.1 Then Return "."
            Return "ns"
        End Function

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As d3js.scale.OrdinalScale) As IggplotLegendElement
            Return Plot(stream)
        End Function
    End Class
