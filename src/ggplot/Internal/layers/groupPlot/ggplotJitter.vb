﻿#Region "Microsoft.VisualBasic::2ed3643b8ff01ea0c7178126d335d49e, src\ggplot\Internal\layers\groupPlot\ggplotJitter.vb"

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

    '   Total Lines: 62
    '    Code Lines: 51 (82.26%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 11 (17.74%)
    '     File Size: 2.22 KB


    '     Enum adjustColor
    ' 
    '         darker, lighter, none
    ' 
    '  
    ' 
    ' 
    ' 
    '     Class ggplotJitter
    ' 
    '         Properties: adjust, radius
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
Imports Microsoft.VisualBasic.Imaging

#If NET48 Then
Imports SoliBrush = System.Drawing.SolidBrush
#Else
Imports SolidBrush = Microsoft.VisualBasic.Imaging.SolidBrush
#End If

Namespace layers

    Public Enum adjustColor
        none
        darker
        lighter
    End Enum

    Public Class ggplotJitter : Inherits ggplotGroup

        Public Property radius As Double = 10
        Public Property adjust As adjustColor = adjustColor.none

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, xscale As d3js.scale.OrdinalScale) As IggplotLegendElement
            Dim x As Double()
            Dim y As Double()
            Dim g As IGraphics = stream.g
            Dim binWidth As Double = DirectCast(stream.scale.X, d3js.scale.OrdinalScale).binWidth
            Dim color As Color
            Dim allGroupData = getDataGroups(stream).ToArray
            Dim colors As Func(Of Object, String) = getColors(stream, allGroupData.Select(Function(i) i.name))
            Dim brush As Brush

            For Each group As NamedCollection(Of Double) In allGroupData
                y = group.Select(AddressOf stream.scale.TranslateY).ToArray
                x = xscale(group.name) _
                    .Replicate(y.Length) _
                    .ToArray
                x = Scatter.Jitter(x, width_jit:=groupWidth * binWidth)
                color = colors(group.name).TranslateColor.Alpha(alpha * 255)

                If adjust <> adjustColor.none Then
                    If adjust = adjustColor.darker Then
                        color = color.Darken
                    Else
                        color = color.Lighten
                    End If
                End If

                brush = New SolidBrush(color)

                For i As Integer = 0 To x.Length - 1
                    Call g.DrawCircle(New PointF(x(i), y(i)), CSng(radius), brush)
                Next
            Next

            Return Nothing
        End Function
    End Class
End Namespace
