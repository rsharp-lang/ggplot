#Region "Microsoft.VisualBasic::8bd9cfaa5309d99d9a2a96ea297cc1d2, src\ggplot\Internal\elements\legend\ggplotLegendElement.vb"

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

    '   Total Lines: 52
    '    Code Lines: 43 (82.69%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 9 (17.31%)
    '     File Size: 1.98 KB


    '     Class ggplotLegendElement
    ' 
    '         Properties: layout, legend, shapeSize, size
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
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.MIME.Html.Render

Namespace elements.legend

    Public Class ggplotLegendElement : Inherits ggplotElement
        Implements IggplotLegendElement

        Public Property legend As LegendObject
        Public Property shapeSize As New Size(120, 45)
        Public Overrides Property layout As Layout Implements IggplotLegendElement.layout

        Public ReadOnly Property size As Integer Implements IggplotLegendElement.size
            Get
                If legend Is Nothing Then
                    Return 0
                Else
                    Return 1
                End If
            End Get
        End Property

        Public Sub Draw(g As IGraphics,
                        canvas As GraphicsRegion,
                        x As Double,
                        y As Double,
                        theme As Theme) Implements IggplotLegendElement.Draw

            Call g.DrawLegends(
                topLeft:=New PointF(x, y),
                legends:={legend},
                gSize:=$"{shapeSize.Width},{shapeSize.Height}",
                fillBg:=theme.legendBoxBackground,
                titleBrush:=theme.mainTextColor.GetBrush
            )
        End Sub

        Public Function MeasureSize(g As IGraphics) As SizeF Implements IggplotLegendElement.MeasureSize
            Dim maxSizeLabel As String = legend.title
            Dim maxSize As SizeF = g.MeasureString(maxSizeLabel, legend.GetFont(g.LoadEnvironment))

            maxSize = New SizeF(shapeSize.Width + maxSize.Width, maxSize.Height)
            maxSize = New SizeF(maxSize.Width, maxSize.Height)

            Return maxSize
        End Function
    End Class
End Namespace
