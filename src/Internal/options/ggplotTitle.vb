Namespace options

    Public Class ggplotTitle : Inherits ggplotOption

        Public Property title As String

        Sub New(title As String)
            Me.title = title
        End Sub

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.main = title
            Return ggplot
        End Function
    End Class
End Namespace