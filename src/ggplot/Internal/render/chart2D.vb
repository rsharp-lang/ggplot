#Region "Microsoft.VisualBasic::3cde93000ddff88e12e480b96fc71bf2, src\ggplot\Internal\render\chart2D.vb"

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

'   Total Lines: 121
'    Code Lines: 108 (89.26%)
' Comment Lines: 0 (0.00%)
'    - Xml Docs: 0.00%
' 
'   Blank Lines: 13 (10.74%)
'     File Size: 4.71 KB


'     Module chart2D
' 
'         Sub: plot2D, reverse
' 
' 
' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements
Imports ggplot.elements.legend
Imports ggplot.layers
Imports ggplot.render
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace render

    Module chart2D

        Private Sub reverse(ByRef vec As Double())
            Dim max As Double = vec.Max
            Dim min As Double = vec.Min

            For i As Integer = 0 To vec.Length - 1
                vec(i) = max - vec(i) + min
            Next
        End Sub

        Public Sub plot2D(ggplot As ggplot, baseData As ggplotData, ByRef g As IGraphics, canvas As GraphicsRegion)
            ' 20250106
            ' layers needs to be initialzed at first!
            Dim layers As New Queue(Of ggplotLayer)(collection:=ggplotAdapter.getLayers(ggplot, baseData))
            Dim x As axisMap = ggplotAdapter.getXAxis(layers, baseData)
            Dim y As axisMap = ggplotAdapter.getYAxis(layers, baseData)
            Dim reverse_y As Boolean = ggplot.args.getValue("scale_y_reverse", env:=ggplot.environment, [default]:=False)
            Dim scale As DataScaler
            Dim xAxis As Array
            Dim theme As Theme = ggplot.ggplotTheme
            Dim css As CSSEnvirnment = g.LoadEnvironment
            Dim plotRegion As Rectangle = canvas.PlotRegion(css)
            Dim fixedRange As Boolean = True

            For Each layer As ggplotLayer In layers.AsEnumerable
                If TypeOf layer Is ggplotScatter Then
                    fixedRange = False
                    Exit For
                End If
            Next

            ggplot.base.data!x = x
            ggplot.base.data!y = y

            If reverse_y AndAlso y.size > 0 Then
                If y.mapper = MapperTypes.Continuous Then
                    Dim y_vec As Double() = y.ToNumeric
                    Call reverse(y_vec)
                    y = axisMap.Create(y_vec)
                Else
                    y = axisMap.Create(CLRVector.asCharacter(y.value).Reverse.ToArray)
                End If
            End If

            baseData = New ggplotData(x, y)

            If baseData.xscale = d3js.scale.scalers.linear Then
                xAxis = x.ToNumeric
                scale = ggplot.get2DScale(
                    rect:=plotRegion,
                    [default]:=(x.ToNumeric, y),
                    fixedRange:=fixedRange,
                    layerData:=From layer As ggplotLayer
                               In layers
                               Let data As ggplotData = layer.data
                               Where Not data Is Nothing
                               Select data
                )
            Else
                xAxis = x.ToFactors
                scale = ggplot.get2DScale(
                    rect:=plotRegion,
                    [default]:=(x.ToFactors, y),
                    layerData:=From layer As ggplotLayer
                               In layers
                               Let data As ggplotData = layer.data
                               Where Not data Is Nothing
                               Select data
                )
            End If

            If Not ggplot.panelBorder Is Nothing Then
                If Not ggplot.panelBorder.fill.StringEmpty AndAlso ggplot.panelBorder.fill <> "NA" Then
                    Call g.FillRectangle(ggplot.panelBorder.fill.GetBrush, plotRegion)
                End If
                If Not ggplot.panelBorder.border Is Nothing Then
                    Call g.DrawRectangle(css.GetPen(ggplot.panelBorder.border), plotRegion)
                End If
            End If

            Call Axis.DrawAxis(
                g, canvas,
                scaler:=scale,
                showGrid:=theme.drawGrid,
                xlabel:=ggplot.xlabel,
                ylabel:=ggplot.ylabel,
                gridFill:=theme.gridFill,
                axisStroke:=theme.axisStroke,
                gridX:=theme.gridStrokeX,
                gridY:=theme.gridStrokeY,
                labelFont:=theme.axisLabelCSS,
                tickFontStyle:=theme.axisTickCSS,
                XtickFormat:=theme.XaxisTickFormat,
                YtickFormat:=theme.YaxisTickFormat,
                xlabelRotate:=theme.xAxisRotate,
                xlayout:=theme.xAxisLayout,
                ylayout:=theme.yAxisLayout
            )

            Dim legends As New List(Of IggplotLegendElement)
            Dim stream As New ggplotPipeline With {
                .ggplot = ggplot,
                .canvas = canvas,
                .g = g,
                .scale = scale,
                .x = xAxis,
                .y = y
            }

            Do While layers.Count > 0
                Call layers _
                    .Dequeue _
                    .Plot(stream) _
                    .DoCall(AddressOf legends.Add)
            Loop

            Call ggplot.Draw2DElements(g, canvas, legends)
        End Sub

    End Module
End Namespace
