#Region "Microsoft.VisualBasic::52a5821d3c899c0cb77466a7eecaaf21, src\ggplot\ggplotExtensions.vb"

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

    '   Total Lines: 18
    '    Code Lines: 15 (83.33%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 3 (16.67%)
    '     File Size: 684 B


    ' Module ggplotExtensions
    ' 
    '     Function: GetStroke
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot.elements
Imports Microsoft.VisualBasic.MIME.Html.CSS

Public Module ggplotExtensions

    Const default_stroke As String = "stroke: white; stroke-width: 2px; stroke-dash: solid;"

    Public Function GetStroke(line As Object, Optional default$ = default_stroke) As Stroke
        If TypeOf line Is lineElement Then
            Return DirectCast(line, lineElement).GetStroke
        ElseIf TypeOf line Is Stroke Then
            Return line
        Else
            Dim css As String = InteropArgumentHelper.getStrokePenCSS(line, default_stroke)
            Dim pen As Stroke = Stroke.TryParse(css)

            Return pen
        End If
    End Function
End Module
