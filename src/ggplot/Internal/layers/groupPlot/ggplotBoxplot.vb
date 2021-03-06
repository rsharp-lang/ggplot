#Region "Microsoft.VisualBasic::9b61384f1a39aeead2280470f924b60c, src\Internal\layers\ggplotBoxplot.vb"

' Author:
' 
'       asuka (amethyst.asuka@gcmodeller.org)
'       xie (genetics@smrucc.org)
'       xieguigang (xie.guigang@live.com)
' 
' Copyright (c) 2018 GPL3 Licensed
' 
' 
' GNU GENERAL PUBLIC LICENSE (GPL3)
' 
' 
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
' 
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
' 
' You should have received a copy of the GNU General Public License
' along with this program. If not, see <http://www.gnu.org/licenses/>.



' /********************************************************************************/

' Summaries:

'     Class ggplotBoxplot
' 
'         Function: Plot
' 
' 
' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots.BoxPlot
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports Microsoft.VisualBasic.MIME.Html.CSS

Namespace layers

    Public Class ggplotBoxplot : Inherits ggplotGroup

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, xscale As d3js.scale.OrdinalScale) As IggplotLegendElement
            Dim g As IGraphics = stream.g
            Dim binWidth As Double = DirectCast(stream.scale.X, d3js.scale.OrdinalScale).binWidth
            Dim yscale As YScaler = stream.scale
            Dim boxWidth As Double = binWidth * groupWidth
            Dim lineStroke As Pen = Stroke.TryParse(stream.theme.gridStrokeX).GDIObject
            Dim labelFont As Font = CSSFont.TryParse(stream.theme.tagCSS).GDIObject(g.Dpi)
            Dim allGroupData = getDataGroups(stream).ToArray
            Dim colors As Func(Of Object, String) = getColors(stream, allGroupData.Select(Function(i) i.name))
            Dim y As DataScaler = stream.scale
            Dim bottom = stream.canvas.PlotRegion.Bottom
            Dim top = stream.canvas.PlotRegion.Top

            For Each group As NamedCollection(Of Double) In allGroupData
                Dim x As Double = xscale(group.name)
                Dim data As New NamedValue(Of Vector) With {
                    .Name = group.name,
                    .Value = group.AsVector
                }
                Dim color As Color = colors(group.name).TranslateColor.Alpha(alpha * 255)

                Call g.DrawLine(lineStroke, New PointF(x, top), New PointF(x, bottom))
                Call Box.PlotBox(
                    group:=data,
                    x0:=x - boxWidth / 2,
                    brush:=New SolidBrush(color),
                    boxWidth:=boxWidth,
                    fillBox:=True,
                    lineWidth:=5,
                    y:=y,
                    dotSize:=10,
                    showDataPoints:=False,
                    showOutliers:=False,
                    g:=g
                )
            Next

            Return Nothing
        End Function
    End Class
End Namespace
