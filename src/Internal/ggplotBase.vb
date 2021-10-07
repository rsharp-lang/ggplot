''' <summary>
''' 只绘制出一个基本的坐标轴
''' </summary>
Public Class ggplotBase

    Public Property reader As ggplotReader

    Public Function getColors(ggplot As ggplot) As String()
        Return reader.getMapColor(ggplot.data, ggplot.environment)
    End Function

End Class