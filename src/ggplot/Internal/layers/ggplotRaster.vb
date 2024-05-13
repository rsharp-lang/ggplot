#Region "Microsoft.VisualBasic::19cf8588f76acdcec3d8ad00d7068e25, src\ggplot\Internal\layers\ggplotRaster.vb"

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

    '   Total Lines: 29
    '    Code Lines: 14
    ' Comment Lines: 8
    '   Blank Lines: 7
    '     File Size: 823 B


    '     Class ggplotRaster
    ' 
    '         Properties: image, layout
    ' 
    '         Function: Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend

Namespace layers

    Public Class ggplotRaster : Inherits ggplotLayer

        ''' <summary>
        ''' the layout and position of the raster image plot placed at, nothing means overlaps the entire plot region
        ''' </summary>
        ''' <returns></returns>
        Public Property layout As RectangleF = Nothing
        ''' <summary>
        ''' the raster image
        ''' </summary>
        ''' <returns></returns>
        Public Property image As Image

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            If layout.IsEmpty Then

            End If

            Call stream.g.DrawImage(image, layout)

            Return Nothing
        End Function
    End Class
End Namespace
