#Region "Microsoft.VisualBasic::edff655295effaee475e33bbfbdd4795, G:/GCModeller/src/runtime/ggplot/src/ggplot//Internal/layers/layer3/ggplotScatter3d.vb"

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

    '   Total Lines: 88
    '    Code Lines: 75
    ' Comment Lines: 1
    '   Blank Lines: 12
    '     File Size: 3.69 KB


    '     Class ggplotScatter3d
    ' 
    '         Constructor: (+2 Overloads) Sub New
    '         Function: createSerialData, populateModels
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Plot3D.Device
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing3D
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers.layer3d

    Public Class ggplotScatter3d : Inherits ggplotScatter
        Implements Ilayer3d

        Sub New(copy2D As ggplotScatter)
            Me.colorMap = copy2D.colorMap
            Me.reader = copy2D.reader
            Me.shape = copy2D.shape
            Me.showLegend = copy2D.showLegend
            Me.size = copy2D.size
            Me.which = copy2D.which
            Me.zindex = copy2D.zindex
        End Sub

        Sub New()
        End Sub

        Public Function populateModels(g As IGraphics,
                                       baseData As ggplotData,
                                       x() As Double,
                                       y() As Double,
                                       z() As Double,
                                       ggplot As ggplot,
                                       theme As Theme,
                                       legendList As List(Of IggplotLegendElement)) As IEnumerable(Of Element3D) Implements Ilayer3d.populateModels

            Dim colors As String() = Nothing
            Dim legends As IggplotLegendElement = Nothing

            If useCustomColorMaps Then
                colors = getColorSet(ggplot, g, x.Length, shape, Nothing, legends)
            ElseIf Not ggplot.base.reader.color Is Nothing Then
                colors = ggplot.base.getColors(ggplot, legends, If(shape, LegendStyles.Circle))
            End If

            Call legendList.Add(legends)

            If Not useCustomData Then
                Return createSerialData($"{baseData.x} ~ {baseData.y} ~ {baseData.z}", x, y, z, colors)
            Else
                With reader.getMapData(ggplot.data, ggplot.environment)
                    Return createSerialData(reader.ToString, .x, .y, .z, colors)
                End With
            End If
        End Function

        Private Overloads Iterator Function createSerialData(title As String,
                                                             x As Double(),
                                                             y As Double(),
                                                             z As Double(),
                                                             colors As String()) As IEnumerable(Of Element3D)
            Dim size As New Size With {
                .Width = Me.size,
                .Height = Me.size
            }
            Dim nsize As Integer = x.Length
            Dim colorList As Func(Of Integer, String)
            Dim shape = If(Me.shape, LegendStyles.Circle)

            If colors.IsNullOrEmpty Then
                ' default black
                colorList = GetVectorElement.Create(Of String)("black").Getter(Of String)
            Else
                colorList = GetVectorElement.Create(Of String)(colors).Getter(Of String)
            End If

            For i As Integer = 0 To nsize - 1
                Yield New ShapePoint With {
                    .Fill = colorList(i).GetBrush,
                    .Location = New Point3D(x(i), y(i), z(i)),
                    .Size = size,
                    .Style = shape,
                    .Label = $"({x(i)},{y(i)},{z(i)})"
                }
            Next
        End Function
    End Class
End Namespace
