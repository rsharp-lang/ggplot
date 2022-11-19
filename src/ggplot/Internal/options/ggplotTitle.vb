#Region "Microsoft.VisualBasic::273e296bfcc66dafd8925d9b0594af41, ggplot\src\ggplot\Internal\options\ggplotTitle.vb"

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

    '   Total Lines: 19
    '    Code Lines: 14
    ' Comment Lines: 0
    '   Blank Lines: 5
    '     File Size: 478 B


    '     Class ggplotTitle
    ' 
    '         Properties: text_wrap, title
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Config
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace options

    Public Class ggplotTitle : Inherits ggplotOption

        Public Property title As String
        Public Property text_wrap As Boolean = False

        Sub New(title As String)
            Me.title = title
        End Sub

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.main = title
            ggplot.ggplotTheme.mainTextWrap = text_wrap

            Return ggplot
        End Function
    End Class
End Namespace
