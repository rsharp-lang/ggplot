#Region "Microsoft.VisualBasic::564338c0d2c0b702d0d2f878b63185ee, src\ggplot\Internal\options\ggplotTicks.vb"

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

    '   Total Lines: 92
    '    Code Lines: 51 (55.43%)
    ' Comment Lines: 25 (27.17%)
    '    - Xml Docs: 96.00%
    ' 
    '   Blank Lines: 16 (17.39%)
    '     File Size: 3.05 KB


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
            Dim scale_reverse As String
            Dim range_tag As String

            If Strings.LCase(axis) = "x" Then
                If Not format.StringEmpty Then
                    ggplot.ggplotTheme.XaxisTickFormat = format
                End If

                scale_reverse = "scale_x_reverse"
                range_tag = "range_x"
            Else
                If Not format.StringEmpty Then
                    ggplot.ggplotTheme.YaxisTickFormat = format
                End If

                scale_reverse = "scale_y_reverse"
                range_tag = "range_y"
            End If

            ggplot.args.slots(scale_reverse) = reverse
            ggplot.args.slots(range_tag) = {min, max}

            Return ggplot
        End Function
    End Class
End Namespace
