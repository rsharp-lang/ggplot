#Region "Microsoft.VisualBasic::26437d02bd67083568f5b753c6a20d79, src\ggplot\Internal\layers\scatter\ggplotSegments.vb"

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

'   Total Lines: 34
'    Code Lines: 28 (82.35%)
' Comment Lines: 0 (0.00%)
'    - Xml Docs: 0.00%
' 
'   Blank Lines: 6 (17.65%)
'     File Size: 1.12 KB


'     Class ggplotSegments
' 
'         Function: Plot
' 
' 
' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    Public Class ggplotSegments : Inherits ggplotScatterLayer

        Public Property thickness As Double = 3

        Protected Friend Overrides ReadOnly Property useCustomData As Boolean
            Get
                If reader Is Nothing Then Return False
                Return True
            End Get
        End Property

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim xend As Single(), yend As Single()
            Dim x As Double() = CLRVector.asNumeric(stream.x)
            Dim y As Double() = CLRVector.asNumeric(stream.y)
            Dim ggplotData As ggplotData = stream.baseData
            Dim ggplot As ggplot = stream.ggplot
            Dim colors As String()
            Dim legends As IggplotLegendElement = Nothing

            If useCustomData Then
                xend = data.xend.ToFloat
                yend = data.yend.ToFloat
            Else
                xend = CLRVector.asFloat(ggplotData.xend)
                yend = CLRVector.asFloat(ggplotData.yend)
            End If

            If useCustomColorMaps Then
                colors = getColorSet(stream, x.Length, LegendStyles.Circle, y, legends)
            ElseIf Not ggplot.base.reader.color Is Nothing Then
                colors = ggplot.base.getColors(ggplot, legends, LegendStyles.Circle)

                If Not colors.IsNullOrEmpty Then
                    If colors.All(Function(c) c.IsInteger) Then
                        Call "all of the fill color is integer, your data class id in integer format maybe mis-interpreted as color value!".warning
                    End If
                End If
            Else
                colors = (++ggplot.colors).Replicate(x.Length).ToArray
                legends = New ggplotLegendElement With {
                    .legend = New LegendObject With {
                        .color = colors(Scan0),
                        .fontstyle = stream.theme.legendLabelCSS,
                        .style = LegendStyles.SolidLine,
                        .title = reader.getLegendLabel
                    }
                }
            End If

            For i As Integer = 0 To x.Length - 1
                Dim pt As PointF = stream.scale.Translate(x(i), y(i))
                Dim xi As Double = pt.X
                Dim yi As Double = pt.Y
                Dim ax As Double = xend(i)
                Dim ay As Double = yend(i)
                Dim color As Color = colors(i).TranslateColor

                Call RenderShape.RenderForceVector(stream.g,
                                                   New PointF(xi, yi),
                                                   New PointF(ax, ay),
                                                   color,
                                                   scale:=minCell,
                                                   w:=thickness)
            Next

            If showLegend Then
                Return legends
            Else
                Return Nothing
            End If
        End Function
    End Class
End Namespace
