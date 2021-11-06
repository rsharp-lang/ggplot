Imports Microsoft.VisualBasic.Imaging.Drawing3D

Namespace options

    Public Class ggplotCamera : Inherits ggplotOption

        Public Property camera As Camera

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.args.add("camera", camera)
            Return ggplot
        End Function
    End Class
End Namespace