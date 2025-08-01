﻿#Region "Microsoft.VisualBasic::2078d0e5ff2881b8999bb4e91a0e7a31, src\ggplot\Internal\options\ggplotTheme.vb"

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

    '   Total Lines: 137
    '    Code Lines: 113 (82.48%)
    ' Comment Lines: 9 (6.57%)
    '    - Xml Docs: 88.89%
    ' 
    '   Blank Lines: 15 (10.95%)
    '     File Size: 5.46 KB


    '     Class ggplotTheme
    ' 
    '         Properties: axis_line, axis_text, axis_text_x, axis_title, legend_background
    '                     legend_position, legend_split, legend_text, legend_tick, legend_title
    '                     panel_background, panel_border, panel_grid, plot_background, plot_title
    '                     text
    ' 
    '         Function: Config
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot.elements
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.MIME.Html.CSS

Namespace options

    ''' <summary>
    ''' a wrapper for the scibasic.net internal <see cref="Canvas.Theme"/> object
    ''' </summary>
    Public Class ggplotTheme : Inherits ggplotOption

        Public Property axis_text As textElement
        Public Property axis_title As textElement
        Public Property axis_text_x As textElement
        Public Property axis_line As String
        Public Property text As textElement
        Public Property legend_background As String
        Public Property legend_text As textElement
        Public Property legend_tick As textElement
        Public Property legend_title As textElement
        Public Property legend_split As Integer
        Public Property legend_position As String
        Public Property plot_background As String
        Public Property plot_title As textElement
        Public Property panel_background As String
        Public Property panel_grid As String
        Public Property panel_border As rectElement

        ''' <summary>
        ''' set up the <see cref="ggplot.ggplotTheme"/> object
        ''' </summary>
        ''' <param name="ggplot"></param>
        ''' <returns></returns>
        Public Overrides Function Config(ggplot As ggplot) As ggplot
            Dim theme As Theme = ggplot.ggplotTheme

            If Not text Is Nothing Then
                If text.is_blank Then
                    theme.mainCSS = Nothing
                    theme.mainTextColor = Nothing
                    theme.axisTickCSS = Nothing
                Else
                    theme.mainCSS = text.GetCSS
                    theme.mainTextColor = text.color
                    theme.axisTickCSS = CSSFont.TryParse(theme.axisTickCSS).SetFontColor(text.color).ToString
                End If
            End If

            If Not plot_title Is Nothing Then
                If plot_title.is_blank Then
                    theme.mainCSS = Nothing
                Else
                    theme.mainCSS = plot_title.ConfigCSS(CSSFont.TryParse(theme.mainCSS)).ToString
                End If
            End If

            If Not axis_title Is Nothing Then
                If axis_title.is_blank Then
                    theme.axisLabelCSS = Nothing
                Else
                    theme.axisLabelCSS = axis_title.GetCSS
                End If
            End If
            If Not axis_text Is Nothing Then
                If axis_text.is_blank Then
                    theme.axisTickCSS = Nothing
                Else
                    theme.axisTickCSS = axis_text.GetCSS
                End If
            End If

            If Not legend_background.StringEmpty Then theme.legendBoxBackground = legend_background
            If Not plot_background.StringEmpty Then theme.background = plot_background
            If Not panel_background.StringEmpty Then theme.gridFill = panel_background

            If Not legend_text Is Nothing Then
                If legend_text.is_blank Then
                    theme.legendLabelCSS = Nothing
                Else
                    theme.legendLabelCSS = legend_text.GetCSS
                End If
            End If
            If Not legend_tick Is Nothing Then
                If legend_tick.is_blank Then
                    theme.legendTickCSS = Nothing
                Else
                    theme.legendTickCSS = legend_tick.GetCSS
                End If
            End If
            If Not legend_title Is Nothing Then
                theme.legendTitleCSS = legend_title.GetCSS
            End If
            If Not legend_position Is Nothing Then
                theme.drawLegend = Not legend_position.TextEquals("none")
            End If

            If Not axis_line.StringEmpty Then
                If axis_line = NameOf(element_blank) Then
                    theme.axisStroke = Nothing
                Else
                    theme.axisStroke = axis_line
                End If
            End If
            If Not panel_grid.StringEmpty Then
                If panel_grid = NameOf(element_blank) Then
                    theme.gridStrokeX = Nothing
                    theme.gridStrokeY = Nothing
                Else
                    theme.gridStrokeX = panel_grid
                    theme.gridStrokeY = panel_grid
                End If
            End If

            If legend_split > 0 Then
                theme.legendSplitSize = legend_split
            End If

            If Not axis_text_x Is Nothing Then
                theme.xAxisRotate = axis_text_x.angle
            End If

            If Not panel_border Is Nothing Then
                ggplot.panelBorder = panel_border
            End If

            ' config theme_void for axis x,y
            If axis_line = NameOf(element_blank) AndAlso element_blank.is_blank(axis_text) AndAlso element_blank.is_blank(axis_title) Then
                ggplot.ggplotTheme.xAxisLayout = XAxisLayoutStyles.None
                ggplot.ggplotTheme.yAxisLayout = YAxisLayoutStyles.None
            End If

            Return ggplot
        End Function
    End Class
End Namespace
