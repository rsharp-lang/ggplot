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
Imports System.Runtime.CompilerServices
Imports ggplot.elements.legend
Imports ggplot.layers
Imports ggplot.layers.layer3d
Imports Microsoft.VisualBasic.ComponentModel.DataStructures
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Plot3D.Device
Imports Microsoft.VisualBasic.Data.ChartPlots.Plot3D.Model
Imports Microsoft.VisualBasic.Emit.Delegates
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Imaging.Drawing3D
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports Microsoft.VisualBasic.Math.Quantile
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime

''' <summary>
''' graphics drawing engine of the ggplot library
''' </summary>
Public Class ggplot : Inherits Plot
    Implements SaveGdiBitmap

    Protected Friend ReadOnly colors As LoopArray(Of String)

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

        Me.colors = Designer _
            .GetColors(theme.colorSet) _
            .Select(Function(c) c.ToHtmlColor) _
            .ToArray
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
    ''' <summary>
    ''' the driver flag for the graphics device
    ''' </summary>
    ''' <returns></returns>
    Public Property driver As Drivers = Drivers.GDI

    ''' <summary>
    ''' the <see cref="data"/> template
    ''' </summary>
    Protected template As Type

    Shared ReadOnly templates As New Dictionary(Of Type, Func(Of Theme, ggplot))

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Sub Register(driver As Type, activator As Func(Of Theme, ggplot))
        templates(driver) = activator
    End Sub

    Public Shared Function CreateRender(driver As Object, theme As Theme) As ggplot
        If driver Is Nothing Then
            Return New ggplot(theme) With {.template = Nothing, .data = driver}
        ElseIf TypeOf driver Is dataframe OrElse TypeOf driver Is list Then
            Return New ggplot(theme) With {.template = driver.GetType, .data = driver}
        Else
            Dim template As Type = driver.GetType
            Dim active As ggplot = templates(template)(theme)

            active.template = template
            active.data = driver

            Return active
        End If
    End Function

    Public Overridable Function CreateReader(mapping As ggplotReader) As ggplotBase
        Return New ggplotBase With {
            .reader = mapping
        }
    End Function

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
        Dim baseData As ggplotData = base.getGgplotData(Me)

        Call g.Clear(theme.background.TranslateColor)

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
                .fov = 100000,
                .viewDistance = -75,
                .angleX = 31.5,
                .angleY = 65,
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
        Dim legends As New List(Of IggplotLegendElement)

        Call populateModels(g, baseData, x, y, z, legends) _
            .IteratesALL _
            .RenderAs3DChart(
                canvas:=g,
                camera:=camera,
                region:=canvas,
                theme:=theme
            )

        Call Draw2DElements(g, canvas, legends)
    End Sub

    Private Iterator Function populateModels(g As IGraphics,
                                             baseData As ggplotData,
                                             x() As Double,
                                             y() As Double,
                                             z() As Double,
                                             legends As List(Of IggplotLegendElement)) As IEnumerable(Of Element3D())

        Dim ppi As Integer = g.Dpi
        Dim xTicks = x.Range.CreateAxisTicks
        Dim yTicks = y.Range.CreateAxisTicks
        Dim zTicks = z.Range.CreateAxisTicks
        Dim tickCss As String = CSSFont.TryParse(theme.axisTickCSS).SetFontColor(theme.mainTextColor).ToString

        ' 然后生成底部的网格
        Yield Grids.Grid1(xTicks, yTicks, (xTicks(1) - xTicks(0), yTicks(1) - yTicks(0)), zTicks.Min, showTicks:=Not theme.axisTickCSS.StringEmpty, strokeCSS:=theme.gridStrokeX, tickCSS:=tickCss).ToArray
        Yield Grids.Grid2(xTicks, zTicks, (xTicks(1) - xTicks(0), zTicks(1) - zTicks(0)), yTicks.Min, showTicks:=Not theme.axisTickCSS.StringEmpty, strokeCSS:=theme.gridStrokeX, tickCSS:=tickCss).ToArray
        Yield Grids.Grid3(yTicks, zTicks, (yTicks(1) - yTicks(0), zTicks(1) - zTicks(0)), xTicks.Max, showTicks:=Not theme.axisTickCSS.StringEmpty, strokeCSS:=theme.gridStrokeX, tickCSS:=tickCss).ToArray

        Yield AxisDraw.Axis(
            xrange:=xTicks, yrange:=yTicks, zrange:=zTicks,
            labelFontCss:=theme.axisLabelCSS,
            labels:=(xlabel, ylabel, zlabel),
            strokeCSS:=theme.axisStroke,
            arrowFactor:="1,2",
            labelColorVal:=theme.mainTextColor
        )

        For Each layer As ggplotLayer In layers.ToArray
            If layer.GetType.ImplementInterface(Of Ilayer3d) Then
                Call layers.Remove(layer)

                Yield DirectCast(layer, Ilayer3d) _
                    .populateModels(g, baseData, x, y, z, Me, theme, legends) _
                    .ToArray
            End If
        Next
    End Function

    Private Function get2DScale(rect As Rectangle,
                                [default] As (x As String(), y As Double()),
                                layerData As IEnumerable(Of ggplotData)) As DataScaler

        Dim allDataset As ggplotData() = layerData.ToArray
        Dim y As Double() = allDataset _
            .Select(Function(d)
                        Return DirectCast(REnv.asVector(Of Double)(d.y), Double())
                    End Function) _
            .IteratesALL _
            .ToArray
        Dim limitsY As Double() = REnv.asVector(Of Double)(args.getByName("range_y"))

        y = y _
            .JoinIterates([default].y) _
            .JoinIterates(limitsY) _
            .Where(Function(d) Not d.IsNaNImaginary) _
            .ToArray

        Dim hasViolin As Boolean = layers.Any(Function(layer) TypeOf layer Is ggplotViolin)

        If hasViolin OrElse layers.Any(Function(layer) TypeOf layer Is ggplotBoxplot) Then
            For Each group In ggplotGroup.getDataGroups([default].x, [default].y)
                Dim quartile As DataQuartile = group.Quartile
                Dim lowerBound = quartile.Q1 - 1.5 * quartile.IQR
                Dim upperBound = quartile.Q3 + 1.5 * quartile.IQR

                If lowerBound < 0 Then
                    If Not hasViolin Then
                        lowerBound = 0
                    End If
                End If

                y = y.JoinIterates({upperBound, lowerBound}).ToArray
            Next
        End If

        Dim yTicks = y.Range.CreateAxisTicks
        Dim scaleX = d3js.scale.ordinal.domain(values:=[default].x).range(integers:={rect.Left, rect.Right})
        Dim scaleY = d3js.scale.linear.domain(values:=yTicks).range(integers:={rect.Bottom, rect.Top})
        Dim scale As New DataScaler() With {
            .AxisTicks = (Nothing, yTicks.AsVector),
            .region = rect,
            .X = scaleX,
            .Y = scaleY
        }

        Return scale
    End Function

    Private Function get2DScale(rect As Rectangle,
                                [default] As (x As Double(), y As Double()),
                                layerData As IEnumerable(Of ggplotData)) As DataScaler

        Dim allDataset As ggplotData() = layerData.ToArray
        Dim x As Double() = allDataset.Select(Function(d) DirectCast(REnv.asVector(Of Double)(d.x), Double())).IteratesALL.ToArray
        Dim y As Double() = allDataset.Select(Function(d) DirectCast(REnv.asVector(Of Double)(d.y), Double())).IteratesALL.ToArray
        Dim limitsX As Double() = REnv.asVector(Of Double)(args.getByName("range_x"))
        Dim limitsY As Double() = REnv.asVector(Of Double)(args.getByName("range_y"))

        ' there are missing value from the 
        ' geom_vline and geom_hline
        ' function
        x = x.JoinIterates([default].x).JoinIterates(limitsX).Where(Function(d) Not d.IsNaNImaginary).ToArray
        y = y.JoinIterates([default].y).JoinIterates(limitsY).Where(Function(d) Not d.IsNaNImaginary).ToArray

        Dim xTicks = x.Range.CreateAxisTicks
        Dim yTicks = y.Range.CreateAxisTicks
        Dim scaleX = d3js.scale.linear.domain(values:=xTicks).range(integers:={rect.Left, rect.Right})
        Dim scaleY = d3js.scale.linear.domain(values:=yTicks).range(integers:={rect.Bottom, rect.Top})
        Dim scale As New DataScaler() With {
            .AxisTicks = (xTicks.AsVector, yTicks.AsVector),
            .region = rect,
            .X = scaleX,
            .Y = scaleY
        }

        Return scale
    End Function

    Private Sub plot2D(baseData As ggplotData, ByRef g As IGraphics, canvas As GraphicsRegion)
        Dim x As Array = baseData.x
        Dim y As Double() = REnv.asVector(Of Double)(baseData.y)
        Dim reverse_y As Boolean = args.getValue("scale_y_reverse", env:=environment, [default]:=False)
        Dim layers As New Queue(Of ggplotLayer)(
            collection:=If(UnionGgplotLayers Is Nothing, Me.layers, UnionGgplotLayers(Me.layers))
        )
        Dim scale As DataScaler

        If baseData.xscale = d3js.scale.scalers.linear Then
            scale = get2DScale(
                rect:=canvas.PlotRegion,
                [default]:=(DirectCast(REnv.asVector(Of Double)(x), Double()), y),
                layerData:=From layer As ggplotLayer
                           In layers
                           Let data As ggplotData = layer.initDataSet(ggplot:=Me)
                           Where Not data Is Nothing
                           Select data
            )
        Else
            scale = get2DScale(
                rect:=canvas.PlotRegion,
                [default]:=(DirectCast(REnv.asVector(Of String)(x), String()), y),
                layerData:=From layer As ggplotLayer
                           In layers
                           Let data As ggplotData = layer.initDataSet(ggplot:=Me)
                           Where Not data Is Nothing
                           Select data
            )
        End If

        If reverse_y AndAlso y.Length > 0 Then
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
            YtickFormat:=theme.YaxisTickFormat,
            xlabelRotate:=theme.xAxisRotate
        )

        Dim legends As New List(Of IggplotLegendElement)
        Dim stream As New ggplotPipeline With {
            .ggplot = Me,
            .canvas = canvas,
            .g = g,
            .scale = scale,
            .x = x,
            .y = y
        }

        Do While layers.Count > 0
            Call layers _
                .Dequeue _
                .Plot(stream) _
                .DoCall(AddressOf legends.Add)
        Loop

        Call Draw2DElements(g, canvas, legends)
    End Sub

    Protected Sub Draw2DElements(g As IGraphics, canvas As GraphicsRegion, legends As List(Of IggplotLegendElement))
        If Not main.StringEmpty Then
            Call DrawMainTitle(g, canvas.PlotRegion)
        End If
        If theme.drawLegend Then
            Call DrawLegends(From group As IggplotLegendElement In legends.SafeQuery Where Not group Is Nothing, g, canvas)
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
        Dim box As Rectangle = canvas.PlotRegion
        Dim x As Double = box.Right + width / 4
        Dim y As Double = box.Top + box.Height / 3

        For i As Integer = 0 To all.Length - 1
            all(i).Draw(g, canvas, x, y, theme)
            x += width
        Next
    End Sub

    Private Sub DrawSingle(legend As IggplotLegendElement, g As IGraphics, canvas As GraphicsRegion)
        If legend.size > 0 Then
            Dim size As SizeF = legend.MeasureSize(g)
            Dim rect As Rectangle = canvas.PlotRegion

            ' default is padding right / middle in height
            Dim x As Single = (canvas.Padding.Right - size.Width) / 2 + rect.Right
            Dim y As Single = (rect.Height - size.Height) / 2 + rect.Top

            Call legend.Draw(g, canvas, x, y, theme)
        End If
    End Sub

    Public Function Save(stream As Stream, format As ImageFormat) As Boolean Implements SaveGdiBitmap.Save
        Dim size As SizeF = graphicsPipeline.getSize(args.slots, environment, New SizeF(1920, 1600))
        Dim image As GraphicsData = Plot($"{size.Width},{size.Height}", driver:=driver)

        Return image.Save(stream)
    End Function
End Class

