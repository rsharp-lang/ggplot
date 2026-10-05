#Region "Microsoft.VisualBasic::a131c3266116719438c1c71437958d3c, src\ggplot\Internal\layers\groupPlot\ggplotBoxplot.vb"

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

'   Total Lines: 85
'    Code Lines: 78 (91.76%)
' Comment Lines: 0 (0.00%)
'    - Xml Docs: 0.00%
' 
'   Blank Lines: 7 (8.24%)
'     File Size: 3.78 KB


'     Class ggplotBoxplot
' 
'         Function: PlotOrdinal
' 
' 
' /********************************************************************************/

#End Region

Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Plots
Imports Microsoft.VisualBasic.Data.Plots.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render

Namespace layers

    Public Class ggplotBoxplot : Inherits ggplotGroup

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, xscale As d3js.scale.OrdinalScale) As IggplotLegendElement
            Dim g As IGraphics = stream.g
            Dim binWidth As Double = DirectCast(stream.scale.X, d3js.scale.OrdinalScale).binWidth
            Dim yscale As YScaler = stream.scale
            Dim boxWidth As Double = binWidth * groupWidth
            Dim css As CSSEnvirnment = g.LoadEnvironment
            Dim lineStroke As Pen = css.GetPen(Stroke.TryParse(stream.theme.gridStrokeX))
            Dim labelFont As Font = css.GetFont(CSSFont.TryParse(stream.theme.tagCSS))
            Dim allGroupData = getDataGroups(stream).ToArray
            Dim colors As Func(Of Object, String) = getColors(stream, allGroupData.Select(Function(i) i.name))
            Dim groups As New List(Of BoxGroup)

            For Each group As NamedCollection(Of Double) In allGroupData
                Call groups.Add(New BoxGroup With {
                    .Name = group.name,
                    .Data = DirectCast(group, IEnumerable(Of Double)).ToArray,
                    .Color = colors(group.name).TranslateColor.Alpha(alpha * 255)
                })
            Next

            ' 委派给新引擎的 BoxPlot：共享画布 + 跨图层联合坐标
            Call LayerRender.DrawBoxes(g, stream.scale, stream.theme, groups)

            Return Nothing
        End Function
    End Class
End Namespace
