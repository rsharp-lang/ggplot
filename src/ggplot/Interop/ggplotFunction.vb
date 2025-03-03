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
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
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

End Module
