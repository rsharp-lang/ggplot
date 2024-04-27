#Region "Microsoft.VisualBasic::44d20a2903433ac20b105c4546cbd540, G:/GCModeller/src/runtime/ggplot/src/ggpubr//layers/ggplotConfidenceEllipse.vb"

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

    '   Total Lines: 48
    '    Code Lines: 35
    ' Comment Lines: 6
    '   Blank Lines: 7
    '     File Size: 1.95 KB


    ' Class ggplotConfidenceEllipse
    ' 
    '     Properties: level
    ' 
    '     Function: Plot, PlotOrdinal
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
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

