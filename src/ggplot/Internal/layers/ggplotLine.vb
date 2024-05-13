#Region "Microsoft.VisualBasic::8242551e125dbbbc8076b0076b454116, src\ggplot\Internal\layers\ggplotLine.vb"

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

    '   Total Lines: 61
    '    Code Lines: 51
    ' Comment Lines: 0
    '   Blank Lines: 10
    '     File Size: 2.79 KB


    '     Class ggplotLine
    ' 
    '         Properties: line_width
    ' 
    '         Function: Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Plots
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Math.Interpolation
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    Public Class ggplotLine : Inherits ggplotLayer

        Public Property line_width As Single = 5

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim serial As SerialData
            Dim colors As String() = Nothing
            Dim legends As IggplotLegendElement = Nothing
            Dim ggplot As ggplot = stream.ggplot
            Dim g As IGraphics = stream.g

            If Not useCustomData Then
                Dim x = CLRVector.asFloat(stream.x)
                Dim y = stream.y
                Dim nsize As Integer = x.Length

                If useCustomColorMaps Then
                    colors = getColorSet(ggplot, g, nsize, LegendStyles.SolidLine, y, legends)
                ElseIf Not ggplot.base.reader.color Is Nothing Then
                    colors = ggplot.base.getColors(ggplot, legends, LegendStyles.SolidLine)
                End If

                serial = ggplotScatter.createSerialData(ggplot.base.reader.ToString, x, CLRVector.asFloat(y), colors, line_width, LegendStyles.SolidLine, colorMap)
            Else
                With Me.data
                    If useCustomColorMaps Then
                        colors = getColorSet(ggplot, g, .nsize, LegendStyles.SolidLine, .y, legends)
                    ElseIf Not ggplot.base.reader.color Is Nothing Then
                        colors = ggplot.base.getColors(ggplot, legends, LegendStyles.SolidLine)
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

                    serial = ggplotScatter.createSerialData(reader.ToString, .x.ToFloat, .y.ToFloat, colors, line_width, LegendStyles.SolidLine, colorMap)
                End With
            End If

            Call LinePlot2D.DrawLine(stream.g, stream.canvas, stream.scale, serial, Splines.None)

            Return legends
        End Function
    End Class
End Namespace
