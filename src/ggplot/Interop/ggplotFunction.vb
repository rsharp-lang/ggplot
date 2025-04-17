#Region "Microsoft.VisualBasic::5eba8ffe403a9a97e6c9327bd3e39b3a, src\ggplot\Interop\ggplotFunction.vb"

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

'   Total Lines: 104
'    Code Lines: 85 (81.73%)
' Comment Lines: 5 (4.81%)
'    - Xml Docs: 60.00%
' 
'   Blank Lines: 14 (13.46%)
'     File Size: 4.08 KB


' Module ggplotFunction
' 
'     Function: aes, geom_point, ggplot
' 
' /********************************************************************************/

#End Region

Imports ggplot.colors
Imports ggplot.layers
Imports ggplot.layers.layer3d
Imports ggplot.options
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports df = Microsoft.VisualBasic.Data.Framework.DataFrame

''' <summary>
''' ggplot function exports for interop
''' </summary>
Public Module ggplotFunction

    Const NULL As Object = Nothing

    Public Function ggplot(data As df,
                           Optional mapping As ggplotReader = Nothing,
                           Optional colorSet As String = "paper",
                           Optional padding As String = "padding: 5% 15% 15% 15%;",
                           Optional args As list = Nothing) As ggplot

        Dim env As Environment = GlobalEnvironment.defaultEmpty

        If args Is Nothing OrElse args.slots Is Nothing Then
            args = list.empty
        End If

        Dim theme As New Theme With {
            .axisLabelCSS = "font-style: strong; font-size: 12; font-family: " & FontFace.MicrosoftYaHei & ";",
            .axisTickCSS = "font-style: normal; font-size: 10; font-family: " & FontFace.MicrosoftYaHei & ";",
            .padding = padding,
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
            .layers = New List(Of ggplotLayer)
            .base = base
            .args = args
            .xlabel = base.reader.x
            .ylabel = base.reader.y
            .zlabel = base.reader.z
        End With

        Return ggplotDriver
    End Function

    Public Function aes(Optional x As String = Nothing,
                        Optional y As String = Nothing,
                        Optional z As String = Nothing,
                        Optional fill As Object = Nothing,
                        Optional color As Object = Nothing,
                        Optional label As String = Nothing) As ggplotReader

        Return New ggplotReader With {
            .x = x,
            .y = y,
            .z = z,
            .label = label,
            .color = color,
            .[class] = fill
        }
    End Function

    Public Function geom_point(Optional mapping As ggplotReader = NULL,
                               Optional color As Object = Nothing,
                               Optional shape As LegendStyles? = Nothing,
                               Optional stroke As Stroke = Nothing,
                               Optional size As Single = 2,
                               Optional show_legend As Boolean = True,
                               Optional alpha As Double = 1) As ggplotScatter

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
                .stroke = stroke?.ToString
            }
        Else
            ' 2D
            Return New ggplotScatter With {
                .colorMap = colorMap,
                .shape = shape,
                .size = size,
                .showLegend = show_legend,
                .reader = mapping,
                .stroke = stroke?.ToString
            }
        End If
    End Function

    Public Function geom_histogram(Optional bins As Integer = 20,
                                   Optional color As Object = Nothing,
                                   Optional alpha As Double = 1,
                                   Optional binwidth As Double = 0.1,
                                   Optional position As LayoutPosition = LayoutPosition.identity,
                                   Optional range As Double() = Nothing) As ggplotHistogram

        Dim minMax As DoubleRange = Nothing

        If Not range.IsNullOrEmpty Then
            minMax = New DoubleRange(range)
        End If

        Dim colorMap = RColorPalette.getColor(color, [default]:="paper")

        Return New ggplotHistogram With {
            .bins = bins,
            .colorMap = ggplotColorMap.CreateColorMap(colorMap, alpha, Nothing),
            .range = minMax,
            .binwidth = binwidth,
            .position = position,
            .alpha = alpha
        }
    End Function

    Public Function geom_violin(Optional color As list = Nothing,
                                Optional width As Double = 0.9,
                                Optional alpha As Double = 0.95,
                                Optional trim As Boolean = True) As ggplotLayer

        Return New ggplotViolin With {
            .groupWidth = width,
            .colorMap = ggplotColorMap.CreateColorMap(color, 1),
            .alpha = alpha
        }
    End Function

    Public Function geom_jitter(Optional mapping As ggplotReader = NULL,
                                Optional data As Object = NULL,
                                Optional stat As Object = "identity",
                                Optional width As Double = 0.5,
                                Optional radius As Double = 6,
                                Optional alpha As Double = 0.85,
                                Optional color As list = Nothing,
                                Optional adjust As adjustColor = adjustColor.none,
                                Optional env As Environment = Nothing) As ggplotLayer

        Return New ggplotJitter With {
            .reader = mapping,
            .groupWidth = width,
            .alpha = alpha,
            .radius = radius,
            .colorMap = ggplotColorMap.CreateColorMap(color, 1, env),
            .adjust = adjust
        }
    End Function

    ''' <summary>
    ''' ## Modify axis, legend, and plot labels
    ''' 
    ''' Good labels are critical for making your plots accessible to 
    ''' a wider audience. Always ensure the axis and legend labels 
    ''' display the full variable name. Use the plot title and subtitle 
    ''' to explain the main findings. It's common to use the caption 
    ''' to provide information about the data source. tag can be used 
    ''' for adding identification tags to differentiate between multiple 
    ''' plots.
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    ''' <param name="title">The text for the title.</param>
    ''' <param name="subtitle">
    ''' The text For the subtitle For the plot which will be displayed 
    ''' below the title.
    ''' </param>
    ''' <param name="caption">
    ''' The text for the caption which will be displayed in the 
    ''' bottom-right of the plot by default.
    ''' </param>
    ''' <param name="tag">
    ''' The text for the tag label which will be displayed at the top-left 
    ''' of the plot by default.
    ''' </param>
    ''' <param name="alt">
    ''' Text used for the generation of alt-text for the plot. See 
    ''' get_alt_text for examples.
    ''' </param>
    ''' <param name="alt_insight"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' You can also set axis and legend labels in the individual scales 
    ''' (using the first argument, the name). If you're changing other 
    ''' scale options, this is recommended.
    ''' 
    ''' If a plot already has a title, subtitle, caption, etc., And you want 
    ''' To remove it, you can Do so by setting the respective argument To 
    ''' NULL. For example, If plot p has a subtitle, Then p + labs(subtitle = NULL) 
    ''' will remove the subtitle from the plot.
    ''' </remarks>
    Public Function labs(Optional x As String = Nothing,
                         Optional y As String = Nothing,
                         Optional title As String = Nothing,
                         Optional subtitle As String = Nothing,
                         Optional caption As String = Nothing,
                         Optional tag As Object = Nothing,
                         Optional alt As Object = Nothing,
                         Optional alt_insight As Object = Nothing) As ggplotOption

        Return New ggplotAxisLabel With {
            .x = x,
            .y = y,
            .title = title
        }
    End Function
End Module
