#Region "Microsoft.VisualBasic::0ba6bca777c3e526271cd7c88f6ed0a9, ggplot\src\ggplot\Internal\layers\groupPlot\ggplotViolin.vb"

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

    '   Total Lines: 57
    '    Code Lines: 50
    ' Comment Lines: 0
    '   Blank Lines: 7
    '     File Size: 2.49 KB


    '     Class ggplotViolin
    ' 
    '         Properties: nbins, showStats, splineDegree, zero_break
    ' 
    '         Function: PlotOrdinal
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.MIME.Html.CSS

Namespace layers

    Public Class ggplotViolin : Inherits ggplotGroup

        Public Property splineDegree As Single = 2
        Public Property showStats As Boolean = False
        Public Property nbins As Integer = 25
        Public Property zero_break As Boolean = True

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, xscale As d3js.scale.OrdinalScale) As IggplotLegendElement
            Dim g As IGraphics = stream.g
            Dim binWidth As Double = DirectCast(stream.scale.X, d3js.scale.OrdinalScale).binWidth
            Dim yscale As YScaler = stream.scale
            Dim semiWidth As Double = binWidth / 2 * groupWidth
            Dim lineStroke As Pen = Stroke.TryParse(stream.theme.lineStroke).GDIObject
            Dim labelFont As Font = CSSFont.TryParse(stream.theme.tagCSS).GDIObject(g.Dpi)
            Dim allGroupData = getDataGroups(stream).ToArray
            Dim colors As Func(Of Object, String) = getColors(stream, allGroupData.Select(Function(i) i.name))
            Dim gridPen As Pen = Stroke.TryParse(stream.theme.gridStrokeX)
            Dim bottom = stream.canvas.PlotRegion.Bottom
            Dim top = stream.canvas.PlotRegion.Top

            For Each group As NamedCollection(Of Double) In allGroupData
                Dim x As Double = xscale(group.name)
                Dim color As Color = colors(group.name).TranslateColor.Alpha(alpha * 255)

                Call g.DrawLine(gridPen, New PointF(x, top), New PointF(x, bottom))
                Call Violin.PlotViolin(
                    group:=group,
                    x:=x,
                    yscale:=yscale,
                    semiWidth:=semiWidth,
                    splineDegree:=splineDegree,
                    polygonStroke:=lineStroke,
                    showStats:=showStats,
                    labelFont:=labelFont,
                    color:=color,
                    g:=g,
                    canvas:=stream.canvas,
                    theme:=stream.theme,
                    zeroBreak:=zero_break,
                    nbins:=nbins
                )
            Next

            Return Nothing
        End Function
    End Class
End Namespace
