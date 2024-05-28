#Region "Microsoft.VisualBasic::2f090420ad0a3bb941365d212bf58a06, src\ggplot\Internal\elements\textElement.vb"

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
    '    Code Lines: 20 (48.78%)
    ' Comment Lines: 15 (36.59%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 6 (14.63%)
    '     File Size: 1.17 KB


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

    ''' <summary>
    ''' data model helper for create the css font object
    ''' </summary>
    Public Class textElement : Inherits ggplotElement

        ''' <summary>
        ''' the font for drawing the specific text elements
        ''' </summary>
        ''' <returns></returns>
        Public Property style As CSSFont
        ''' <summary>
        ''' the text drawing color
        ''' </summary>
        ''' <returns></returns>
        Public Property color As String
        ''' <summary>
        ''' the text drawing angle
        ''' </summary>
        ''' <returns></returns>
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
