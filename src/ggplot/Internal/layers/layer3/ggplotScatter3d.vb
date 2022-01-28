#Region "Microsoft.VisualBasic::1356bdcd9ebd46b80487644a7cf02b87, src\Internal\layers\layer3\ggplotScatter3d.vb"

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

'     Class ggplotScatter3d
' 
'         Function: Plot
' 
' 
' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Plot3D.Device
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing3D

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
            Dim legends As legendGroupElement = Nothing

            If useCustomColorMaps Then
                colors = getColorSet(ggplot, g, x.Length, shape, Nothing, legends)
            ElseIf Not ggplot.base.reader.color Is Nothing Then
                colors = ggplot.base.getColors(ggplot, legends, shape)
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

            For i As Integer = 0 To nsize - 1
                Yield New ShapePoint With {
                    .Fill = colors(i).GetBrush,
                    .Location = New Point3D(x(i), y(i), z(i)),
                    .Size = size,
                    .Style = shape,
                    .Label = $"({x(i)},{y(i)},{z(i)})"
                }
            Next
        End Function
    End Class
End Namespace
