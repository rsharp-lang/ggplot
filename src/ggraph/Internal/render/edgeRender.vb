#Region "Microsoft.VisualBasic::427ef0062e7bd9cc3f88b4c3546899a6, src\ggraph\Internal\render\edgeRender.vb"

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

    '   Total Lines: 56
    '    Code Lines: 47 (83.93%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 9 (16.07%)
    '     File Size: 2.14 KB


    '     Class edgeRender
    ' 
    '         Properties: color, width
    ' 
    '         Function: getWeightScale, Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Imaging

Namespace ggraph.render

    Public Class edgeRender : Inherits ggplotLayer

        Public Property color As String
        Public Property width As New DoubleRange({2, 5})

        Friend Function getWeightScale(graph As NetworkGraph) As Func(Of Edge, Single)
            Dim scale As DoubleRange = graph.graphEdges.Select(Function(e) e.weight).Range
            Dim wscale As DoubleRange = width
            Dim linkWidth As Func(Of Edge, Single) =
                Function(e)
                    Dim w As Single = scale.ScaleMapping(e.weight, wscale)

                    If w <= 0 OrElse w.IsNaNImaginary Then
                        Return width.Min
                    Else
                        Return w
                    End If
                End Function

            Return linkWidth
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim graph As NetworkGraph = stream.ggplot.data
            Dim linkWidth As Func(Of Edge, Single) = getWeightScale(graph)
            Dim edgeDashType As New Dictionary(Of String, DashStyle)
            Dim edgeColor As Color = Me.color.TranslateColor
            Dim engine As New EdgeRendering(
                linkWidth:=linkWidth,
                edgeDashTypes:=edgeDashType,
                scalePos:=stream.layout,
                throwEx:=False,
                edgeShadowDistance:=0,
                defaultEdgeColor:=edgeColor,
                drawEdgeBends:=False,
                drawEdgeDirection:=False
            )
            Dim labels = engine.drawEdges(stream.g, graph).ToArray

            DirectCast(stream, graphPipeline).labels.AddRange(labels)

            Return Nothing
        End Function
    End Class
End Namespace
