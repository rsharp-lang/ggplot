Namespace options

    Public Class coord_flip : Inherits ggplotOption

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.ggplotTheme.flipAxis = True
            Return ggplot
        End Function
    End Class
End Namespace