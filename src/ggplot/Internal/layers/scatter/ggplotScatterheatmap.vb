#Region "Microsoft.VisualBasic::588ea8ea2b9307dc91c4f85a55df104d, ggplot\src\ggplot\Internal\layers\scatter\ggplotScatterheatmap.vb"

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

    '   Total Lines: 66
    '    Code Lines: 53
    ' Comment Lines: 5
    '   Blank Lines: 8
    '     File Size: 2.95 KB


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
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime
Imports stdNum = System.Math

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
            Dim layerdata = DirectCast(REnv.asVector(Of Double)(data.getVector(Me.layer, fullSize:=True)), Double())
            Dim cellSize = getCellsize(x, y)
            Dim colors As SolidBrush() = Designer _
                .GetColors(stream.theme.colorSet, n:=maplevels) _
                .Select(Function(c) New SolidBrush(c)) _
                .ToArray
            Dim radius As Single = stdNum.Min(cellSize.Width, cellSize.Height) / 2
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

            Return New legendColorMapElement With {
                .colorMapLegend = New ColorMapLegend(stream.theme.colorSet, maplevels) With {
                    .title = Me.layer,
                    .tickAxisStroke = Stroke.TryParse(stream.theme.legendTickAxisStroke).GDIObject,
                    .tickFont = CSSFont.TryParse(stream.theme.legendTickCSS).GDIObject(stream.g.Dpi),
                    .format = stream.theme.legendTickFormat,
                    .ticks = layerdata.CreateAxisTicks,
                    .titleFont = CSSFont.TryParse(stream.theme.legendTitleCSS).GDIObject(stream.g.Dpi)
                },
                .width = stream.canvas.Padding.Right * 3 / 4,
                .height = stream.canvas.PlotRegion.Height
            }
        End Function
    End Class
End Namespace
