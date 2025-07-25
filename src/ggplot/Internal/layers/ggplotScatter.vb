#Region "Microsoft.VisualBasic::293893b2839678ac5f467c2a98143b55, src\ggplot\Internal\layers\ggplotScatter.vb"

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

'   Total Lines: 131
'    Code Lines: 114 (87.02%)
' Comment Lines: 0 (0.00%)
'    - Xml Docs: 0.00%
' 
'   Blank Lines: 17 (12.98%)
'     File Size: 5.57 KB


'     Class ggplotScatter
' 
'         Properties: shape, size, stroke
' 
'         Function: createSerialData, GetSerialData, Plot
' 
' 
' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.InteropServices
Imports ggplot.colors
Imports ggplot.elements.legend
Imports ggplot.options
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Plots
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.Html
Imports SMRUCC.Rsharp.Runtime.Vectorization

#If NET48 Then
#Else
Imports Brush = Microsoft.VisualBasic.Imaging.Brush
#End If

Namespace layers

    Public Interface IggplotSize
        Property size As ggplotSize
    End Interface

    ''' <summary>
    ''' A scatter plot is a type of data visualization that uses dots to represent the values of two different numerical variables. 
    ''' The position of each dot on the horizontal and vertical axes corresponds to the values of the two variables being plotted. 
    ''' Scatter plots are particularly useful for showing the relationship between two variables and are a key tool in statistics 
    ''' and data analysis.
    ''' 
    ''' Here are some key aspects of scatter plots:
    ''' 
    ''' ### Purpose:
    ''' - To observe and interpret relationships between two quantitative variables.
    ''' - To identify trends, patterns, or clusters in data.
    ''' - To help in making predictions or drawing conclusions based on the observed relationships.
    ''' 
    ''' ### Components:
    ''' - **X-axis (Horizontal axis):** Represents one variable, often called the independent variable.
    ''' - **Y-axis (Vertical axis):** Represents the other variable, often called the dependent variable.
    ''' - **Data Points (Dots):** Each point represents an observation with coordinates corresponding to its values on both axes.
    ''' 
    ''' ### Types of Relationships:
    ''' - **Positive Correlation:** As the value of one variable increases, the value of the other variable also increases. The points tend to rise from left to right.
    ''' - **Negative Correlation:** As the value of one variable increases, the value of the other variable decreases. The points tend to fall from left to right.
    ''' - **No Correlation:** There is no apparent relationship between the variables. The points are scattered randomly on the plot.
    ''' - **Non-linear Relationship:** The relationship between variables is not a straight line but might be curved or follow another pattern.
    ''' 
    ''' ### Interpreting the Plot:
    ''' - **Cluster:** A group of points that are close together, indicating a concentration of data points.
    ''' - **Outlier:** A point that is distant from other points, indicating an unusual observation or a data error.
    ''' - **Trend Line (or Line of Best Fit):** A line that best represents the relationship between the variables. It can be used to make predictions.
    ''' 
    ''' ### Uses:
    ''' - In fields like economics, biology, psychology, and social sciences to understand the dynamics between variables.
    ''' - In machine learning and data science for feature selection and to understand the underlying structure of data.
    ''' </summary>
    ''' <remarks>
    ''' scatter and bubble
    ''' </remarks>
    Public Class ggplotScatter : Inherits ggplotLayer
        Implements IggplotSize

        Public Property shape As LegendStyles?
        Public Property size As ggplotSize Implements IggplotSize.size
        Public Property stroke As String

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim legends As IggplotLegendElement = Nothing
            Dim serials As SerialData() = GetSerialData(stream, legends).ToArray
            Dim brush As Func(Of PointData, Brush)

            For Each serial As SerialData In serials
                brush = serial.BrushHandler

                Call Scatter2D.DrawScatter(
                    g:=stream.g,
                    scatter:=serial.pts,
                    scaler:=stream.scale,
                    fillPie:=True,
                    shape:=serial.shape,
                    pointSize:=serial.pointSize,
                    getPointBrush:=brush,
                    strokeCss:=CSS.Stroke.TryParse(stroke, Nothing),
                    serialName:=serial.title,
                    commentText:=stream.ggplot.commentText
                ) _
                .ToArray
            Next

            Return legends
        End Function

        Protected Function getValues(ggplot As ggplot) As Double()
            Dim reader = Me.reader

            If reader Is Nothing Then
                reader = ggplot.base.reader
            End If

            Return CLRVector.asNumeric(reader.getSizeSource(ggplot))
        End Function

        Protected Function getValues(stream As ggplotPipeline) As Double()
            Return getValues(stream.ggplot)
        End Function

        Public Function GetSerialData(stream As ggplotPipeline, <Out> Optional ByRef legends As IggplotLegendElement = Nothing) As IEnumerable(Of SerialData)
            Dim colors As String() = Nothing
            Dim ggplot As ggplot = stream.ggplot
            Dim size As Single() = Nothing

            If Me.size.range Is Nothing Then
                size = Me.size.getSizeValues(stream.y).ToArray
            Else
                size = Me.size.getSizeValues(getValues(stream)).ToArray
            End If

            If ggplot.driver = Drivers.SVG Then
                size = size _
                    .Select(Function(sz) sz * stream.g.Dpi / 96) _
                    .ToArray
            End If

            If Not useCustomData Then
                Dim x = CLRVector.asFloat(stream.x)
                Dim y = CLRVector.asFloat(stream.y)
                Dim nsize As Integer = x.Length

                If useCustomColorMaps Then
                    Dim source As Double()
                    Dim reader = stream.ggplot.base.reader

                    If reader.color IsNot Nothing Then
                        source = CLRVector.asNumeric(reader.getColorSource(ggplot))
                    Else
                        source = CLRVector.asNumeric(y)
                    End If

                    colors = getColorSet(ggplot, stream.g, nsize, shape, source, legends)
                ElseIf Not ggplot.base.reader.color Is Nothing Then
                    colors = ggplot.base.getColors(
                        ggplot:=ggplot,
                        legends:=legends,
                        shape:=If(shape, LegendStyles.Circle)
                    )
                End If

                Return createSerialData(
                    stream.defaultTitle,
                    x, y, colors,
                    size,
                    TryCast(legends, legendGroupElement), shape, colorMap)
            Else
                With Me.data
                    If useCustomColorMaps Then
                        colors = getColorSet(ggplot, stream.g, .nsize, shape, .y, legends)
                    ElseIf Not ggplot.base.reader.color Is Nothing Then
                        colors = ggplot.base.getColors(ggplot, legends, shape)
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

                    Return createSerialData(
                        reader.ToString,
                        .x.ToFloat, .y.ToFloat, colors,
                        size,
                        TryCast(legends, legendGroupElement), shape, colorMap)
                End With
            End If
        End Function

        Protected Friend Shared Iterator Function createSerialData(legend As String,
                                                                   x As Single(),
                                                                   y As Single(),
                                                                   colors As String(),
                                                                   size As Single(),
                                                                   multiple_group As legendGroupElement,
                                                                   shape As LegendStyles?,
                                                                   colorMap As ggplotColorMap) As IEnumerable(Of SerialData)
            If multiple_group IsNot Nothing Then
                Dim groups As New Dictionary(Of String, List(Of PointData))
                Dim color_str As String
                Dim color As Color

                For i As Integer = 0 To x.Length - 1
                    color_str = If(colors Is Nothing, Nothing, colors(i))

                    If Not groups.ContainsKey(color_str) Then
                        Call groups.Add(color_str, New List(Of PointData))
                    End If

                    Call groups(color_str).Add(New PointData(x(i), y(i)) With {
                        .color = color_str
                    })
                Next

                Dim offset As i32 = 0

                For Each group As List(Of PointData) In groups.Values
                    ' set the default line color
                    color = group _
                        .Select(Function(c) If(c.color, "black")) _
                        .GroupBy(Function(c_str) c_str) _
                        .OrderByDescending(Function(a) a.Count) _
                        .First _
                        .Key _
                        .TranslateColor

                    Yield New SerialData() With {
                        .color = color,
                        .pointSize = size(offset),
                        .width = size(offset),
                        .shape = If(shape Is Nothing, LegendStyles.Circle, shape.Value),
                        .title = multiple_group(++offset).title,
                        .pts = group.ToArray
                    }
                Next
            Else
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

                Yield New SerialData() With {
                    .color = color,
                    .shape = If(shape Is Nothing, LegendStyles.Circle, shape.Value),
                    .title = legend,
                    .pts = x _
                        .Select(Function(xi, i)
                                    Return New PointData(xi, y(i)) With {
                                        .color = If(colors Is Nothing, Nothing, colors(i)),
                                        .value = size(i)
                                    }
                                End Function) _
                        .ToArray
                }
            End If
        End Function
    End Class
End Namespace
