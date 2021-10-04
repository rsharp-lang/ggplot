Imports System.ComponentModel

Public Class ggplotTicks : Inherits ggplotOption

    Public Property format As String = "F2"

    ''' <summary>
    ''' target axis config: x or y
    ''' </summary>
    ''' <returns></returns>
    Public Property axis As String = "X"

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

    Public Overrides Function Config(ggplot As ggplot) As ggplot
        If Strings.LCase(axis) = "x" Then
            ggplot.ggplotTheme.XaxisTickFormat = format
        Else
            ggplot.ggplotTheme.YaxisTickFormat = format
        End If

        Return ggplot
    End Function
End Class
