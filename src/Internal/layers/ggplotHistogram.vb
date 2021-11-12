#Region "Microsoft.VisualBasic::caf8636e7acf3e31c95afc6c8808ee0a, src\Internal\layers\ggplotHistogram.vb"

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

'     Class ggplotHistogram
' 
'         Function: Plot
' 
' 
' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.colors
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots.BarPlot.Histogram
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Math.Distributions.BinBox

Namespace layers

    Public Class ggplotHistogram : Inherits ggplotLayer

        Public Property bins As Integer

        Dim binData As DataBinBox(Of Double)()

        Public Overrides Function Plot(
            g As IGraphics,
            canvas As GraphicsRegion,
            baseData As ggplotData,
            x As Double(),
            y As Double(),
            scale As DataScaler,
            ggplot As ggplot,
            theme As Theme
        ) As IggplotLegendElement

            Dim color As String = getColorName(ggplot)
            Dim legend As New LegendObject With {
                .color = color,
                .fontstyle = theme.legendLabelCSS,
                .style = LegendStyles.Rectangle,
                .title = ggplot.base.reader.ToString
            }
            Dim histData As HistProfile = binData.NewModel(legend)
            Dim colorData As New NamedValue(Of Color) With {
                .Name = legend.title,
                .Value = color.TranslateColor
            }

            Call HistogramPlot.DrawSample(g, canvas.PlotRegion, histData, colorData, scale)

            Return New ggplotLegendElement With {
                .legend = legend
            }
        End Function

        Private Function getColorName(ggplot As ggplot) As String
            If useCustomColorMaps Then
                Return DirectCast(colorMap, ggplotColorLiteral).ToString
            ElseIf ggplot.base.reader.color Is Nothing Then
                Return "black"
            Else
                Return ggplot.base.reader.color.ToString
            End If
        End Function

        Friend Shared Sub configHistogram(ggplot As ggplot, hist As ggplotHistogram)
            Dim data As ggplotData

            If hist.useCustomData Then
                data = hist.reader.getMapData(ggplot.data, ggplot.environment)
            Else
                data = ggplot.base.reader.getMapData(ggplot.data, ggplot.environment)
            End If

            Dim dataX As Double() = data.x
            Dim bins = CutBins _
                .FixedWidthBins(dataX, k:=hist.bins, Function(xi) xi) _
                .ToArray
            Dim y As Double() = bins.Select(Function(d) CDbl(d.Count)).ToArray

            hist.binData = bins
            ggplot.base.data.Add("y", y)

            If ggplot.ylabel.StringEmpty Then
                ggplot.ylabel = "Count"
            End If
        End Sub
    End Class
End Namespace
