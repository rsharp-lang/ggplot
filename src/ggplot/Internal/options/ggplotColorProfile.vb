#Region "Microsoft.VisualBasic::c6f1864fe4685556f269f9ef824a198f, ggplot\src\ggplot\Internal\options\ggplotColorProfile.vb"

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

'   Total Lines: 14
'    Code Lines: 10
' Comment Lines: 0
'   Blank Lines: 4
'     File Size: 360 B


'     Class ggplotColorProfile
' 
'         Properties: profile
' 
'         Function: Config
' 
' 
' /********************************************************************************/

#End Region

Imports ggplot.colors
Imports ggplot.layers

Namespace options

    Public Class ggplotColorProfile : Inherits ggplotOption

        Public Property profile As ggplotColorMap

        ''' <summary>
        ''' config the last <see cref="ggplotLayer"/> its color map value.
        ''' </summary>
        ''' <param name="ggplot"></param>
        ''' <returns></returns>
        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.layers.Last.colorMap = profile
            Return ggplot
        End Function
    End Class
End Namespace
