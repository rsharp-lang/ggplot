Imports ggplot.elements
Imports Microsoft.VisualBasic.MIME.Html.CSS

Public Module ggplotExtensions

    Public Function GetStroke(line As Object, Optional default$ = "stroke: width; stroke-width: 6px; stroke-dash: solid;") As Stroke
        If TypeOf line Is lineElement Then
            Return DirectCast(line, lineElement).GetStroke
        ElseIf TypeOf line Is Stroke Then
            Return line
        Else
            Dim css As String = InteropArgumentHelper.getStrokePenCSS(line, "stroke: width; stroke-width: 6px; stroke-dash: solid;")
            Dim pen As Stroke = Stroke.TryParse(css)

            Return pen
        End If
    End Function
End Module
