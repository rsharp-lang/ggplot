#Region "Microsoft.VisualBasic::9ffd2cc1e9f2ef1ae61a344e78842e7d, src\ggplot\Internal\elements\textElement.vb"

    ' Author:
    ' 
    '       xieguigang (I@xieguigang.me)
    ' 
    ' Copyright (c) 2021 R# language
    ' 
    ' 
    ' MIT License
    ' 
    ' 
    ' Permission is hereby granted, free of charge, to any person obtaining a copy
    ' of this software and associated documentation files (the "Software"), to deal
    ' in the Software without restriction, including without limitation the rights
    ' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    ' copies of the Software, and to permit persons to whom the Software is
    ' furnished to do so, subject to the following conditions:
    ' 
    ' The above copyright notice and this permission notice shall be included in all
    ' copies or substantial portions of the Software.
    ' 
    ' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    ' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    ' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    ' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    ' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    ' SOFTWARE.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 26
    '    Code Lines: 20
    ' Comment Lines: 0
    '   Blank Lines: 6
    '     File Size: 701 B


    '     Class textElement
    ' 
    '         Properties: angle, color, style
    ' 
    '         Function: ConfigCSS, GetCSS
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.MIME.Html.CSS

Namespace elements

    Public Class textElement : Inherits ggplotElement

        Public Property style As CSSFont
        Public Property color As String
        Public Property angle As Single

        Public Function GetCSS() As String
            Return style.ToString
        End Function

        Public Function ConfigCSS(css As CSSFont) As CSSFont
            If style.size > 0 Then
                css.size = style.size
            End If
            If Not style.family.StringEmpty Then
                css.family = style.family
            End If

            Return css
        End Function
    End Class
End Namespace
