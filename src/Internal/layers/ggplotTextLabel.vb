#Region "Microsoft.VisualBasic::efaca294bbd2eda39567ab58cfbfcba4, src\Internal\layers\ggplotTextLabel.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    '     Class ggplotTextLabel
    ' 
    '         Properties: isLabeler
    ' 
    '         Function: layoutLabels, Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.d3js.Layout
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Language.Vectorization
Imports Microsoft.VisualBasic.MIME.Html.CSS

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
