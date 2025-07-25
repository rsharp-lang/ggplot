Namespace options

    Public Class ggplotSize : Inherits ggplotOption

        Public Property min As Double
        Public Property max As Double

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            Return ggplot
        End Function
    End Class
End Namespace