#Region "Microsoft.VisualBasic::eda7210996fbb0eb2b172b281537f75a, src\ggplot\Internal\render\g3d.vb"

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

    '   Total Lines: 91
    '    Code Lines: 75 (82.42%)
    ' Comment Lines: 4 (4.40%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 12 (13.19%)
    '     File Size: 4.22 KB


    '     Module g3d
    ' 
    '         Function: Camera, populateModels
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports ggplot.elements.legend
Imports ggplot.layers
Imports ggplot.layers.layer3d
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Plot3D.Device
Imports Microsoft.VisualBasic.Data.ChartPlots.Plot3D.Model
Imports Microsoft.VisualBasic.Emit.Delegates
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing3D
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render

Namespace render

    Module g3d

        <Extension>
        Public Function Camera(ggplot As ggplot, plotSize As Size) As Camera
            Dim cameraVal As Object = ggplot.args.getByName("camera")

            If cameraVal Is Nothing Then
                Return New Camera With {
                    .screen = plotSize,
                    .fov = 100000,
                    .viewDistance = -75,
                    .angleX = 31.5,
                    .angleY = 65,
                    .angleZ = 125
                }
            Else
                With DirectCast(cameraVal, Camera)
                    .screen = plotSize
                    Return cameraVal
                End With
            End If
        End Function

        Public Iterator Function populateModels(ggplot As ggplot,
                                                g As IGraphics,
                                                baseData As ggplotData,
                                                x() As Double,
                                                y() As Double,
                                                z() As Double,
                                                legends As List(Of IggplotLegendElement)) As IEnumerable(Of Element3D())

            Dim css As CSSEnvirnment = g.LoadEnvironment
            Dim max As Double = x.JoinIterates(y).JoinIterates(z).Max
            Dim min As Double = x.JoinIterates(y).JoinIterates(z).Min
            Dim theme As Theme = ggplot.ggplotTheme
            'Dim xTicks = x.Range.CreateAxisTicks
            'Dim yTicks = y.Range.CreateAxisTicks
            'Dim zTicks = z.Range.CreateAxisTicks
            Dim ticks = New DoubleRange(min, max).CreateAxisTicks
            Dim tickCss As String = CSSFont.TryParse(theme.axisTickCSS).SetFontColor(theme.mainTextColor).ToString

            ' 然后生成底部的网格
            Yield Grids.Grid1(css, ticks, ticks, (ticks(1) - ticks(0), ticks(1) - ticks(0)), ticks.Min, showTicks:=Not theme.axisTickCSS.StringEmpty, strokeCSS:=theme.gridStrokeX, tickCSS:=tickCss).ToArray
            Yield Grids.Grid2(css, ticks, ticks, (ticks(1) - ticks(0), ticks(1) - ticks(0)), ticks.Min, showTicks:=Not theme.axisTickCSS.StringEmpty, strokeCSS:=theme.gridStrokeX, tickCSS:=tickCss).ToArray
            Yield Grids.Grid3(css, ticks, ticks, (ticks(1) - ticks(0), ticks(1) - ticks(0)), ticks.Max, showTicks:=Not theme.axisTickCSS.StringEmpty, strokeCSS:=theme.gridStrokeX, tickCSS:=tickCss).ToArray

            Yield AxisDraw.Axis(
                css,
                xrange:=ticks, yrange:=ticks, zrange:=ticks,
                labelFontCss:=theme.axisLabelCSS,
                labels:=(ggplot.xlabel, ggplot.ylabel, ggplot.zlabel),
                strokeCSS:=theme.axisStroke,
                arrowFactor:="1,2",
                labelColorVal:=theme.mainTextColor
            )

            Dim ggplotLayers As New List(Of ggplotLayer)(ggplot.layers)

            For Each layer As ggplotLayer In ggplotLayers.ToArray
                If layer.GetType.ImplementInterface(Of Ilayer3d) Then
                    Call ggplotLayers.Remove(layer)

                    Yield DirectCast(layer, Ilayer3d) _
                        .populateModels(g, baseData, x, y, z, ggplot, theme, legends) _
                        .ToArray
                End If
            Next
        End Function

    End Module
End Namespace
