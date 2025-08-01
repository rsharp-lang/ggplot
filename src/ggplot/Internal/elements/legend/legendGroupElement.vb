﻿#Region "Microsoft.VisualBasic::ffa593e7eb9f07d6a4ee031f5740d3c9, src\ggplot\Internal\elements\legend\legendGroupElement.vb"

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

    '   Total Lines: 89
    '    Code Lines: 74 (83.15%)
    ' Comment Lines: 3 (3.37%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 12 (13.48%)
    '     File Size: 3.57 KB


    '     Class legendGroupElement
    ' 
    '         Properties: layout, legends, shapeSize, size
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

#If NET48 Then
Imports Pen = System.Drawing.Pen
Imports Pens = System.Drawing.Pens
Imports Brush = System.Drawing.Brush
Imports Font = System.Drawing.Font
Imports Brushes = System.Drawing.Brushes
Imports SolidBrush = System.Drawing.SolidBrush
Imports DashStyle = System.Drawing.Drawing2D.DashStyle
Imports Image = System.Drawing.Image
Imports Bitmap = System.Drawing.Bitmap
Imports GraphicsPath = System.Drawing.Drawing2D.GraphicsPath
Imports LineCap = System.Drawing.Drawing2D.LineCap
Imports TextureBrush = System.Drawing.TextureBrush
#Else
Imports Pen = Microsoft.VisualBasic.Imaging.Pen
Imports Pens = Microsoft.VisualBasic.Imaging.Pens
Imports Brush = Microsoft.VisualBasic.Imaging.Brush
Imports Font = Microsoft.VisualBasic.Imaging.Font
Imports Brushes = Microsoft.VisualBasic.Imaging.Brushes
Imports SolidBrush = Microsoft.VisualBasic.Imaging.SolidBrush
Imports DashStyle = Microsoft.VisualBasic.Imaging.DashStyle
Imports Image = Microsoft.VisualBasic.Imaging.Image
Imports Bitmap = Microsoft.VisualBasic.Imaging.Bitmap
Imports GraphicsPath = Microsoft.VisualBasic.Imaging.GraphicsPath
Imports LineCap = Microsoft.VisualBasic.Imaging.LineCap
Imports TextureBrush = Microsoft.VisualBasic.Imaging.TextureBrush
#End If

Namespace elements.legend

    ''' <summary>
    ''' A category group color legends
    ''' </summary>
    Public Class legendGroupElement : Inherits ggplotElement
        Implements IggplotLegendElement

        Public Overrides Property layout As Layout Implements IggplotLegendElement.layout
        Public Property legends As LegendObject()
        Public Property shapeSize As New Size(90, 30)

        Default Public ReadOnly Property Item(i As Integer) As LegendObject
            Get
                Return _legends(i)
            End Get
        End Property

        Public ReadOnly Property size As Integer Implements IggplotLegendElement.size
            Get
                If legends.IsNullOrEmpty Then
                    Return 0
                Else
                    Return legends.Length
                End If
            End Get
        End Property

        Public Sub Draw(g As IGraphics, canvas As GraphicsRegion, x As Double, y As Double, theme As Theme) Implements IggplotLegendElement.Draw
            Dim brush As Brush = theme.mainTextColor.GetBrush
            Dim layout As New PointF(x, y)

            Call g.DrawLegends(
                topLeft:=layout,
                legends:=legends,
                gSize:=$"{shapeSize.Width},{shapeSize.Height}",
                fillBg:=theme.legendBoxBackground,
                titleBrush:=brush,
                d:=5
            )
        End Sub

        Public Function MeasureSize(g As IGraphics) As SizeF Implements IggplotLegendElement.MeasureSize
            Dim maxSizeLabel As String = legends.Select(Function(l) l.title).MaxLengthString
            Dim maxSize As SizeF = g.MeasureString(maxSizeLabel, legends(Scan0).GetFont(g.LoadEnvironment))

            maxSize = New SizeF(shapeSize.Width + maxSize.Width, maxSize.Height)
            maxSize = New SizeF(maxSize.Width, maxSize.Height * (legends.Length + 1))

            Return maxSize
        End Function
    End Class

End Namespace
