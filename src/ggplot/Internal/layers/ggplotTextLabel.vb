#Region "Microsoft.VisualBasic::e718d0e025000446a449af0e24ede8ad, src\ggplot\Internal\layers\ggplotTextLabel.vb"

    ' Author:
    ' 
    '       xieguigang (I@xieguigang.me)
    ' 
    ' Copyright (c) 2021 R# language
    ' 
    ' 
    ' MIT License
    ' 
    ' 
    ' Permission is hereby granted, free of charge, to any person obtaining a copy
    ' of this software and associated documentation files (the "Software"), to deal
    ' in the Software without restriction, including without limitation the rights
    ' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    ' copies of the Software, and to permit persons to whom the Software is
    ' furnished to do so, subject to the following conditions:
    ' 
    ' The above copyright notice and this permission notice shall be included in all
    ' copies or substantial portions of the Software.
    ' 
    ' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    ' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    ' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    ' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    ' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    ' SOFTWARE.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 161
    '    Code Lines: 126 (78.26%)
    ' Comment Lines: 12 (7.45%)
    '    - Xml Docs: 33.33%
    ' 
    '   Blank Lines: 23 (14.29%)
    '     File Size: 6.47 KB


    '     Class ggplotTextLabel
    ' 
    '         Properties: check_overlap, fontSize, isLabeler
    ' 
    '         Function: layoutLabels, Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.d3js.Layout
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Text.Nudge
Imports Microsoft.VisualBasic.Language.Vectorization
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render

#If NET48 Then
Imports Pen = System.Drawing.Pen
Imports Pens = System.Drawing.Pens
Imports Brush = System.Drawing.Brush
Imports Font = System.Drawing.Font
Imports Brushes = System.Drawing.Brushes
Imports SolidBrush = System.Drawing.SolidBrush
Imports DashStyle = System.Drawing.Drawing2D.DashStyle
Imports Image = System.Drawing.Image
Imports Bitmap = System.Drawing.Bitmap
Imports GraphicsPath = System.Drawing.Drawing2D.GraphicsPath
Imports LineCap = System.Drawing.Drawing2D.LineCap
Imports TextureBrush = System.Drawing.TextureBrush
#Else
Imports Pen = Microsoft.VisualBasic.Imaging.Pen
Imports Pens = Microsoft.VisualBasic.Imaging.Pens
Imports Brush = Microsoft.VisualBasic.Imaging.Brush
Imports Font = Microsoft.VisualBasic.Imaging.Font
Imports Brushes = Microsoft.VisualBasic.Imaging.Brushes
Imports SolidBrush = Microsoft.VisualBasic.Imaging.SolidBrush
Imports DashStyle = Microsoft.VisualBasic.Imaging.DashStyle
Imports Image = Microsoft.VisualBasic.Imaging.Image
Imports Bitmap = Microsoft.VisualBasic.Imaging.Bitmap
Imports GraphicsPath = Microsoft.VisualBasic.Imaging.GraphicsPath
Imports LineCap = Microsoft.VisualBasic.Imaging.LineCap
Imports TextureBrush = Microsoft.VisualBasic.Imaging.TextureBrush
#End If

Namespace layers

    Public Class ggplotTextLabel : Inherits ggplotLayer

        ''' <summary>
        ''' calling from the ``geom_label`` function?
        ''' </summary>
        ''' <returns></returns>
        Public Property isLabeler As Boolean
        Public Property check_overlap As Boolean = False
        Public Property fontSize As Single? = Nothing

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim legend As legendGroupElement = Nothing
            Dim labels As String()
            Dim ggplot As ggplot = stream.ggplot
            Dim g As IGraphics = stream.g
            Dim css As CSSEnvirnment = g.LoadEnvironment
            Dim labelStyle As Font = css.GetFont(CSSFont.TryParse(stream.theme.tagCSS))
            Dim x = stream.x
            Dim y = stream.y
            Dim scale As DataScaler = stream.scale

            If useCustomData Then
                labels = ggplot.getText(reader.label)
            Else
                labels = ggplot.getText(ggplot.base.reader.label)
            End If

            If Not fontSize Is Nothing Then
                labelStyle = New Font(
                    familyName:=labelStyle.Name,
                    emSize:=CSng(fontSize),
                    style:=labelStyle.Style
                )
            End If

            Dim anchors As Anchor() = Nothing

            x = stream.TranslateX
            y = y.Select(Function(yi) scale.TranslateY(yi)).ToArray

            For Each label As Label In layoutLabels(labels, x, y, g, labelStyle, stream.canvas, anchors, ggplot)
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
                                      box As GraphicsRegion,
                                      ByRef anchors As Anchor(),
                                      ggplot As ggplot) As Label()

            Dim labelList As Label() = labels _
                .Select(Function(label, i)
                            If label.StringEmpty Then
                                Return Nothing
                            End If

                            Dim size As SizeF = g.MeasureString(label, style)
                            Dim xi As Double = x(i) - size.Width / 2
                            Dim yi As Double = y(i) + 5

                            Return New Label(size) With {
                                .text = label,
                                .X = xi,
                                .Y = yi
                            }
                        End Function) _
                .ToArray

            anchors = x _
                .Select(Function(xi, i) New Anchor(xi, y(i), 5)) _
                .ToArray

            If Not which Is Nothing Then
                Dim i As BooleanVector = getFilter(ggplot)
                Dim ntest As Integer = i.Sum

                If ntest > 0 AndAlso labelList.Length <> i.Length Then
                    Throw New InvalidProgramException(
                        $"the label list size(n={labelList.Length}) is not matched with the filter vector size(m={i.Sum})!" &
                        If(labelList.Length > 0, "", " the label list is empty, please check of the label mapping in ggplot function!"))
                End If

                labelList = (New Vector(Of Label)(labelList))(i)
                anchors = (New Vector(Of Anchor)(anchors))(i)
            End If

            labelList = (From lb As Label
                         In labelList
                         Where Not lb Is Nothing).ToArray

            If check_overlap Then
                'Call d3js _
                '    .labeler _
                '    .Width(box.Width) _
                '    .Height(box.Height) _
                '    .Labels(labelList) _
                '    .WithOffset(New PointF(box.Left, box.Top)) _
                '    .Anchors(anchors) _
                '    .Start(nsweeps:=1000, showProgress:=False)
                Dim ax As New GraphicsTextHandle With {
                    .texts = labelList,
                    .canvas = box
                }

                Call ax.adjust_text()
            End If

            Return labelList
        End Function
    End Class
End Namespace
