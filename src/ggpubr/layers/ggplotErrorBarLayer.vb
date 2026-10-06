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
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render

    ''' <summary>
    ''' The statistical summary layer of the paper figures, which draws the
    ''' mean(or median) of every group together with its error bar.
    '''
    ''' The size of the error bar is described by the function parameter:
    '''
    ''' + mean_se: the standard error of the mean
    ''' + mean_sd: the standard deviation
    ''' + mean_ci: the confidence interval of the mean
    ''' + sqrt_n: the standard error scaled by the square root of n
    ''' + a custom function which maps the group samples to the error size
    ''' </summary>
    Public Class ggplotErrorBarLayer : Inherits ggplotGroup

        Public Property errorType As String = "mean_se"
        Public Property fun As Func(Of Double(), Double)
        Public Property errorWidth As Double = 0.2
        Public Property pointSize As Double = 2
        Public Property addPoint As Boolean = True
        Public Property tag As String

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim tags As String() = stream.x
            Dim data = getDataGroups(stream).ToArray

            If data.Length = 0 Then Return Nothing

            Dim center As Double() = stream.TranslateX()
            Dim cap As Double = errorWidth / 2 * resolveBinWidth(stream)
            Dim colors As Func(Of Object, String) = getColors(stream, data.Select(Function(g) g.name))
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim pen As Pen = css.GetPen(Stroke.TryParse(stream.theme.lineStroke), allowNull:=False)

            For Each group As NamedCollection(Of Double) In data
                Dim index As Integer() = indexOf(tags, group.name)

                If index.Length = 0 Then Continue For

                Dim values As Double() = group.value
                Dim center1 As Double = values.Average
                Dim err As Double = resolveError(values)
                Dim x As Double = center(index(Scan0))
                Dim y As Double = stream.scale.TranslateY(center1)
                Dim stroke As String = colors(group.name)

                Call stream.g.DrawLine(pen, New PointF(CSng(x), CSng(stream.scale.TranslateY(center1 - err))),
                                           New PointF(CSng(x), CSng(stream.scale.TranslateY(center1 + err))))
                Call stream.g.DrawLine(pen, New PointF(CSng(x - cap), CSng(stream.scale.TranslateY(center1 - err))),
                                           New PointF(CSng(x + cap), CSng(stream.scale.TranslateY(center1 - err))))
                Call stream.g.DrawLine(pen, New PointF(CSng(x - cap), CSng(stream.scale.TranslateY(center1 + err))),
                                           New PointF(CSng(x + cap), CSng(stream.scale.TranslateY(center1 + err))))

                If addPoint Then
                    Dim radius As Single = CSng(pointSize)
                    Dim fill As Brush = stroke.GetBrush

                    Call stream.g.FillEllipse(fill, CSng(x - radius), CSng(y - radius), radius * 2, radius * 2)
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' resolve the error size of a group sample
        ''' </summary>
        Private Function resolveError(values As Double()) As Double
            If Not fun Is Nothing Then
                Return fun(values)
            End If

            Dim n As Double = values.Length

            If n < 2 Then Return 0

            Dim mean As Double = values.Average
            Dim variance As Double = values.Select(Function(v) (v - mean) * (v - mean)).Average

            Select Case errorType.Trim.ToLower.Replace("_", "")
                Case "meansd", "sd"
                    Return Math.Sqrt(variance)
                Case "meanci", "ci"
                    Return 1.96 * Math.Sqrt(variance / n)
                Case "sqrtn"
                    Return Math.Sqrt(variance / n)
                Case Else
                    Return Math.Sqrt(variance / n)
            End Select
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
