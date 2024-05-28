#Region "Microsoft.VisualBasic::3bd8e823eb866d02921ec4cd52b430bb, src\ggplot\Internal\elements\legend\IggplotLegendElement.vb"

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

    '   Total Lines: 27
    '    Code Lines: 12 (44.44%)
    ' Comment Lines: 9 (33.33%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 6 (22.22%)
    '     File Size: 825 B


    '     Interface IggplotLegendElement
    ' 
    '         Properties: layout, size
    ' 
    '         Function: MeasureSize
    ' 
    '         Sub: Draw
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Namespace elements.legend

    ''' <summary>
    ''' legend element to draw of current ggplot layer
    ''' </summary>
    Public Interface IggplotLegendElement

        Property layout As Layout

        ''' <summary>
        ''' the number of the legend object that 
        ''' contains in current legend group 
        ''' element object.
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property size As Integer

        Sub Draw(g As IGraphics, canvas As GraphicsRegion, x As Double, y As Double, theme As Theme)
        Function MeasureSize(g As IGraphics) As SizeF

    End Interface
End Namespace
