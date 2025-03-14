#Region "Microsoft.VisualBasic::76a70c82a3dca1875b32e0e5bf36f048, src\ggplot\Internal\layers\ggplotABLine.vb"

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

    '   Total Lines: 80
    '    Code Lines: 63 (78.75%)
    ' Comment Lines: 2 (2.50%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 15 (18.75%)
    '     File Size: 2.70 KB


    '     Class ggplotABLine
    ' 
    '         Properties: abline
    ' 
    '         Function: constraint, initDataSet, Plot, translateOrdinal
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Shapes
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render

Namespace layers

    Public Class ggplotABLine : Inherits ggplotLayer

        Public Property abline As Line

        Protected Friend Overrides Function initDataSet(ggplot As ggplot, baseData As ggplotData) As ggplotData
            data = New ggplotData With {
                .x = axisMap.FromNumeric(New Double() {abline.A.X, abline.B.X}),
                .y = axisMap.FromNumeric(New Double() {abline.A.Y, abline.B.Y})
            }

            Return data
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim scale As DataScaler = stream.scale
            Dim a As PointF = constraint(abline.A, scale)
            Dim b As PointF = constraint(abline.B, scale)
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment

            If scale.xscale <> scalers.ordinal Then
                a = scale.Translate(a)
                b = scale.Translate(b)
            Else
                a = translateOrdinal(a, scale)
                b = translateOrdinal(b, scale)
            End If

            Call stream.g.DrawLine(css.GetPen(abline.Stroke), a, b)

            Return Nothing
        End Function

        Private Shared Function translateOrdinal(a As PointF, scale As DataScaler) As PointF
            If a.X < Single.MinValue Then
                a = New PointF With {.X = scale.X.rangeMin, .Y = scale.TranslateY(a.Y)}
            ElseIf a.X > Single.MaxValue Then
                a = New PointF With {.X = scale.X.rangeMax, .Y = scale.TranslateY(a.Y)}
            Else
                ' 直接是绘图数据了？
                ' 不做任何转换
            End If

            Return a
        End Function

        Private Shared Function constraint(pf As PointF, scale As DataScaler) As PointF
            Dim x, y As Single

            If scale.xscale <> scalers.ordinal Then
                If pf.X < Single.MinValue Then
                    x = scale.xmin
                ElseIf pf.X > Single.MaxValue Then
                    x = scale.xmax
                Else
                    x = pf.X
                End If
            Else
                x = pf.X
            End If

            If pf.Y < Single.MinValue Then
                y = scale.ymin
            ElseIf pf.Y > Single.MaxValue Then
                y = scale.ymax
            Else
                y = pf.Y
            End If

            Return New PointF(x, y)
        End Function
    End Class
End Namespace
