Imports ggplot.elements
Imports ggplot.elements.legend
Imports ggplot.layers
Imports ggplot.render
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math

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
            Dim x As axisMap = baseData.x
            Dim y As Double() = baseData.y.ToNumeric
            Dim reverse_y As Boolean = ggplot.args.getValue("scale_y_reverse", env:=ggplot.environment, [default]:=False)
            Dim layers As New Queue(Of ggplotLayer)(
                collection:=If(
                    ggplot.UnionGgplotLayers Is Nothing,
                    ggplot.layers,
                    ggplot.UnionGgplotLayers(ggplot.layers)
                )
            )
            Dim scale As DataScaler
            Dim xAxis As Array
            Dim theme As Theme = ggplot.ggplotTheme

            If baseData.xscale = d3js.scale.scalers.linear Then
                xAxis = x.ToNumeric
                scale = ggplot.get2DScale(
                    rect:=canvas.PlotRegion,
                    [default]:=(x.ToNumeric, y),
                    layerData:=From layer As ggplotLayer
                               In layers
                               Let data As ggplotData = layer.initDataSet(ggplot:=ggplot)
                               Where Not data Is Nothing
                               Select data
                )
            Else
                xAxis = x.ToFactors
                scale = ggplot.get2DScale(
                    rect:=canvas.PlotRegion,
                    [default]:=(x.ToFactors, y),
                    layerData:=From layer As ggplotLayer
                               In layers
                               Let data As ggplotData = layer.initDataSet(ggplot:=ggplot)
                               Where Not data Is Nothing
                               Select data
                )
            End If

            If reverse_y AndAlso y.Length > 0 Then
                Call reverse(y)
            End If

            If Not ggplot.panelBorder Is Nothing Then
                If Not ggplot.panelBorder.fill.StringEmpty AndAlso ggplot.panelBorder.fill <> "NA" Then
                    Call g.FillRectangle(ggplot.panelBorder.fill.GetBrush, canvas.PlotRegion)
                End If
                If Not ggplot.panelBorder.border Is Nothing Then
                    Call g.DrawRectangle(ggplot.panelBorder.border.GDIObject, canvas.PlotRegion)
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