﻿Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports ggplot
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports Microsoft.VisualBasic.Imaging.Physics.layout
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports SMRUCC.Rsharp.Runtime.Vectorization

Public Class ggplotTextRepelLabel : Inherits ggplotTextLabel

    Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
        Dim legend As legendGroupElement = Nothing
        Dim label_strs As String()
        Dim ggplot As ggplot.ggplot = stream.ggplot
        Dim g As IGraphics = stream.g
        Dim css As CSSEnvirnment = g.LoadEnvironment
        Dim labelStyle As Font = css.GetFont(CSSFont.TryParse(stream.theme.tagCSS))
        Dim x = stream.x
        Dim y = stream.y
        Dim scale As DataScaler = stream.scale
        Dim rect As Rectangle = stream.canvas.PlotRegion

        If useCustomData Then
            label_strs = ggplot.getText(reader.label)
        Else
            label_strs = ggplot.getText(ggplot.base.reader.label)
        End If

        If Not fontSize Is Nothing Then
            labelStyle = New Font(
                familyName:=labelStyle.Name,
                emSize:=CSng(fontSize),
                style:=labelStyle.Style
            )
        End If

        x = stream.TranslateX
        y = y.Select(Function(yi) scale.TranslateY(yi)).ToArray

        Dim nodes As Node() = New Node(x.Length - 1) {}
        Dim algo As New LabelAdjust With {.canvas = New SizeF(rect.Width, rect.Height)}
        Dim source As New List(Of PointF)
        Dim labels As New Dictionary(Of Node, TextProperties)
        Dim text_size As SizeF
        Dim arrow As New Pen(Color.Black, 2) With {
            .DashStyle = DashStyle.Dash,
            .EndCap = LineCap.ArrowAnchor
        }
        Dim colors As String() = GetColors(stream)

        For i As Integer = 0 To nodes.Length - 1
            nodes(i) = New Node() With {
                .X = x(i) - rect.Left,
                .Y = y(i) - rect.Top,
                .fixed = False,
                .size = 3
            }
            text_size = g.MeasureString(label_strs(i), labelStyle)
            source.Add(New PointF(nodes(i).X, nodes(i).Y))
            labels.Add(nodes(i), New TextProperties With {
                .Width = text_size.Width,
                .Height = text_size.Height
            })
        Next

        Call algo.Solve(nodes, labels)

        For i As Integer = 0 To nodes.Length - 1
            Dim n As Node = nodes(i)
            Dim size = labels(n)
            Dim r1 As New RectangleF(source(i).X, source(i).Y, size.Width, size.Height)
            Dim r2 As New RectangleF(n.X + rect.Left, n.Y + rect.Top, size.Width, size.Height)

            Call g.DrawLine(arrow, r1.Centre, r2.Centre)
            Call g.DrawString(label_strs(i), labelStyle, colors(i).GetBrush, r2.Centre)
        Next

        If showLegend Then
            Return legend
        Else
            Return Nothing
        End If
    End Function

    Private Function GetColors(stream As ggplotPipeline) As String()
        Dim colors As String() = Nothing
        Dim ggplot As ggplot.ggplot = stream.ggplot

        If Not useCustomData Then
            Dim y = CLRVector.asFloat(stream.y)
            Dim nsize As Integer = y.Length

            If useCustomColorMaps Then
                Dim source As Double()
                Dim reader = stream.ggplot.base.reader

                If reader.color IsNot Nothing Then
                    source = CLRVector.asNumeric(reader.getColorSource(ggplot))
                Else
                    source = CLRVector.asNumeric(y)
                End If

                colors = getColorSet(ggplot, stream.g, nsize, Nothing, source, Nothing)
            ElseIf Not ggplot.base.reader.color Is Nothing Then
                colors = ggplot.base.getColors(
                    ggplot:=ggplot,
                    legends:=Nothing,
                    shape:=Nothing
                )
            End If

            Return colors
        Else
            With Me.data
                If useCustomColorMaps Then
                    colors = getColorSet(ggplot, stream.g, .nsize, Nothing, .y, Nothing)
                ElseIf Not ggplot.base.reader.color Is Nothing Then
                    colors = ggplot.base.getColors(ggplot, Nothing, Nothing)
                Else
                    colors = (++ggplot.colors).Replicate(.nsize).ToArray
                End If

                Return colors
            End With
        End If
    End Function
End Class