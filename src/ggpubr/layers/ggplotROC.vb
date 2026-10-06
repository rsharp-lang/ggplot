Imports System.Drawing
Imports ggplot
Imports ggplot.layers
Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Plots.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports SMRUCC.Rsharp.Runtime.Vectorization

''' <summary>
''' The ROC curve layer, which plots the true positive rate against the false
''' positive rate of a binary classifier, and annotates the area under the
''' curve.
'''
''' The curve is computed over all the distinct score thresholds, and the point
''' count is capped by a sub-sampling so that a large dataset stays readable.
''' </summary>
Public Class ggplotROC : Inherits ggplotLayer

    Public Property score As Double()
    Public Property label As Boolean()
    Public Property group As String()
    Public Property auc As Boolean = True
    Public Property maxPoints As Integer = 1000
    Public Property color As String = "steelblue"
    Public Property lineWidth As Single = 2

    Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
        If score Is Nothing OrElse label Is Nothing Then Return Nothing
        If score.Length <> label.Length OrElse score.Length = 0 Then Return Nothing

        Dim css As CSSEnvirnment = stream.g.LoadEnvironment
        Dim font As Font = css.GetFont(CSSFont.TryParse(stream.theme.tagCSS))
        Dim groups As String()

        If group Is Nothing Then
            groups = Enumerable.Repeat(".", score.Length).ToArray
        Else
            groups = CLRVector.asCharacter(group)
        End If

        Dim left As Single = stream.scale.X.rangeMin
        Dim right As Single = stream.scale.X.rangeMax
        Dim bottom As Single = stream.scale.Y.rangeMax
        Dim top As Single = stream.scale.Y.rangeMin
        Dim box As New RectangleF(left, top, right - left, bottom - top)

        For Each g In groups.Distinct
            Dim index As Integer() = Enumerable.Range(0, score.Length).Where(Function(i) groups(i) = g).ToArray
            Dim points As PointF() = curve(score, label, index)
            Dim pen As Pen = css.GetPen(Stroke.TryParse($"{color}; stroke-width: {lineWidth}"), allowNull:=False)

            For i As Integer = 1 To points.Length - 1
                Call stream.g.DrawLine(pen, points(i - 1), points(i))
            Next

            If auc AndAlso points.Length > 1 Then
                Dim area As Double = trapezoid(points)
                Dim tag As String = $"{g}: AUC={area.ToString("F3")}"
                Dim size As SizeF = stream.g.MeasureString(tag, font)

                Call stream.g.DrawString(tag, font, Brushes.Black,
                    New PointF(CSng(box.Right - size.Width - 8), CSng(box.Top + 8)))
            End If
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' compute the ROC curve of a group
    ''' </summary>
    Private Function curve(score As Double(), label As Boolean(), index As Integer()) As PointF()
        Dim thresholds As Double() = score _
            .Where(Function(v) Not Double.IsNaN(v)) _
            .Distinct() _
            .OrderByDescending(Function(v) v) _
            .ToArray()
        Dim positives As Integer = index.Count(Function(i) label(i))
        Dim negatives As Integer = index.Length - positives
        Dim result As New List(Of PointF)

        If positives = 0 OrElse negatives = 0 Then Return result.ToArray

        Dim stride As Integer = Math.Max(1, thresholds.Length \ Math.Max(maxPoints, 2))

        For t As Integer = 0 To thresholds.Length - 1
            If t Mod stride <> 0 AndAlso t <> thresholds.Length - 1 Then Continue For

            Dim tp As Integer = 0
            Dim fp As Integer = 0

            For Each i As Integer In index
                If score(i) >= thresholds(t) Then
                    If label(i) Then
                        tp += 1
                    Else
                        fp += 1
                    End If
                End If
            Next

            Call result.Add(New PointF(
                CSng(fp / negatives),
                CSng(tp / positives)
            ))
        Next

        Call result.Insert(0, New PointF(0, 0))

        Return result.ToArray()
    End Function

    Private Shared Function trapezoid(points As PointF()) As Double
        Dim area As Double = 0

        For i As Integer = 1 To points.Length - 1
            area += (points(i - 1).X + points(i).X) / 2 * (points(i).Y - points(i - 1).Y)
        Next

        Return area
    End Function
End Class
