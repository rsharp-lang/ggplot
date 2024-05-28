#Region "Microsoft.VisualBasic::dad8ec65d144a49c94a7615116bf5723, src\ggplot\Internal\elements\lineElement.vb"

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

    '   Total Lines: 41
    '    Code Lines: 32 (78.05%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 9 (21.95%)
    '     File Size: 1.12 KB


    '     Class lineElement
    ' 
    '         Properties: color, linetype, width
    ' 
    '         Function: (+2 Overloads) GetStroke, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing.Drawing2D
Imports Microsoft.VisualBasic.MIME.Html.CSS

Namespace elements

    Public Class lineElement : Inherits ggplotElement

        Public Property color As String
        Public Property width As Single
        Public Property linetype As DashStyle = DashStyle.Solid

        Public Function GetStroke(stroke As Stroke) As Stroke
            Dim copy As New Stroke(stroke)

            If Not color.StringEmpty Then
                copy.fill = color
            End If
            If width > 0 Then
                copy.width = width
            End If
            If copy.dash <> linetype Then
                copy.dash = linetype
            End If

            Return copy
        End Function

        Public Function GetStroke() As Stroke
            Return New Stroke With {
                .dash = linetype,
                .fill = color,
                .width = width
            }
        End Function

        Public Overrides Function ToString() As String
            Return GetStroke.CSSValue
        End Function

    End Class
End Namespace
