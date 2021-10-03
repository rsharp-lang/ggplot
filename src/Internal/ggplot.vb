Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime

Public Class ggplot : Inherits Plot
    Implements SaveGdiBitmap

    Public Sub New(theme As Theme)
        MyBase.New(theme)
    End Sub

    Public Property data As Object
    Public Property layers As New Queue(Of ggplotLayer)
    Public Property base As ggplotLayer
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
            gridY:=theme.gridStrokeY
        )
    End Sub

    Public Function Save(stream As Stream, format As ImageFormat) As Boolean Implements SaveGdiBitmap.Save
        Dim image = Plot()
        Return image.Save(stream)
    End Function
End Class
