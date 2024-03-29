﻿Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports ggplot
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.Statistics.PCA
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports Microsoft.VisualBasic.Imaging.Math2D

Public Class ggplotConfidenceEllipse : Inherits ggplotGroup

    Public Property level As Double = 0.95

    Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
        Dim sourceData As SerialData = New ggplotScatter().GetSerialData(stream)
        Dim groups As String() = sourceData.pts.Select(Function(p) p.color).ToArray
        Dim x As Double() = sourceData.pts.Select(Function(p) CDbl(p.pt.X)).ToArray
        Dim y As Double() = sourceData.pts.Select(Function(p) CDbl(p.pt.Y)).ToArray
        Dim allGroupData = getDataGroups(groups, x, y).ToArray

        For Each group_data As NamedCollection(Of PointF) In allGroupData
            Dim translate As PointF() = group_data _
                .Select(Function(p) stream.scale.Translate(p)) _
                .ToArray
            Dim group As New Polygon2D(translate)
            Dim shape As Ellipse = Ellipse.ConfidenceEllipse(group, level)
            Dim path As GraphicsPath = shape.BuildPath
            Dim fill As Brush = group_data.name.GetBrush

            Call stream.g.FillPath(fill, path)
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' no used
    ''' </summary>
    ''' <param name="stream"></param>
    ''' <param name="x"></param>
    ''' <returns></returns>
    Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As OrdinalScale) As IggplotLegendElement
        Throw New NotImplementedException()
    End Function
End Class
