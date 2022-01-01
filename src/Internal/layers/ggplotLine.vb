#Region "Microsoft.VisualBasic::9a29a02bf6a88a90abbc3f5742e26a62, src\Internal\layers\ggplotLine.vb"

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

'     Class ggplotLine
' 
'         Function: Plot
' 
' 
' /********************************************************************************/

#End Region

Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Plots
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Math.Interpolation

Namespace layers

    Public Class ggplotLine : Inherits ggplotLayer

        Public Property line_width As Single = 5

        Public Overrides Function Plot(
            g As IGraphics,
            canvas As GraphicsRegion,
            baseData As ggplotAdapter,
            x As Double(),
            y As Double(),
            scale As DataScaler,
            ggplot As ggplot,
            theme As Theme
        ) As IggplotLegendElement

            Dim serial As SerialData
            Dim colors As String() = Nothing
            Dim legends As IggplotLegendElement = Nothing

            If Not useCustomData Then
                Dim nsize As Integer = x.Length

                If useCustomColorMaps Then
                    colors = getColorSet(ggplot, g, nsize, LegendStyles.SolidLine, y, legends)
                ElseIf Not ggplot.base.reader.color Is Nothing Then
                    colors = ggplot.base.getColors(ggplot)
                End If

                serial = ggplotScatter.createSerialData(ggplot.base.reader.ToString, x, y, colors, line_width, LegendStyles.SolidLine, colorMap)
            Else
                With Me.data
                    If useCustomColorMaps Then
                        colors = getColorSet(ggplot, g, .nsize, LegendStyles.SolidLine, .y, legends)
                    ElseIf Not ggplot.base.reader.color Is Nothing Then
                        colors = ggplot.base.getColors(ggplot)
                    Else
                        colors = (++ggplot.colors).Replicate(.nsize).ToArray
                        legends = New ggplotLegendElement With {
                            .legend = New LegendObject With {
                                .color = colors(Scan0),
                                .fontstyle = theme.legendLabelCSS,
                                .style = LegendStyles.SolidLine,
                                .title = reader.getLegendLabel
                            }
                        }
                    End If

                    serial = ggplotScatter.createSerialData(reader.ToString, .x, .y, colors, line_width, LegendStyles.SolidLine, colorMap)
                End With
            End If

            Call LinePlot2D.DrawLine(g, canvas, scale, serial, Splines.None)

            Return legends
        End Function
    End Class
End Namespace
