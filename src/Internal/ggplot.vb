#Region "Microsoft.VisualBasic::28216d8e7a111ac3d90f79b715b200e5, src\Internal\ggplot.vb"

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

' Class ggplot
' 
'     Properties: args, base, data, environment, ggplotTheme
'                 layers
' 
'     Constructor: (+1 Overloads) Sub New
' 
'     Function: getText, getValue, Save
' 
'     Sub: DrawLegends, DrawMultiple, DrawSingle, PlotInternal, reverse
' 
' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports ggplot.elements.legend
Imports ggplot.layers
Imports ggplot.layers.layer3d
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Plot3D.Device
Imports Microsoft.VisualBasic.Data.ChartPlots.Plot3D.Model
Imports Microsoft.VisualBasic.Emit.Delegates
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing3D
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports Microsoft.VisualBasic.MIME.Html.CSS
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

    Public ReadOnly Property is3D As Boolean
        Get
            Return Not base.reader.isPlain2D
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

        If base.reader.isPlain2D Then
            Call plot2D(baseData, g, canvas)
        Else
            Call plot3D(baseData, g, canvas)
        End If
    End Sub

    Private Function Camera(plotSize As Size) As Camera
        Dim cameraVal As Object = args.getByName("camera")

        If cameraVal Is Nothing Then
            Return New Camera With {
                .screen = plotSize,
                .fov = 10000,
                .viewDistance = -75,
                .angleX = 30,
                .angleY = 30,
                .angleZ = 125
            }
        Else
            With DirectCast(cameraVal, Camera)
                .screen = plotSize
                Return cameraVal
            End With
        End If
    End Function

    Private Sub plot3D(baseData As ggplotData, ByRef g As IGraphics, canvas As GraphicsRegion)
        Dim x As Double() = REnv.asVector(Of Double)(baseData.x)
        Dim y As Double() = REnv.asVector(Of Double)(baseData.y)
        Dim z As Double() = REnv.asVector(Of Double)(baseData.z)
        Dim labelColor As New SolidBrush(theme.tagColor.TranslateColor)
        Dim camera As Camera = Me.Camera(canvas.PlotRegion.Size)

        Call populateModels(g, baseData, x, y, z) _
            .IteratesALL _
            .RenderAs3DChart(
                canvas:=g,
                camera:=camera,
                region:=canvas,
                labelFont:=CSSFont.TryParse(theme.tagCSS).GDIObject(g.Dpi),
                labelerItr:=0,
                showLabel:=theme.drawLabels,
                labelColor:=labelColor
            )
    End Sub

    Private Iterator Function populateModels(g As IGraphics,
                                             baseData As ggplotData,
                                             x() As Double,
                                             y() As Double,
                                             z() As Double) As IEnumerable(Of Element3D())

        Dim ppi As Integer = g.Dpi
        Dim axisLabelFont As Font = CSSFont.TryParse(theme.axisLabelCSS).GDIObject(ppi)
        Dim xTicks = x.Range.CreateAxisTicks
        Dim yTicks = y.Range.CreateAxisTicks
        Dim zTicks = z.Range.CreateAxisTicks

        ' 然后生成底部的网格
        Yield Grids.Grid1(xTicks, yTicks, (xTicks(1) - xTicks(0), yTicks(1) - yTicks(0)), zTicks.Min).ToArray
        Yield Grids.Grid2(xTicks, zTicks, (xTicks(1) - xTicks(0), zTicks(1) - zTicks(0)), yTicks.Min).ToArray
        Yield Grids.Grid3(yTicks, zTicks, (yTicks(1) - yTicks(0), zTicks(1) - zTicks(0)), xTicks.Max).ToArray

        Yield AxisDraw.Axis(
            xrange:=xTicks, yrange:=yTicks, zrange:=zTicks,
            labelFont:=axisLabelFont,
            labels:=(xlabel, ylabel, zlabel),
            strokeCSS:=theme.axisStroke,
            arrowFactor:="2,2"
        )

        For Each layer As ggplotLayer In layers.ToArray
            If layer.GetType.ImplementInterface(Of Ilayer3d) Then
                Call layers.Remove(layer)

                Yield DirectCast(layer, Ilayer3d) _
                    .populateModels(g, baseData, x, y, z, Me, theme) _
                    .ToArray
            End If
        Next
    End Function

    Private Sub plot2D(baseData As ggplotData, ByRef g As IGraphics, canvas As GraphicsRegion)
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

