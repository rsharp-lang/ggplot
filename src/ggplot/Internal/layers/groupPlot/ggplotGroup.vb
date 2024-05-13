#Region "Microsoft.VisualBasic::c7e2aa622fe4dbad8f6a74e3e7f07418, src\ggplot\Internal\layers\groupPlot\ggplotGroup.vb"

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

    '   Total Lines: 104
    '    Code Lines: 75
    ' Comment Lines: 8
    '   Blank Lines: 21
    '     File Size: 4.01 KB


    '     Class ggplotGroup
    ' 
    '         Properties: groupWidth
    ' 
    '         Function: getColors, (+3 Overloads) getDataGroups, getLabelPosY, Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.colors
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports Microsoft.VisualBasic.Math.Quantile

Namespace layers

    Public MustInherit Class ggplotGroup : Inherits ggplotLayer

        Public Property groupWidth As Double = 0.5

        ''' <summary>
        ''' get colorset mapping from <see cref="colorMap"/> or 
        ''' use default color set 'paper'.
        ''' </summary>
        ''' <param name="stream"></param>
        ''' <param name="groupNames"></param>
        ''' <returns></returns>
        Protected Function getColors(stream As ggplotPipeline, groupNames As IEnumerable(Of String)) As Func(Of Object, String)
            If colorMap Is Nothing Then
                colorMap = ggplotColorMap.CreateColorMap(
                    map:="Paper",
                    alpha:=alpha,
                    env:=stream.ggplot.environment
                )
            End If

            Return colorMap.ColorHandler(stream.ggplot, groupNames.ToArray)
        End Function

        Public Shared Iterator Function getDataGroups(tags As String(), x As Double(), y As Double()) As IEnumerable(Of NamedCollection(Of PointF))
            Dim data As New Dictionary(Of String, List(Of PointF))

            For i As Integer = 0 To tags.Length - 1
                If Not data.ContainsKey(tags(i)) Then
                    Call data.Add(tags(i), New List(Of PointF))
                End If

                Call data(tags(i)).Add(New PointF(x(i), y(i)))
            Next

            For Each group In data
                Yield New NamedCollection(Of PointF) With {
                    .name = group.Key,
                    .value = group.Value.ToArray
                }
            Next
        End Function

        Public Shared Iterator Function getDataGroups(tags As String(), y As Double()) As IEnumerable(Of NamedCollection(Of Double))
            Dim data As New Dictionary(Of String, List(Of Double))

            For i As Integer = 0 To tags.Length - 1
                If Not data.ContainsKey(tags(i)) Then
                    Call data.Add(tags(i), New List(Of Double))
                End If

                Call data(tags(i)).Add(y(i))
            Next

            For Each group In data
                Yield New NamedCollection(Of Double) With {
                    .name = group.Key,
                    .value = group.Value.ToArray
                }
            Next
        End Function

        Protected Function getLabelPosY(group As IEnumerable(Of Double), y As DataScaler, Optional factor As Double = 1) As Double
            Dim groupVal As Double() = group.ToArray
            Dim quartile = groupVal.Quartile
            Dim outlier = groupVal.AsVector.Outlier(quartile)

            If Not outlier.outlier.IsNullOrEmpty Then
                quartile = outlier.normal.Quartile
            End If

            ' max
            Return y.TranslateY(quartile.range.Max * factor)
        End Function

        Protected Function getDataGroups(stream As ggplotPipeline) As IEnumerable(Of NamedCollection(Of Double))
            Dim tags As String() = stream.x
            Dim y As Double() = stream.y

            Return getDataGroups(tags, y)
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            If stream.scale.xscale = d3js.scale.scalers.linear Then
                Throw New NotImplementedException
            Else
                Return PlotOrdinal(stream, stream.scale.X)
            End If
        End Function

        Protected MustOverride Function PlotOrdinal(stream As ggplotPipeline, x As d3js.scale.OrdinalScale) As IggplotLegendElement

    End Class
End Namespace
