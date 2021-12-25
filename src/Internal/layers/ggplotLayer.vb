#Region "Microsoft.VisualBasic::f9189d97d87e51dd25f9da3d32221702, src\Internal\layers\ggplotLayer.vb"

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

'     Class ggplotLayer
' 
'         Properties: colorMap, reader, showLegend, useCustomColorMaps, useCustomData
'                     which
' 
'         Function: getFilter
' 
' 
' /********************************************************************************/

#End Region

Imports ggplot.colors
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Language.Vectorization
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime

Namespace layers

    Public MustInherit Class ggplotLayer

        Public Property reader As ggplotReader
        Public Property colorMap As ggplotColorMap
        Public Property showLegend As Boolean = True
        Public Property which As Expression
        ''' <summary>
        ''' z-index, the smaller value of this index, the first that we draw this image layer
        ''' </summary>
        ''' <returns></returns>
        Public Property zindex As Integer

        Protected ReadOnly Property useCustomData As Boolean
            Get
                If reader Is Nothing Then Return False

                Return Not (reader.y.StringEmpty AndAlso reader.label.StringEmpty)
            End Get
        End Property

        Protected ReadOnly Property useCustomColorMaps As Boolean
            Get
                Return colorMap IsNot Nothing OrElse ((Not reader Is Nothing) AndAlso Not reader.color Is Nothing)
            End Get
        End Property

        Protected data As ggplotData = Nothing

        Protected Friend Overridable Function initDataSet(ggplot As ggplot) As ggplotData
            If useCustomData Then
                data = reader.getMapData(ggplot.data, ggplot.environment)
            End If

            Return data
        End Function

        Protected Function getColorSet(ggplot As ggplot,
                                       g As IGraphics,
                                       nsize As Integer,
                                       shape As LegendStyles,
                                       data As Double(),
                                       ByRef legends As IggplotLegendElement) As String()
            legends = Nothing

            If reader Is Nothing AndAlso TypeOf colorMap Is ggplotColorLiteral Then
                Dim colorString As String = DirectCast(colorMap, ggplotColorLiteral).ToString
                Dim colors As String() = Enumerable _
                    .Range(0, nsize) _
                    .Select(Function(any) colorString) _
                    .ToArray

                legends = New ggplotLegendElement With {
                    .legend = New LegendObject With {
                        .color = colorString,
                        .fontstyle = ggplot.ggplotTheme.legendLabelCSS,
                        .style = shape,
                        .title = ggplot.base.reader.getLegendLabel
                    }
                }

                Return colors
            ElseIf reader Is Nothing AndAlso TypeOf colorMap Is ggplotColorPalette Then
                Dim maplevels As Integer = 30
                Dim palette As ggplotColorPalette = DirectCast(colorMap, ggplotColorPalette)
                Dim maps As Func(Of Object, String) = palette.ColorHandler(ggplot, data)
                Dim theme As Theme = ggplot.ggplotTheme
                Dim padding As New GraphicsRegion(g.Size, theme.padding)

                legends = New legendColorMapElement With {
                    .colorMapLegend = New ColorMapLegend(palette.colorMap, maplevels) With {
                        .title = ggplot.base.reader.getLegendLabel,
                        .tickAxisStroke = Stroke.TryParse(theme.legendTickAxisStroke).GDIObject,
                        .tickFont = CSSFont.TryParse(theme.legendTickCSS).GDIObject(g.Dpi),
                        .format = theme.legendTickFormat,
                        .ticks = data.CreateAxisTicks,
                        .titleFont = CSSFont.TryParse(theme.legendTitleCSS).GDIObject(g.Dpi)
                    },
                    .width = padding.Padding.Right * 3 / 4,
                    .height = padding.PlotRegion.Height
                }

                Return data.Select(Function(d) maps(d)).ToArray
            Else
                Dim factors As String() = ggplot.getText(reader?.color)
                Dim maps As Func(Of Object, String) = colorMap.ColorHandler(ggplot, factors)
                Dim legendItems As LegendObject() = colorMap.TryGetFactorLegends(factors, shape, ggplot.ggplotTheme)
                Dim colors = factors.Select(Function(factor) maps(factor)).ToArray

                legends = New legendGroupElement With {
                    .legends = legendItems
                }

                If legendItems.IsNullOrEmpty Then
                    legends = Nothing
                End If

                Return colors
            End If
        End Function

        Public MustOverride Function Plot(
            g As IGraphics,
            canvas As GraphicsRegion,
            baseData As ggplotData,
            x As Double(),
            y As Double(),
            scale As DataScaler,
            ggplot As ggplot,
            theme As Theme
        ) As IggplotLegendElement

        Public Function getFilter(ggplot As ggplot) As BooleanVector
            Dim i As New List(Of Object)
            Dim measure As New Environment(ggplot.environment, ggplot.environment.stackFrame, isInherits:=False)
            Dim x = DirectCast(ggplot.data, dataframe).colnames _
                .SeqIterator _
                .ToArray

            For Each var In x
                Call measure.Push(var.value, Nothing, [readonly]:=False)
            Next

            For Each row As NamedCollection(Of Object) In DirectCast(ggplot.data, dataframe).forEachRow(x.Select(Function(xi) xi.value).ToArray)
                For Each var In x
                    Call measure(var.value).SetValue(row(var), measure)
                Next

                i.Add(REnv.single(which.Evaluate(measure)))
            Next

            Return New BooleanVector(REnv.asLogical(i.ToArray))
        End Function
    End Class
End Namespace
