Imports ggplot.colors
Imports ggplot.layers
Imports ggplot.layers.layer3d
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports df = Microsoft.VisualBasic.Math.DataFrame.DataFrame

''' <summary>
''' ggplot function exports for interop
''' </summary>
Public Module ggplotFunction

    Const NULL As Object = Nothing

    Public Function ggplot(data As df,
                           Optional mapping As ggplotReader = Nothing,
                           Optional colorSet As String = "paper",
                           Optional args As list = Nothing) As ggplot

        Dim env As Environment = GlobalEnvironment.defaultEmpty

        If args Is Nothing OrElse args.slots Is Nothing Then
            args = list.empty
        End If

        Dim theme As New Theme With {
            .axisLabelCSS = "font-style: strong; font-size: 12; font-family: " & FontFace.MicrosoftYaHei & ";",
            .axisTickCSS = "font-style: normal; font-size: 10; font-family: " & FontFace.MicrosoftYaHei & ";",
            .padding = InteropArgumentHelper.getPadding(args.getByName("padding"), g.DefaultUltraLargePadding),
            .drawLegend = True,
            .legendLabelCSS = "font-style: normal; font-size: 13; font-family: " & FontFace.MicrosoftYaHei & ";",
            .colorSet = RColorPalette.getColorSet(colorSet, [default]:="paper"),
            .xAxisLayout = args.getXAxisLayout(),
            .yAxisLayout = args.getYAxisLayout()
        }
        Dim ggplotDriver As ggplot = ggplot.CreateRender(data, env, theme)
        Dim base As ggplotBase = ggplotDriver.CreateReader(mapping)

        With ggplotDriver
            .driver = Drivers.Default
            .data = data
            .layers = New List(Of ggplotLayer)
            .base = base
            .args = args
            .xlabel = base.reader.x
            .ylabel = base.reader.y
            .zlabel = base.reader.z
        End With

        Return ggplotDriver
    End Function

    Public Function aes(x As String, y As String,
                        Optional z As String = Nothing,
                        Optional label As String = Nothing) As ggplotReader

        Return New ggplotReader With {
            .x = x,
            .y = y,
            .z = z,
            .label = label
        }
    End Function

    Public Function geom_point(Optional mapping As ggplotReader = NULL,
                               Optional color As String = Nothing,
                               Optional shape As LegendStyles? = Nothing,
                               Optional stroke As Stroke = Nothing,
                               Optional size As Single = 2,
                               Optional show_legend As Boolean = True,
                               Optional alpha As Double = 1) As ggplotLayer

        Dim colorMap As ggplotColorMap = ggplotColorMap.CreateColorMap(color, alpha)
        Dim strokeCss As String = InteropArgumentHelper.getStrokePenCSS(stroke, [default]:=Nothing)

        If mapping IsNot Nothing AndAlso Not mapping.isPlain2D Then
            ' 3D
            Return New ggplotScatter3d With {
                .colorMap = colorMap,
                .reader = mapping,
                .shape = shape,
                .size = size,
                .showLegend = show_legend,
                .stroke = stroke.ToString
            }
        Else
            ' 2D
            Return New ggplotScatter With {
                .colorMap = colorMap,
                .shape = shape,
                .size = size,
                .showLegend = show_legend,
                .reader = mapping,
                .stroke = stroke.ToString
            }
        End If
    End Function

End Module
