Namespace options

    Public Class ggplotAxisLabel : Inherits ggplotOption

        Public Property x As String
        Public Property y As String
        Public Property title As String

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.xlabel = x
            ggplot.ylabel = y

            Return ggplot
        End Function
    End Class
End Namespace