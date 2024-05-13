#Region "Microsoft.VisualBasic::293893b2839678ac5f467c2a98143b55, src\ggplot\Internal\layers\ggplotScatter.vb"

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

    '   Total Lines: 131
    '    Code Lines: 114
    ' Comment Lines: 0
    '   Blank Lines: 17
    '     File Size: 5.57 KB


    '     Class ggplotScatter
    ' 
    '         Properties: shape, size, stroke
    ' 
    '         Function: createSerialData, GetSerialData, Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.InteropServices
Imports ggplot.colors
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Plots
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.Html
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    Public Class ggplotScatter : Inherits ggplotLayer

        Public Property shape As LegendStyles?
        Public Property size As Single
        Public Property stroke As String

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim legends As IggplotLegendElement = Nothing
            Dim serial As SerialData = GetSerialData(stream, legends)
            Dim brush = serial.BrushHandler

            Call Scatter2D.DrawScatter(
                g:=stream.g,
                scatter:=serial.pts,
                scaler:=stream.scale,
                fillPie:=True,
                shape:=serial.shape,
                pointSize:=serial.pointSize,
                getPointBrush:=brush,
                strokeCss:=CSS.Stroke.TryParse(stroke, Nothing)
            ) _
            .ToArray

            Return legends
        End Function

        Public Function GetSerialData(stream As ggplotPipeline, <Out> Optional ByRef legends As IggplotLegendElement = Nothing) As SerialData
            Dim colors As String() = Nothing
            Dim ggplot As ggplot = stream.ggplot
            Dim size As Single = If(ggplot.driver = Drivers.SVG, Me.size * stream.g.Dpi / 96, Me.size)

            If Not useCustomData Then
                Dim x = CLRVector.asFloat(stream.x)
                Dim y = CLRVector.asFloat(stream.y)
                Dim nsize As Integer = x.Length

                If useCustomColorMaps Then
                    Dim source As Double()
                    Dim reader = stream.ggplot.base.reader

                    If reader.color IsNot Nothing Then
                        source = CLRVector.asNumeric(reader.getColorSource(ggplot))
                    Else
                        source = CLRVector.asNumeric(y)
                    End If

                    colors = getColorSet(ggplot, stream.g, nsize, shape, source, legends)
                ElseIf Not ggplot.base.reader.color Is Nothing Then
                    colors = ggplot.base.getColors(
                        ggplot:=ggplot,
                        legends:=legends,
                        shape:=If(shape, LegendStyles.Circle)
                    )
                End If

                Return createSerialData(stream.defaultTitle, x, y, colors, size, shape, colorMap)
            Else
                With Me.data
                    If useCustomColorMaps Then
                        colors = getColorSet(ggplot, stream.g, .nsize, shape, .y, legends)
                    ElseIf Not ggplot.base.reader.color Is Nothing Then
                        colors = ggplot.base.getColors(ggplot, legends, shape)
                    Else
                        colors = (++ggplot.colors).Replicate(.nsize).ToArray
                        legends = New ggplotLegendElement With {
                            .legend = New LegendObject With {
                                .color = colors(Scan0),
                                .fontstyle = stream.theme.legendLabelCSS,
                                .style = LegendStyles.SolidLine,
                                .title = reader.getLegendLabel
                            }
                        }
                    End If

                    Return createSerialData(reader.ToString, .x.ToFloat, .y.ToFloat, colors, size, shape, colorMap)
                End With
            End If
        End Function

        Protected Friend Shared Function createSerialData(legend As String,
                                                          x As Single(),
                                                          y As Single(),
                                                          colors As String(),
                                                          size!,
                                                          shape As LegendStyles?,
                                                          colorMap As ggplotColorMap) As SerialData
            Dim color As Color

            If colors Is Nothing Then
                If Not colorMap Is Nothing Then
                    color = DirectCast(colorMap, ggplotColorLiteral).ToColor
                End If
            Else
                color = Nothing
            End If

            If color.IsEmpty Then
                color = Color.Black
            End If

            Return New SerialData() With {
                .color = color,
                .pointSize = size,
                .width = size,
                .shape = If(shape Is Nothing, LegendStyles.Circle, shape.Value),
                .title = legend,
                .pts = x _
                    .Select(Function(xi, i)
                                Return New PointData(xi, y(i)) With {
                                    .color = If(colors Is Nothing, Nothing, colors(i))
                                }
                            End Function) _
                    .ToArray
            }
        End Function
    End Class
End Namespace
