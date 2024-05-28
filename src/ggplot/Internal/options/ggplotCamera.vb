#Region "Microsoft.VisualBasic::934bc7bfe19b93b67ada263e792ec24d, src\ggplot\Internal\options\ggplotCamera.vb"

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
    '    Code Lines: 10 (58.82%)
    ' Comment Lines: 3 (17.65%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 4 (23.53%)
    '     File Size: 450 B


    '     Class ggplotCamera
    ' 
    '         Properties: camera
    ' 
    '         Function: Config
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Imaging.Drawing3D

Namespace options

    ''' <summary>
    ''' 3d camera wrapper object in ggplot
    ''' </summary>
    Public Class ggplotCamera : Inherits ggplotOption

        Public Property camera As Camera

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.args.add("camera", camera)
            Return ggplot
        End Function
    End Class
End Namespace
