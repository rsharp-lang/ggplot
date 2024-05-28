#Region "Microsoft.VisualBasic::5106048fb894eb45410ec465ccaa1e56, src\ggplot\Internal\options\element_blank.vb"

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

    '   Total Lines: 17
    '    Code Lines: 14
    ' Comment Lines: 0
    '   Blank Lines: 3
    '     File Size: 483 B


    '     Class element_blank
    ' 
    '         Function: GetCssStroke
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports any = Microsoft.VisualBasic.Scripting

Namespace options

    ''' <summary>
    ''' a flag object for indicates the NULL or default value
    ''' </summary>
    Public Class element_blank

        ''' <summary>
        ''' does current blank value means ggplot2 liked waiver object?
        ''' 
        ''' A waiver is a "flag" object, similar to NULL, that indicates the calling function 
        ''' should just use the default value. It is used in certain functions to distinguish 
        ''' between displaying nothing (NULL) and displaying a default value calculated 
        ''' elsewhere (waiver())
        ''' </summary>
        ''' <returns></returns>
        Public Property waiver As Boolean

        ''' <summary>
        ''' gets the default value
        ''' </summary>
        ''' <param name="stroke"></param>
        ''' <returns>
        ''' this function may returns values of:
        ''' 
        ''' 1. nothing, for the given object is nothing
        ''' 2. element_blank, if the given object is <see cref="element_blank"/>
        ''' 3. css value, if the given object is scibasic.net internal css object
        ''' 
        ''' </returns>
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
