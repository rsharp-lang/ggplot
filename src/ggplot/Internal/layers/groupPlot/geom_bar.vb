#Region "Microsoft.VisualBasic::cbebc5c99044771197ecceeafbfafdb9, src\ggplot\Internal\layers\groupPlot\geom_bar.vb"

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

    '   Total Lines: 182
    '    Code Lines: 119 (65.38%)
    ' Comment Lines: 41 (22.53%)
    '    - Xml Docs: 82.93%
    ' 
    '   Blank Lines: 22 (12.09%)
    '     File Size: 8.63 KB


    '     Class geom_bar
    ' 
    '         Properties: position, stat
    ' 
    '         Function: aggregate_sum, getYAxis, PlotOrdinal
    ' 
    '         Sub: dataframe_bar
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.IO
Imports System.Runtime.CompilerServices
Imports ggplot.colors
Imports ggplot.elements
Imports ggplot.elements.legend
Imports ggplot.options
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Plots
Imports Microsoft.VisualBasic.Data.Plots.Canvas
Imports Microsoft.VisualBasic.Data.Plots.Plot3D.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports Microsoft.VisualBasic.Linq
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports std = System.Math

Namespace layers

    ''' <summary>
    ''' A bar plot, also known as a bar chart or bar graph, is a type of data visualization that presents categorical 
    ''' data with rectangular bars with heights or lengths proportional to the values that they represent. Bar plots
    ''' are one of the most common types of charts and are used in a wide variety of fields for their simplicity and
    ''' effectiveness in conveying information.
    ''' 
    ''' Here are the main components and characteristics of a bar plot:
    ''' 
    ''' **Components:**
    ''' 1. **Axes:** The horizontal axis (x-axis) and the vertical axis (y-axis) define the scales for the categories and the values, respectively. 
    ''' 2. **Bars:** The bars are the rectangular elements of the plot. Each bar represents a category, and its length or height corresponds to the value of that category.
    ''' 3. **Labels:** The categories are usually labeled on the x-axis, and the y-axis is often labeled with the type of value being represented (e.g., frequency, count, percentage).
    ''' 4. **Title:** The plot may have a title that describes the data being presented.
    ''' 
    ''' **Types of Bar Plots:**
    ''' - **Vertical Bar Plot:** The bars extend vertically, with the categories along the horizontal axis and the values along the vertical axis.
    ''' - **Horizontal Bar Plot:** The bars extend horizontally, with the categories along the vertical axis and the values along the horizontal axis.
    ''' - **Grouped Bar Plot:** Used to compare two or more related groups of data. The bars are grouped by category, and each group contains bars for the different subcategories being compared.
    ''' - **Stacked Bar Plot:** Each bar represents the whole, and segments within the bar represent different parts of the whole. This is useful for showing the composition of each category.
    ''' 
    ''' **When to Use Bar Plots:**
    ''' - To compare frequencies, counts, or other quantities across different categories.
    ''' - To display distributions of categorical data.
    ''' - To illustrate changes over time if each bar represents a time period.
    ''' 
    ''' **Advantages:**
    ''' - Easy to understand and interpret.
    ''' - Good for comparing different categories.
    ''' - Can be used for a wide range of data types.
    ''' 
    ''' **Disadvantages:**
    ''' - Can become cluttered if there are too many categories.
    ''' - May not be the best choice for representing continuous data.
    ''' </summary>
    Public Class geom_bar : Inherits ggplotGroup

        ''' <summary>
        ''' the bar height evaluation method, value could be 
        ''' 
        ''' 1. identity
        ''' 2. percentage
        ''' </summary>
        ''' <returns></returns>
        Public Property stat As String
        ''' <summary>
        ''' the position adjustment, could be a
        ''' <see cref="ggplotPosition"/> object or one of the
        ''' "identity", "stack", "fill" and "dodge" names
        ''' </summary>
        Public Property position As Object
        ''' <summary>
        ''' the position adjustment object, takes precedence over the
        ''' <see cref="position"/> name when it is not nothing
        ''' </summary>
        Public Property positionAdjust As ggplotPosition

        Public Overrides Function getYAxis(y() As Double, ggplot As ggplot) As axisMap
            Dim groupName As String = ggplot.base.reader.color

            If stat = "percentage" Then
                Return axisMap.FromNumeric({0, 1})
            End If

            If TypeOf ggplot.data Is dataframe Then
                Dim table As dataframe = DirectCast(ggplot.data, dataframe)

                If Not groupName.StringEmpty AndAlso table.hasName(groupName) AndAlso y.Length > 0 Then
                    Dim groupFactors As String() = CLRVector.asCharacter(table(groupName))
                    Dim zip = y.Zip(groupFactors).GroupBy(Function(z) z.Second).ToArray

                    If zip.Length > 0 Then
                        Dim max As Double = zip.Max(Function(a) a.Sum(Function(i) i.First))
                        Dim min As Double = zip.Min(Function(a) a.Sum(Function(i) i.First))

                        min = std.Min(0, min)

                        Return axisMap.FromNumeric({min, max})
                    End If
                End If

                ' 没有分组信息时按数值自身的极值确定y轴范围
                Dim lower As Double = Aggregate yi As Double In y Into Min(yi)
                Dim upper As Double = Aggregate yi As Double In y Into Max(yi)

                lower = std.Min(0, lower)

                Return axisMap.FromNumeric({lower, upper})
            Else
                ' 无分组信息(数据源不是dataframe)时，直接按数值自身的极值确定y轴范围
                Dim lower As Double = Aggregate yi As Double In y Into Min(yi)
                Dim upper As Double = Aggregate yi As Double In y Into Max(yi)

                lower = std.Min(0, lower)

                Return axisMap.FromNumeric({lower, upper})
            End If
        End Function

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As OrdinalScale) As IggplotLegendElement
            Dim groupName As String = stream.ggplot.base.reader.color
            Dim ggplot As ggplot = stream.ggplot
            Dim legends As legendGroupElement = Nothing

            If TypeOf stream.ggplot.data Is dataframe Then
                Call dataframe_bar(stream, x, legends)
            Else
                Call vector_bar(stream)
            End If

            If showLegend Then
                Return legends
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' 当数据源不是dataframe(无分组信息)时，按单一系列绘制普通条形图
        ''' </summary>
        Private Sub vector_bar(stream As ggplotPipeline)
            Dim ggplot As ggplot = stream.ggplot
            Dim y As Double() = stream.y
            Dim categories As String() = CLRVector.asCharacter(stream.x)

            If categories.Length <> y.Length Then
                Throw New InvalidDataException($"the length of the x axis({categories.Length}) and the y axis({y.Length}) data are not equal!")
            End If

            Dim values As Double() = y

            If stat = "percentage" Then
                Dim total As Double = Aggregate sum As Double In y Into Sum(sum)

                If total <> 0 Then
                    values = y.Select(Function(v) v / total).ToArray
                End If
            End If

            Call LayerRender.DrawBars(
                g:=stream.g,
                scaler:=stream.scale,
                theme:=stream.theme,
                categories:=categories,
                values:=values,
                stack:=BarPlot.StackMode.None,
                horizontal:=ggplot.ggplotTheme.flipAxis
            )
        End Sub

        ''' <summary>
        ''' resolve the fill color of every series of the bar plot, and build the
        ''' legend entries when they are not provided by the color mapper
        ''' </summary>
        Private Function resolveFill(ggplot As ggplot,
                                     legends As legendGroupElement,
                                     groupFactors As String(),
                                     colors As String()) As NamedValue(Of Color)()
            If Not legends Is Nothing AndAlso Not legends.legends.IsNullOrEmpty Then
                Return legends.legends _
                    .Select(Function(l) New NamedValue(Of Color)(l.title, l.color.TranslateColor)) _
                    .ToArray()
            End If

            Dim terms As String() = groupFactors.Distinct.ToArray

            If Not colors Is Nothing AndAlso colors.Length > 0 Then
                Dim buffer As NamedValue(Of Color)() = New NamedValue(Of Color)(terms.Length - 1) {}

                For i As Integer = 0 To terms.Length - 1
                    buffer(i) = New NamedValue(Of Color)(terms(i), colors(i Mod colors.Length).TranslateColor)
                Next

                legends = New legendGroupElement With {
                    .legends = buffer.Select(Function(v) New LegendObject With {
                        .title = v.Name,
                        .color = v.Value.ToHtmlColor,
                        .style = LegendStyles.Rectangle,
                        .fontstyle = ggplot.ggplotTheme.legendLabelCSS
                    }).ToArray
                }

                Return buffer
            End If

            Dim plain As NamedValue(Of Color)() = New NamedValue(Of Color)(terms.Length - 1) {}
            Dim fallback As Color = System.Drawing.Color.SteelBlue

            For i As Integer = 0 To terms.Length - 1
                plain(i) = New NamedValue(Of Color)(terms(i), fallback)
            Next

            Dim legendTitle As String = If(ggplot.base.reader.color Is Nothing,
                                      NameOf(ggplot.base.reader),
                                      DirectCast(ggplot.base.reader.color, String))

            legends = New legendGroupElement With {
                .legends = New LegendObject() {New LegendObject With {
                    .title = legendTitle,
                    .color = fallback.ToHtmlColor,
                    .style = LegendStyles.Rectangle,
                    .fontstyle = ggplot.ggplotTheme.legendLabelCSS
                }}
            }

            Return plain
        End Function

        Private Sub dataframe_bar(stream As ggplotPipeline, x As OrdinalScale, ByRef legends As legendGroupElement)
            Dim ggplot As ggplot = stream.ggplot
            Dim table As dataframe = DirectCast(stream.ggplot.data, dataframe)
            Dim groupName As String = ggplot.base.reader.color
            Dim groupFactors As String()
            Dim colors As String() = Nothing
            Dim y As Double() = stream.y

            ' 只有在颜色映射是一个分类调色板时才能按分组取色，
            ' 否则退化为基准图的配色或者单一颜色
            Dim isCategorical As Boolean = useCustomColorMaps AndAlso
                                          colorMap.GetType.IsInheritsFrom(GetType(ggplotColorCustomSet), strict:=False)

            If groupName.StringEmpty OrElse Not table.hasName(groupName) Then
                groupFactors = Enumerable.Repeat(".", stream.x.Length).ToArray
            Else
                groupFactors = CLRVector.asCharacter(table(groupName))
            End If

            If isCategorical Then
                colors = getColorSet(ggplot, LegendStyles.Rectangle, groupFactors, legends)
            ElseIf Not ggplot.base.reader.color Is Nothing Then
                colors = ggplot.base.getColors(ggplot, legends, LegendStyles.Rectangle)
            End If

            Dim zip = groupFactors _
                .Zip(y) _
                .Zip(CLRVector.asCharacter(stream.x)) _
                .GroupBy(Function(a) a.Second) _
                .ToArray

            Dim fill = resolveFill(ggplot, legends, groupFactors, colors)

            Dim groupData As New List(Of BarDataSample)

            For Each group In zip
                Dim sum = group _
                    .Select(Function(d) d.First) _
                    .GroupBy(Function(a) a.First) _
                    .ToDictionary(Function(a) a.Key,
                                  Function(a)
                                      Return aggregate_sum(a)
                                  End Function)

                Call groupData.Add(New BarDataSample With {
                    .tag = group.Key,
                    .data = fill _
                        .Select(Function(a) sum.TryGetValue(a.Name, [default]:=0)) _
                        .ToArray
                })
            Next

            Dim stackbars As New BarDataGroup With {
                .Samples = groupData.ToArray,
                .Serials = fill
            }
            Dim categories As String() = stackbars.Samples _
                .Select(Function(a) a.tag) _
                .ToArray
            Dim seriesNames As String() = fill _
                .Select(Function(f) f.Name) _
                .ToArray
            Dim seriesColors As Color() = fill _
                .Select(Function(f) f.Value) _
                .ToArray
            Dim values As Double(,) = New Double(seriesNames.Length - 1, categories.Length - 1) {}

            For i As Integer = 0 To seriesNames.Length - 1
                For j As Integer = 0 To categories.Length - 1
                    values(i, j) = stackbars.Samples(j).data(i)
                Next
            Next

            Call drawBars(stream, categories, seriesNames, seriesColors, values)
        End Sub

        ''' <summary>
        ''' resolve the position adjustment of this bar layer
        ''' </summary>
        Private Function resolvePosition() As ggplotPosition
            If Not positionAdjust Is Nothing Then
                Return positionAdjust
            End If

            If position Is Nothing Then
                If stat = "percentage" Then
                    Return New ggplotPositionFill
                Else
                    Return New ggplotPositionIdentity
                End If
            End If

            If TypeOf position Is String Then
                If DirectCast(position, String).StringEmpty Then
                    If stat = "percentage" Then
                        Return New ggplotPositionFill
                    Else
                        Return New ggplotPositionIdentity
                    End If
                End If
            End If

            Return ggplotPosition.Resolve(position)
        End Function

        ''' <summary>
        ''' draw the bars in the data coordinate space, so that the position
        ''' adjustments(dodge/stack/fill) take effect on the real values
        ''' </summary>
        Private Sub drawBars(stream As ggplotPipeline,
                             categories As String(),
                             seriesNames As String(),
                             seriesColors As Color(),
                             values As Double(,))
            Dim scale As DataScaler = stream.scale
            Dim nCat As Integer = categories.Length
            Dim nSer As Integer = seriesNames.Length

            If nCat = 0 OrElse nSer = 0 Then Return

            If scale.xscale <> scalers.ordinal Then
                ' 非分类轴时退化为旧引擎的比例堆叠绘制
                Call LayerRender.DrawBars(
                    g:=stream.g,
                    scaler:=scale,
                    theme:=stream.theme,
                    categories:=categories,
                    values:=Nothing,
                    colors:=seriesColors,
                    stack:=BarPlot.StackMode.Percent,
                    horizontal:=stream.theme.flipAxis,
                    multiValues:=values,
                    seriesNames:=seriesNames
                )

                Return
            End If

            Dim layout As BarLayout = resolvePosition().Arrange(nCat, nSer, values)
            Dim binWidth As Double = DirectCast(scale.X, OrdinalScale).binWidth

            For j As Integer = 0 To nCat - 1
                Dim center As Double = scale.TranslateX(categories(j))

                For i As Integer = 0 To nSer - 1
                    Dim height As Double = layout.size(i, j)

                    If height = 0 Then Continue For

                    Dim start As Double = layout.slot(j, i * 2)
                    Dim slotWidth As Double = layout.slot(j, i * 2 + 1)
                    Dim band As Double = binWidth * layout.band
                    Dim left As Double = center - band / 2 + band * start
                    Dim right As Double = left + band * slotWidth
                    Dim y0 As Double = scale.TranslateY(layout.base(i, j))
                    Dim y1 As Double = scale.TranslateY(layout.base(i, j) + height)
                    Dim bar As New RectangleF(
                        CSng(Math.Min(left, right)),
                        CSng(Math.Min(y0, y1)),
                        CSng(Math.Abs(right - left)),
                        CSng(Math.Abs(y1 - y0))
                    )

                    If bar.Width <= 0 OrElse bar.Height <= 0 Then Continue For

                    Using brush As New SolidBrush(seriesColors(i Mod seriesColors.Length))
                        Call stream.g.FillRectangle(brush, bar)
                    End Using
                Next
            Next
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Shared Function aggregate_sum(a As IEnumerable(Of (first$, second#))) As Double
            Return Aggregate xi As (first$, second#)
                   In a
                   Into Sum(xi.second)
        End Function
    End Class
End Namespace
