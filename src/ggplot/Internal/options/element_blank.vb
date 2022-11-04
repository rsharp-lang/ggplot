Imports any = Microsoft.VisualBasic.Scripting

Namespace options

    Public Class element_blank

        Public Shared Function GetCssStroke(stroke As Object) As String
            If stroke Is Nothing Then
                Return Nothing
            ElseIf TypeOf stroke Is element_blank Then
                Return NameOf(element_blank)
            Else
                Return any.ToString(stroke)
            End If
        End Function
    End Class
End Namespace