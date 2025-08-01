﻿#Region "Microsoft.VisualBasic::aabd3da52444f9f574d0125f8ecaeb14, src\ggplot\Internal\layers\scatter\ggplotScatterheatmap.vb"

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

    '   Total Lines: 98
    '    Code Lines: 83 (84.69%)
    ' Comment Lines: 5 (5.10%)
    '    - Xml Docs: 80.00%
    ' 
    '   Blank Lines: 10 (10.20%)
    '     File Size: 4.43 KB


    '     Class ggplotScatterheatmap
    ' 
    '         Properties: layer, maplevels
    ' 
    '         Function: Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports std = System.Math

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

Namespace layers

    Public Class ggplotScatterheatmap : Inherits ggplotScatterLayer

        ''' <summary>
        ''' the dataframe feature name for get heatmap layer
        ''' </summary>
        ''' <returns></returns>
        Public Property layer As String
        Public Property maplevels As Integer = 120

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            ' get scatter data
            Dim data As dataframe = stream.ggplot.data
            Dim x As Double() = stream.scale.X(stream.x)
            Dim y As Double() = stream.scale.Y(stream.y)
            Dim layerdata = CLRVector.asNumeric(data.getVector(Me.layer, fullSize:=True))
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim cellSize = getCellsize(x, y)
            Dim colors As SolidBrush() = Designer _
                .GetColors(stream.theme.colorSet, n:=maplevels) _
                .Select(Function(c) New SolidBrush(c)) _
                .ToArray
            Dim radius As Single = std.Min(cellSize.Width, cellSize.Height) / 2
            Dim valueRange As New DoubleRange(layerdata)
            Dim indexRange As New DoubleRange(0, maplevels - 1)
            Dim layerIndex As Integer() = layerdata _
                .Select(Function(xi) CInt(valueRange.ScaleMapping(xi, indexRange))) _
                .ToArray

            If radius < minCell Then
                radius = minCell
            End If

            For i As Integer = 0 To x.Length - 1
                Dim xi = x(i) - cellSize.Width / 2
                Dim yi = y(i) - cellSize.Height / 2
                Dim cell As New RectangleF(xi, yi, cellSize.Width, cellSize.Height)

                Call stream.g.FillRectangle(colors(layerIndex(i)), cell)
            Next

            Dim padding As PaddingLayout = PaddingLayout.EvaluateFromCSS(css, stream.canvas.Padding)

            Return New legendColorMapElement With {
                .colorMapLegend = New ColorMapLegend(stream.theme.colorSet, maplevels) With {
                    .title = Me.layer,
                    .tickAxisStroke = css.GetPen(Stroke.TryParse(stream.theme.legendTickAxisStroke)),
                    .tickFont = css.GetFont(CSSFont.TryParse(stream.theme.legendTickCSS)),
                    .format = stream.theme.legendTickFormat,
                    .ticks = layerdata.CreateAxisTicks,
                    .titleFont = css.GetFont(CSSFont.TryParse(stream.theme.legendTitleCSS))
                },
                .width = padding.Right * 3 / 4,
                .height = stream.canvas.PlotRegion(css).Height
            }
        End Function
    End Class
End Namespace
