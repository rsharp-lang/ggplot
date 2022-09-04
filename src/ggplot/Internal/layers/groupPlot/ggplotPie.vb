Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Fractions
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports stdNum = System.Math

Namespace layers

    Public Class ggplotPie : Inherits ggplotGroup

        Public Property valueLabel As ValueLabels = ValueLabels.Percentage

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As OrdinalScale) As IggplotLegendElement
            Dim allGroupData = getDataGroups(stream).ToArray
            Dim colors As Func(Of Object, String) = getColors(stream, allGroupData.Select(Function(i) i.name))
            Dim colorSet = allGroupData _
                .Select(Function(group)
                            Return colors(group.name) _
                                .TranslateColor _
                                .Alpha(alpha * 255)
                        End Function) _
                .ToArray
            Dim sumAll = Aggregate group In allGroupData Into Sum(group.First)
            Dim pie As FractionData() = allGroupData _
                .Select(Function(group, i)
                            Return New FractionData With {
                                .Color = colorSet(i),
                                .Name = group.name,
                                .Value = group.First,
                                .Percentage = group.First / sumAll
                            }
                        End Function) _
                .ToArray
            Dim labelFont As Font = CSSFont.TryParse(stream.theme.tagCSS).GDIObject(stream.g.Dpi)
            Dim r As Single = stdNum.Min(stream.canvas.PlotRegion.Height, stream.canvas.PlotRegion.Width) / 2

            Call stream.g.PlotPie(
                stream.canvas, pie,
                valueLabelFont:=labelFont,
                font:=labelFont,
                layoutRect:=Nothing,
                r:=r,
                shadowAngle:=0,
                shadowDistance:=0,
                valueLabel:=valueLabel,
                legendAlt:=Nothing
            )

            Return New legendGroupElement With {
                .layout = Nothing,
                .legends = pie _
                    .Select(Function(a)
                                Return New LegendObject With {
                                    .color = a.Color.ToHtmlColor,
                                    .fontstyle = stream.theme.legendLabelCSS,
                                    .style = LegendStyles.Rectangle,
                                    .title = a.Name
                                }
                            End Function) _
                    .ToArray,
                .shapeSize = Nothing
            }
        End Function
    End Class
End Namespace