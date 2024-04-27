#Region "Microsoft.VisualBasic::9731639fdc26e7422ad022ed906a9cce, G:/GCModeller/src/runtime/ggplot/src/ggplot//Internal/options/ggplotAxisLabel.vb"

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
    '    Code Lines: 13
    ' Comment Lines: 0
    '   Blank Lines: 4
    '     File Size: 497 B


    '     Class ggplotAxisLabel
    ' 
    '         Properties: title, x, y
    ' 
    '         Function: Config
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace options

    Public Class ggplotAxisLabel : Inherits ggplotOption

        Public Property x As String
        Public Property y As String
        Public Property title As String

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.xlabel = If(x, ggplot.xlabel)
            ggplot.ylabel = If(y, ggplot.ylabel)
            ggplot.main = If(title, ggplot.main)

            Return ggplot
        End Function
    End Class
End Namespace
