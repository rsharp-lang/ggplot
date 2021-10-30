Imports System.Drawing
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.d3js.Layout
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Language.Vectorization
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime

Namespace layers

    Public Class ggplotTextLabel : Inherits ggplotLayer

        ''' <summary>
        ''' calling from the ``geom_label`` function?
        ''' </summary>
        ''' <returns></returns>
        Public Property isLabeler As Boolean

        Public Overrides Function Plot(g As IGraphics,
                                       canvas As GraphicsRegion,
                                       baseData As ggplotData,
                                       x() As Double,
                                       y() As Double,
                                       scale As DataScaler,
                                       ggplot As ggplot,
                                       theme As Theme) As IggplotLegendElement

            Dim legend As legendGroupElement = Nothing
            Dim labels As String()
            Dim labelStyle As Font = CSSFont.TryParse(theme.tagCSS).GDIObject(g.Dpi)

            If useCustomData Then
                labels = ggplot.getText(reader.label)
            Else
                labels = ggplot.getText(ggplot.base.reader.label)
            End If

            Dim anchors As Anchor() = Nothing

            x = x.Select(Function(xi) scale.TranslateX(xi)).ToArray
            y = y.Select(Function(yi) scale.TranslateY(yi)).ToArray

            For Each label As Label In layoutLabels(labels, x, y, g, labelStyle, canvas.PlotRegion, anchors, ggplot)
                Call g.DrawString(label.text, labelStyle, Brushes.Black, label.location)
            Next

            If showLegend Then
                Return legend
            Else
                Return Nothing
            End If
        End Function

        Private Function layoutLabels(labels As String(),
                                      x As Double(), y As Double(),
                                      g As IGraphics,
                                      style As Font,
                                      box As Rectangle,
                                      ByRef anchors As Anchor(),
                                      ggplot As ggplot) As Label()

            Dim labelList As Label() = labels _
                .Select(Function(label, i)
                            Return New Label(g.MeasureString(label, style)) With {
                                .text = label,
                                .X = x(i),
                                .Y = y(i)
                            }
                        End Function) _
                .ToArray

            anchors = x _
                .Select(Function(xi, i) New Anchor(xi, y(i), 5)) _
                .ToArray

            If Not which Is Nothing Then
                Dim i As BooleanVector = getFilter(ggplot)

                labelList = (New Vector(Of Label)(labelList))(i)
                anchors = (New Vector(Of Anchor)(anchors))(i)
            End If

            Call d3js _
                .labeler _
                .Width(box.Width) _
                .Height(box.Height) _
                .Labels(labelList) _
                .WithOffset(New PointF(box.Left, box.Top)) _
                .Anchors(anchors) _
                .Start(nsweeps:=10, showProgress:=False)

            Return labelList
        End Function
    End Class
End Namespace