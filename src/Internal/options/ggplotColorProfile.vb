Namespace options

    Public Class ggplotColorProfile : Inherits ggplotOption

        Public Property profile As ggplotColorMap

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.layers.Last.colorMap = profile
            Return ggplot
        End Function
    End Class
End Namespace