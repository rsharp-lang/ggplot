Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
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

    Public Property data As Object
    Public Property layers As New Queue(Of ggplotLayer)
    Public Property base As ggplotBase
    Public Property args As list
    Public Property environment As Environment

    Protected Overrides Sub PlotInternal(ByRef g As IGraphics, canvas As GraphicsRegion)
        Dim baseData = base.reader.getMapData(data, environment)
        Dim x As Double() = REnv.asVector(Of Double)(baseData.x)
        Dim y As Double() = REnv.asVector(Of Double)(baseData.y)
        Dim xTicks = x.Range.CreateAxisTicks
        Dim yTicks = y.Range.CreateAxisTicks
        Dim rect As Rectangle = canvas.PlotRegion
        Dim scaleX = d3js.scale.linear.domain(xTicks).range(integers:={rect.Left, rect.Right})
        Dim scaleY = d3js.scale.linear.domain(yTicks).range(integers:={rect.Bottom, rect.Top})
        Dim scale As New DataScaler() With {
            .AxisTicks = (xTicks.AsVector, yTicks.AsVector),
            .region = rect,
            .X = scaleX,
            .Y = scaleY
        }

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

        Dim legends As New List(Of legendGroupElement)

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
            Call DrawLegends(From group As legendGroupElement In legends Where Not group Is Nothing, g, canvas)
        End If
    End Sub

    Private Overloads Sub DrawLegends(legends As IEnumerable(Of legendGroupElement), g As IGraphics, canvas As GraphicsRegion)
        Dim all = legends.ToArray
        Dim width As Double = canvas.Padding.Right / (all.Length + 1)
        Dim box = canvas.PlotRegion
        Dim x As Double = box.Right + width / 4
        Dim y As Double = box.Top + box.Height / 3

        For i As Integer = 0 To all.Length - 1
            all(i).Draw(g, canvas, x, y)
            x += width
        Next
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
