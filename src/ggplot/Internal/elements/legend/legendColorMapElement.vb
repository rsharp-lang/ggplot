#Region "Microsoft.VisualBasic::6cdf55ff007650e6f65e268d6d183b9e, src\ggplot\Internal\elements\legend\legendColorMapElement.vb"

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
    '    Code Lines: 31
    ' Comment Lines: 3
    '   Blank Lines: 7
    '     File Size: 1.59 KB


    '     Class legendColorMapElement
    ' 
    '         Properties: colorMapLegend, height, layout, size, width
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
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors

Namespace elements.legend

    ''' <summary>
    ''' legend for heatmap scale
    ''' </summary>
    Public Class legendColorMapElement : Inherits ggplotElement
        Implements IggplotLegendElement

        Public Overrides Property layout As Layout Implements IggplotLegendElement.layout
        Public Property colorMapLegend As ColorMapLegend
        Public Property width As Single
        Public Property height As Single

        Public ReadOnly Property size As Integer Implements IggplotLegendElement.size
            Get
                If colorMapLegend Is Nothing Then
                    Return 0
                Else
                    Return 1
                End If
            End Get
        End Property

        Public Sub Draw(g As IGraphics, canvas As GraphicsRegion, x As Double, y As Double, theme As Theme) Implements IggplotLegendElement.Draw
            Call colorMapLegend.Draw(g, New Rectangle(x, y, width, height))
        End Sub

        Public Function MeasureSize(g As IGraphics) As SizeF Implements IggplotLegendElement.MeasureSize
            Dim titleSize = g.MeasureString(colorMapLegend.title, colorMapLegend.titleFont)
            Dim maxWidth As Single = {titleSize.Width, width}.Max

            Return New SizeF(maxWidth, height)
        End Function
    End Class
End Namespace
