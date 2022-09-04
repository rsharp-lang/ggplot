Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Fractions
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Math
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime
Imports stdNum = System.Math

Namespace layers

    Public Class ggplotScatterpie : Inherits ggplotLayer

        Public Property pie As String()

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            ' get scatter data
            Dim data As dataframe = stream.ggplot.data
            Dim x As Double() = stream.scale.X(stream.x)
            Dim y As Double() = stream.scale.Y(stream.y)
            Dim piedata = pie _
                .ToDictionary(Function(name) name,
                              Function(name)
                                  Return DirectCast(REnv.asVector(Of Double)(data.getVector(name, fullSize:=True)), Double())
                              End Function)
            ' evaluate cells grid
            Dim cellWidth = NumberGroups.diff(x.OrderBy(Function(xi) xi).ToArray).OrderByDescending(Function(a) a).Take(10).Average
            Dim cellHeight = NumberGroups.diff(y.OrderBy(Function(yi) yi).ToArray).OrderByDescending(Function(a) a).Take(10).Average
            Dim colors As Color() = Designer.GetColors(stream.theme.colorSet, n:=pie.Length)
            Dim radius As Single = stdNum.Min(cellWidth, cellHeight)

            For i As Integer = 0 To x.Length - 1
                Dim xi = x(i)
                Dim yi = y(i)
                Dim pie As FractionData() = Me.pie _
                    .Select(Function(name, pi)
#Disable Warning
                                Return New FractionData() With {
                                    .Name = name,
                                    .Color = colors(pi),
                                    .Value = piedata(name)(i)
                                }
#Enable Warning
                            End Function) _
                    .ToArray

                Call stream.g.PlotPie(
                    topLeft:=New Point(xi, yi),
                    data:=pie,
                    valueLabelFont:=Nothing,
                    font:=Nothing,
                    layoutRect:=Nothing,
                    r:=radius,
                    shadowAngle:=0,
                    shadowDistance:=0,
                    valueLabel:=ValueLabels.None,
                    legendAlt:=Nothing
                )
            Next

            Return New legendGroupElement With {
                .legends = pie _
                    .Select(Function(name, i)
                                Return New LegendObject With {
                                    .color = colors(i).ToHtmlColor,
                                    .fontstyle = stream.theme.legendLabelCSS,
                                    .style = LegendStyles.Rectangle,
                                    .title = name
                                }
                            End Function) _
                    .ToArray
            }
        End Function
    End Class
End Namespace