#Region "Microsoft.VisualBasic::38f911ab95bac89f2521a7340a689558, src\ggraph\Internal\render\textRender.vb"

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

    '   Total Lines: 75
    '    Code Lines: 64
    ' Comment Lines: 0
    '   Blank Lines: 11
    '     File Size: 3.07 KB


    '     Class textRender
    ' 
    '         Properties: color, defaultSize, fontsize, iteration
    ' 
    '         Function: getFontSize, Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Styling.FillBrushes
Imports Microsoft.VisualBasic.Data.visualize.Network.Styling.Numeric
Imports Microsoft.VisualBasic.Imaging

Namespace ggraph.render

    Public Class textRender : Inherits ggplotLayer

        Public Property fontsize As IGetSize
        Public Property iteration As Integer = 30
        Public Property color As IGetBrush
        Public Property defaultSize As Single = 45.0!

        Private Function getFontSize(graph As NetworkGraph, dpi As Integer) As Func(Of Node, Single)
            If fontsize Is Nothing Then
                Return Function(any) FontFace.PointSizeScale(defaultSize, dpi)
            Else
                Dim map As Dictionary(Of String, Single) = fontsize _
                    .GetSize(graph.vertex) _
                    .ToDictionary(Function(n) n.Key.label,
                                  Function(n)
                                      Return n.Maps
                                  End Function)

                Return Function(n)
                           If n Is Nothing Then
                               Return FontFace.PointSizeScale(defaultSize, dpi)
                           Else
                               Return FontFace.PointSizeScale(map.TryGetValue(n.label, [default]:=45.0!), dpi)
                           End If
                       End Function
            End If
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim renderLabel As New LabelRendering(
                labelColorAsNodeColor:=True,
                iteration:=iteration,
                showLabelerProgress:=False,
                defaultLabelColorValue:="black",
                labelTextStrokeCSS:=Nothing,
                getLabelColor:=Function() Drawing.Color.Black
            )
            Dim allLabels As New List(Of LayoutLabel)
            Dim fontsize As Func(Of Node, Single) = getFontSize(stream.ggplot.data, stream.g.Dpi)

            For Each label As LayoutLabel In DirectCast(stream, graphPipeline).labels.Where(Function(lb) lb.hasGDIData)
                label.style = New Font(
                    family:=label.style.FontFamily,
                    emSize:=fontsize(label.node),
                    style:=label.style.Style
                )

                With stream.g.MeasureString(label.label.text, label.style)
                    label.label.width = .Width * 2
                    label.label.height = .Height * 2
                End With

                Call allLabels.Add(label)
            Next

            Call renderLabel.renderLabels(
                g:=stream.g,
                labelList:=allLabels
            )

            Return Nothing
        End Function
    End Class
End Namespace
