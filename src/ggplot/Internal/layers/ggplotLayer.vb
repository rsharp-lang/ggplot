#Region "Microsoft.VisualBasic::842e74764385dd4fdf7582d786eca586, ggplot\src\ggplot\Internal\layers\ggplotLayer.vb"

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

'   Total Lines: 151
'    Code Lines: 121
' Comment Lines: 4
'   Blank Lines: 26
'     File Size: 6.46 KB


'     Class ggplotLayer
' 
'         Properties: alpha, colorMap, reader, showLegend, useCustomColorMaps
'                     useCustomData, which, zindex
' 
'         Function: getColorSet, getFilter, initDataSet
' 
' 
' /********************************************************************************/

#End Region

Imports System.Data
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
Imports SMRUCC.Rsharp.Runtime.Vectorization
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
        Public Property alpha As Double = 1

        Protected ReadOnly Property useCustomData As Boolean
            Get
                If reader Is Nothing Then Return False

                Return Not (reader.y.StringEmpty AndAlso reader.label.StringEmpty)
            End Get
        End Property

        ''' <summary>
        ''' the local <see cref="colorMap"/> is not nothing means use the custom color maps
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' the local mapping always overrides the global mapping
        ''' </remarks>
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
                                       shape As LegendStyles?,
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
                        .style = If(shape Is Nothing, LegendStyles.Circle, shape.Value),
                        .title = ggplot.base.reader.getLegendLabel
                    }
                }

                Return colors
            ElseIf reader Is Nothing AndAlso colorMap.GetType.IsInheritsFrom(GetType(ggplotColorCustomSet), strict:=False) Then
                Dim maplevels As Integer = 30
                Dim palette As ggplotColorCustomSet = DirectCast(colorMap, ggplotColorCustomSet)
                Dim maps As Func(Of Object, String) = palette.ColorHandler(ggplot, data)
                Dim theme As Theme = ggplot.ggplotTheme
                Dim padding As New GraphicsRegion(g.Size, theme.padding)
                Dim legend As ColorMapLegend

                If TypeOf colorMap Is ggplotColorPalette Then
                    legend = New ColorMapLegend(DirectCast(palette.colorMap, String), maplevels)
                Else
                    legend = New ColorMapLegend(CLRVector.asCharacter(palette.colorMap), maplevels)
                End If

                With legend
                    .title = ggplot.base.reader.getLegendLabel
                    .tickAxisStroke = Stroke.TryParse(theme.legendTickAxisStroke).GDIObject
                    .tickFont = CSSFont.TryParse(theme.legendTickCSS).GDIObject(g.Dpi)
                    .format = theme.legendTickFormat
                    .ticks = data.CreateAxisTicks
                    .titleFont = CSSFont.TryParse(theme.legendTitleCSS).GDIObject(g.Dpi)
                End With

                legends = New legendColorMapElement With {
                    .colorMapLegend = legend,
                    .width = padding.Padding.Right * 3 / 4,
                    .height = padding.PlotRegion.Height
                }

                Return data.Select(Function(d) maps(d)).ToArray
            Else
                Dim factors As String() = ggplot.getText(reader?.color)

                If factors Is Nothing Then
                    Throw New MissingPrimaryKeyException()
                End If

                Dim maps As Func(Of Object, String) = colorMap.ColorHandler(ggplot, factors)
                Dim legendItems As LegendObject() = colorMap.TryGetFactorLegends(factors, If(shape Is Nothing, LegendStyles.Circle, shape), ggplot.ggplotTheme)
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

        Public MustOverride Function Plot(stream As ggplotPipeline) As IggplotLegendElement

        Public Function getFilter(ggplot As ggplot) As BooleanVector
            Dim i As New List(Of Object)
            Dim measure As New Environment(ggplot.environment, ggplot.environment.stackFrame, isInherits:=False)
            Dim x = DirectCast(ggplot.data, dataframe).colnames _
                .SeqIterator _
                .ToArray

            For Each var As SeqValue(Of String) In x
                Call measure.Push(var.value, Nothing, [readonly]:=False)
            Next

            Dim fields As String() = x _
                .Select(Function(xi) xi.value) _
                .ToArray

            For Each row As NamedCollection(Of Object) In DirectCast(ggplot.data, dataframe).forEachRow(fields)
                For Each var As SeqValue(Of String) In x
                    Call measure(var.value).setValue(row(var), measure)
                Next

                i.Add(REnv.single(which.Evaluate(measure)))
            Next

            Return New BooleanVector(CLRVector.asLogical(i.ToArray))
        End Function
    End Class
End Namespace
