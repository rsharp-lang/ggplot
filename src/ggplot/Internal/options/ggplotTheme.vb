#Region "Microsoft.VisualBasic::d8c91f76170805e53b0b3c6808013937, src\Internal\options\ggplotTheme.vb"

' Author:
' 
'       asuka (amethyst.asuka@gcmodeller.org)
'       xie (genetics@smrucc.org)
'       xieguigang (xie.guigang@live.com)
' 
' Copyright (c) 2018 GPL3 Licensed
' 
' 
' GNU GENERAL PUBLIC LICENSE (GPL3)
' 
' 
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
' 
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
' 
' You should have received a copy of the GNU General Public License
' along with this program. If not, see <http://www.gnu.org/licenses/>.



' /********************************************************************************/

' Summaries:

'     Class ggplotTheme
' 
'         Properties: axis_text
' 
'         Function: Config
' 
' 
' /********************************************************************************/

#End Region

Imports ggplot.elements
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.MIME.Html.CSS

Namespace options

    Public Class ggplotTheme : Inherits ggplotOption

        Public Property axis_text As textElement
        Public Property axis_title As textElement
        Public Property axis_line As String
        Public Property text As textElement
        Public Property legend_background As String
        Public Property legend_text As textElement
        Public Property plot_background As String
        Public Property plot_title As textElement
        Public Property panel_background As String
        Public Property panel_grid As String

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            Dim theme As Theme = ggplot.ggplotTheme

            If Not text Is Nothing Then
                theme.mainCSS = text.GetCSS
                theme.mainTextColor = text.color
                theme.axisTickCSS = CSSFont.TryParse(theme.axisTickCSS).SetFontColor(text.color).ToString
            End If

            If Not plot_title Is Nothing Then
                theme.mainCSS = plot_title.ConfigCSS(CSSFont.TryParse(theme.mainCSS)).ToString
            End If

            If Not axis_title Is Nothing Then
                theme.axisLabelCSS = axis_title.GetCSS
            End If
            If Not axis_text Is Nothing Then
                theme.axisTickCSS = axis_text.GetCSS
            End If

            If Not legend_background.StringEmpty Then theme.legendBoxBackground = legend_background
            If Not plot_background.StringEmpty Then theme.background = plot_background
            If Not panel_background.StringEmpty Then theme.gridFill = panel_background

            If Not legend_text Is Nothing Then
                theme.legendLabelCSS = legend_text.GetCSS
            End If

            If Not axis_line.StringEmpty Then
                theme.axisStroke = axis_line
            End If
            If Not panel_grid.StringEmpty Then
                theme.gridStrokeX = panel_grid
                theme.gridStrokeY = panel_grid
            End If

            Return ggplot
        End Function
    End Class
End Namespace
