#Region "Microsoft.VisualBasic::13247d50d3bf6beb65f72fafc0713db5, src\ggplot\Internal\elements\ggplotElement.vb"

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
    '    Code Lines: 7 (38.89%)
    ' Comment Lines: 7 (38.89%)
    '    - Xml Docs: 85.71%
    ' 
    '   Blank Lines: 4 (22.22%)
    '     File Size: 494 B


    '     Class ggplotElement
    ' 
    '         Properties: layout
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot.options
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas

Namespace elements

    ''' <summary>
    ''' ### Theme elements
    ''' 
    ''' In conjunction with the theme system, the ``element_`` 
    ''' functions specify the display of how non-data 
    ''' components of the plot are drawn.
    ''' </summary>
    Public Class ggplotElement : Inherits element_blank

        Public Overridable Property layout As Layout

    End Class
End Namespace
