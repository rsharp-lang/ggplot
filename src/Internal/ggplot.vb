Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime

''' <summary>
''' graphics drawing engine of the ggplot library
''' </summary>
Public Class ggplot : Inherits Plot
    Implements SaveGdiBitmap

    Public ReadOnly Property ggplotTheme As Theme
        Get
            Return theme
        End Get
    End Property

    Public Sub New(theme As Theme)
        MyBase.New(theme)
    End Sub

    ''' <summary>
    ''' dataframe, list, and others
    ''' </summary>
    ''' <returns></returns>
    Public Property data As Object
    Public Property layers As New List(Of ggplotLayer)
    Public Property base As ggplotBase
    Public Property args As list
    Public Property environment As Environment

    Public Function getText(source As String) As String()
        Return REnv.asVector(Of String)(DirectCast(data, dataframe).getColumnVector(source))
    End Function

    Public Function getValue(source As String) As Double()
        Return REnv.asVector(Of Double)(DirectCast(data, dataframe).getColumnVector(source))
    End Function

    Private Sub reverse(ByRef vec As Double())
        Dim max As Double = vec.Max
        Dim min As Double = vec.Min

        For i As Integer = 0 To vec.Length - 1
            vec(i) = max - vec(i) + min
        Next
    End Sub

    Public Shared UnionGgplotLayers As Func(Of IEnumerable(Of ggplotLayer), IEnumerable(Of ggplotLayer))

    Protected Overrides Sub PlotInternal(ByRef g As IGraphics, canvas As GraphicsRegion)
        Dim baseData As ggplotData = base.reader.getMapData(data, environment)
        Dim x As Double() = REnv.asVector(Of Double)(baseData.x)
        Dim y As Double() = REnv.asVector(Of Double)(baseData.y)
        Dim xTicks = x.Range.CreateAxisTicks
        Dim yTicks = y.Range.CreateAxisTicks
        Dim rect As Rectangle = canvas.PlotRegion
        Dim scaleX = d3js.scale.linear.domain(xTicks).range(integers:={rect.Left, rect.Right})
        Dim scaleY = d3js.scale.linear.domain(yTicks).range(integers:={rect.Bottom, rect.Top})
        Dim reverse_y As Boolean = args.getValue("scale_y_reverse", env:=environment, [default]:=False)
        Dim scale As New DataScaler() With {
            .AxisTicks = (xTicks.AsVector, yTicks.AsVector),
            .region = rect,
            .X = scaleX,
            .Y = scaleY
        }
        Dim layers As New Queue(Of ggplotLayer)(
            collection:=If(UnionGgplotLayers Is Nothing, Me.layers, UnionGgplotLayers(Me.layers))
        )

        If reverse_y Then
            Call reverse(y)
        End If

        Call Axis.DrawAxis(
            g, canvas,
            scaler:=scale,
            showGrid:=theme.drawGrid,
            xlabel:=xlabel,
            ylabel:=ylabel,
            gridFill:=theme.gridFill,
            axisStroke:=theme.axisStroke,
            gridX:=theme.gridStrokeX,
            gridY:=theme.gridStrokeY,
            labelFont:=theme.axisLabelCSS,
            tickFontStyle:=theme.axisTickCSS,
            XtickFormat:=theme.XaxisTickFormat,
            YtickFormat:=theme.YaxisTickFormat
        )

        Dim legends As New List(Of IggplotLegendElement)

        Do While layers.Count > 0
            Call layers _
                .Dequeue _
                .Plot(g, canvas, baseData, x, y, scale, ggplot:=Me, theme:=theme) _
                .DoCall(AddressOf legends.Add)
        Loop

        If Not main.StringEmpty Then
            Call DrawMainTitle(g, rect)
        End If
        If theme.drawLegend Then
            Call DrawLegends(From group As IggplotLegendElement In legends Where Not group Is Nothing, g, canvas)
        End If
    End Sub

    Private Overloads Sub DrawLegends(legends As IEnumerable(Of IggplotLegendElement), g As IGraphics, canvas As GraphicsRegion)
        Dim all As IggplotLegendElement() = legends.ToArray

        If all.Length > 1 Then
            If all.All(Function(l) TypeOf l Is ggplotLegendElement) Then
                Dim union As New legendGroupElement With {
                    .legends = all _
                        .Select(Function(l)
                                    Return DirectCast(l, ggplotLegendElement).legend
                                End Function) _
                        .ToArray
                }

                Call DrawSingle(union, g, canvas)
            Else
                Call DrawMultiple(all, g, canvas)
            End If
        ElseIf all.Length = 1 Then
            Call DrawSingle(all(Scan0), g, canvas)
        End If
    End Sub

    Private Sub DrawMultiple(all As IggplotLegendElement(), g As IGraphics, canvas As GraphicsRegion)
        Dim width As Double = canvas.Padding.Right / (all.Length + 1)
        Dim box = canvas.PlotRegion
        Dim x As Double = box.Right + width / 4
        Dim y As Double = box.Top + box.Height / 3

        For i As Integer = 0 To all.Length - 1
            all(i).Draw(g, canvas, x, y)
            x += width
        Next
    End Sub

    Private Sub DrawSingle(legend As IggplotLegendElement, g As IGraphics, canvas As GraphicsRegion)
        Dim size As SizeF = legend.MeasureSize(g)
        Dim rect = canvas.PlotRegion

        ' default is padding right / middle in height
        Dim x As Single = (canvas.Padding.Right - size.Width) / 2 + rect.Right
        Dim y As Single = (rect.Height - size.Height) / 2 + rect.Top

        Call legend.Draw(g, canvas, x, y)
    End Sub

    Public Function Save(stream As Stream, format As ImageFormat) As Boolean Implements SaveGdiBitmap.Save
        Dim size As New Size(1920, 1600)

        If {"w", "h"}.All(AddressOf args.hasName) Then
            size = New Size(
                width:=args.getValue(Of Integer)("w", environment, 1920),
                height:=args.getValue(Of Integer)("h", environment, 1600)
            )
        ElseIf {"width", "height"}.All(AddressOf args.hasName) Then
            size = New Size(
                width:=args.getValue(Of Integer)("width", environment, 1920),
                height:=args.getValue(Of Integer)("height", environment, 1600)
            )
        ElseIf args.hasName("size") Then
            size = InteropArgumentHelper _
                .getSize(args.getByName("size"), environment, "1920,1600") _
                .SizeParser
        End If

        Dim image = Plot($"{size.Width},{size.Height}")
        Return image.Save(stream)
    End Function
End Class
