﻿#Region "Microsoft.VisualBasic::a4351e3c6628b46fdb99b281ce3df4d4, src\Internal\layers\ggplotScatter.vb"

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

'     Class ggplotScatter
' 
'         Properties: shape, size
' 
'         Function: createSerialData, Plot
' 
' 
' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.colors
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Plots
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Namespace layers

    Public Class ggplotScatter : Inherits ggplotLayer

        Public Property shape As LegendStyles
        Public Property size As Single

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim serial As SerialData
            Dim colors As String() = Nothing
            Dim legends As IggplotLegendElement = Nothing
            Dim ggplot As ggplot = stream.ggplot

            If Not useCustomData Then
                Dim x = stream.x
                Dim y = stream.y
                Dim nsize As Integer = x.Length

                If useCustomColorMaps Then
                    colors = getColorSet(ggplot, stream.g, nsize, shape, y, legends)
                ElseIf Not ggplot.base.reader.color Is Nothing Then
                    colors = ggplot.base.getColors(ggplot)
                End If

                serial = createSerialData(stream.defaultTitle, x, y, colors, size, shape, colorMap)
            Else
                With Me.data
                    If useCustomColorMaps Then
                        colors = getColorSet(ggplot, stream.g, .nsize, shape, .y, legends)
                    ElseIf Not ggplot.base.reader.color Is Nothing Then
                        colors = ggplot.base.getColors(ggplot)
                    Else
                        colors = (++ggplot.colors).Replicate(.nsize).ToArray
                        legends = New ggplotLegendElement With {
                            .legend = New LegendObject With {
                                .color = colors(Scan0),
                                .fontstyle = stream.theme.legendLabelCSS,
                                .style = LegendStyles.SolidLine,
                                .title = reader.getLegendLabel
                            }
                        }
                    End If

                    serial = createSerialData(reader.ToString, .x, .y, colors, size, shape, colorMap)
                End With
            End If

            Call Scatter2D.DrawScatter(
                g:=stream.g,
                scatter:=serial.pts,
                scaler:=stream.scale,
                fillPie:=True,
                shape:=serial.shape,
                pointSize:=serial.pointSize,
                getPointBrush:=serial.BrushHandler
            ) _
            .ToArray

            Return legends
        End Function

        Protected Friend Shared Function createSerialData(legend As String,
                                                          x As Double(),
                                                          y As Double(),
                                                          colors As String(),
                                                          size!,
                                                          shape As LegendStyles,
                                                          colorMap As ggplotColorMap) As SerialData

            Dim color As Color

            If colors Is Nothing Then
                If Not colorMap Is Nothing Then
                    color = DirectCast(colorMap, ggplotColorLiteral).ToColor
                End If
            Else
                color = Nothing
            End If

            If color.IsEmpty Then
                color = Color.Black
            End If

            Return New SerialData() With {
                .color = color,
                .pointSize = size,
                .width = size,
                .shape = shape,
                .title = legend,
                .pts = x _
                    .Select(Function(xi, i)
                                Return New PointData(xi, y(i)) With {
                                    .color = If(colors Is Nothing, Nothing, colors(i))
                                }
                            End Function) _
                    .ToArray
            }
        End Function
    End Class
End Namespace