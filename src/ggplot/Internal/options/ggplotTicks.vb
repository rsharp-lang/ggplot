#Region "Microsoft.VisualBasic::253eb587d68ad2fb98f524e552a8dde9, src\Internal\options\ggplotTicks.vb"

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

    '     Class ggplotTicks
    ' 
    '         Properties: axis, format, max, min, reverse
    '         Enum TickStyleNames
    ' 
    ' 
    ' 
    ' 
    '  
    ' 
    '     Function: Config, ParseFormat
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.ComponentModel

Namespace options

    ''' <summary>
    ''' Position scales for continuous data (x &amp; y)
    ''' 
    ''' scale_x_continuous() and scale_y_continuous() are the default 
    ''' scales for continuous x and y aesthetics. There are three 
    ''' variants that set the trans argument for commonly used 
    ''' transformations: ``scale_*_log10()``, ``scale_*_sqrt()`` and 
    ''' ``scale_*_reverse()``.
    ''' </summary>
    Public Class ggplotTicks : Inherits ggplotOption

        Public Property format As String = "F2"

        ''' <summary>
        ''' target axis config: x or y
        ''' </summary>
        ''' <returns></returns>
        Public Property axis As String = "X"

        Public Property min As Double
        Public Property max As Double

        Public Property reverse As Boolean = False

        Public Enum TickStyleNames
            ''' <summary>
            ''' 普通的数字
            ''' </summary>
            <Description("F2")> numeric
            ''' <summary>
            ''' 带百分比符号的百分数样式
            ''' </summary>
            <Description("P2")> percent
            ''' <summary>
            ''' 科学计数法样式
            ''' </summary>
            <Description("G3")> scientific
            ''' <summary>
            ''' 带金钱符号样式的
            ''' </summary>
            <Description("C3")> money
        End Enum

        Public Shared Function ParseFormat(style As String) As String
            If style.StringEmpty Then
                Return Nothing
            End If

            Static defaultFormats As Dictionary(Of String, TickStyleNames) =
                Enums(Of TickStyleNames) _
                    .ToDictionary(Function(tick)
                                      Return tick.ToString
                                  End Function)

            If defaultFormats.ContainsKey(style.ToLower) Then
                Return defaultFormats(style.ToLower).Description
            Else
                Return style.ToUpper
            End If
        End Function

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            If Strings.LCase(axis) = "x" Then
                If Not format.StringEmpty Then
                    ggplot.ggplotTheme.XaxisTickFormat = format
                End If

                ggplot.args.slots("scale_x_reverse") = reverse
            Else
                If Not format.StringEmpty Then
                    ggplot.ggplotTheme.YaxisTickFormat = format
                End If

                ggplot.args.slots("scale_y_reverse") = reverse
            End If

            Return ggplot
        End Function
    End Class
End Namespace
