Public MustInherit Class ggplotOption

    Public MustOverride Function Config(ggplot As ggplot) As ggplot

End Class

Public Class ggplotAxisLabel : Inherits ggplotOption

    Public Property x As String
    Public Property y As String

    Public Overrides Function Config(ggplot As ggplot) As ggplot
        ggplot.xlabel = x
        ggplot.ylabel = y

        Return ggplot
    End Function
End Class

Public Class ggplotTheme : Inherits ggplotOption

    Public Overrides Function Config(ggplot As ggplot) As ggplot
        Throw New NotImplementedException()
    End Function
End Class