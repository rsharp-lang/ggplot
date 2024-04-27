#Region "Microsoft.VisualBasic::9055174db3469d041233aed6cf9c58b7, G:/GCModeller/src/runtime/ggplot/src/ggplot//Internal/ggplot.vb"

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

    '   Total Lines: 287
    '    Code Lines: 220
    ' Comment Lines: 26
    '   Blank Lines: 41
    '     File Size: 10.26 KB


    ' Class ggplot
    ' 
    '     Properties: args, base, clearCanvas, data, driver
    '                 environment, ggplotTheme, is3D, layers, panelBorder
    '                 titleOffset
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    '     Function: CreateReader, CreateRender, getText, getValue, Save
    ' 
    '     Sub: Draw2DElements, DrawLegends, DrawMultiple, DrawSingle, plot3D
    '          PlotInternal, Register
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Runtime.CompilerServices
Imports ggplot.elements
Imports ggplot.elements.legend
Imports ggplot.layers
Imports ggplot.render
Imports Microsoft.VisualBasic.ComponentModel.DataStructures
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Plot3D.Device
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Imaging.Drawing3D
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports std = System.Math
Imports stdDataframe = Microsoft.VisualBasic.Math.DataFrame.DataFrame

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
    ''' <summary>
    ''' the runtime environment for current ggplot object, 
    ''' which is comes from the first 
    ''' <see cref="ggplot2.ggplot(Object, Object, Object, list, Environment)"/> 
    ''' function calls.
    ''' </summary>
    ''' <returns></returns>
    Public Property environment As Environment
    ''' <summary>
    ''' the driver flag for the graphics device
    ''' </summary>
    ''' <returns></returns>
    Public Property driver As Drivers = Drivers.GDI
    Public Property titleOffset As Double = 2
    Public Property clearCanvas As Boolean = True
    ''' <summary>
    ''' works on 2D chart plot
    ''' </summary>
    ''' <returns></returns>
    Public Property panelBorder As rectElement

    ''' <summary>
    ''' the <see cref="data"/> template
    ''' </summary>
    Protected template As Type

    Shared ReadOnly templates As New Dictionary(Of Type, Func(Of Theme, ggplot))

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Sub Register(driver As Type, activator As Func(Of Theme, ggplot))
        templates(driver) = activator
    End Sub

    Public Shared Function CreateRender(driver As Object, env As Environment, theme As Theme) As ggplot
        If driver Is Nothing Then
            Return New ggplot(theme) With {.template = Nothing, .data = driver, .environment = env}
        ElseIf TypeOf driver Is dataframe OrElse
            TypeOf driver Is stdDataframe OrElse
            TypeOf driver Is list Then

            If TypeOf driver Is stdDataframe Then
                driver = MathDataSet.toDataframe(DirectCast(driver, stdDataframe), list.empty, env)
            End If

            Return New ggplot(theme) With {
                .template = driver.GetType,
                .data = driver,
                .environment = env
            }
        Else
            Dim template As Type = driver.GetType
            Dim active As ggplot = templates(template)(theme)

            active.template = template
            active.data = driver
            active.environment = env

            Return active
        End If
    End Function

    Public Overridable Function CreateReader(mapping As ggplotReader) As ggplotBase
        Return New ggplotBase With {
            .reader = mapping
        }
    End Function

    Public Function getText(sourceData As Object) As String()
        Dim data As dataframe = DirectCast(Me.data, dataframe)
        Dim source As String() = CLRVector.asCharacter(sourceData)

        If source IsNot Nothing Then
            If source.Length = 1 AndAlso data.hasName(source(0)) Then
                Return CLRVector.asCharacter(data.getColumnVector(source(0)))
            Else
                Return source
            End If
        Else
            Return Nothing
        End If
    End Function

    Public Function getValue(source As String) As Double()
        Dim data As dataframe = DirectCast(Me.data, dataframe)

        If source IsNot Nothing AndAlso data.hasName(source) Then
            Return CLRVector.asNumeric(data.getColumnVector(source))
        Else
            Return Nothing
        End If
    End Function

    Public Shared UnionGgplotLayers As Func(Of IEnumerable(Of ggplotLayer), IEnumerable(Of ggplotLayer))

    Protected Overrides Sub PlotInternal(ByRef g As IGraphics, canvas As GraphicsRegion)
        Dim baseData As ggplotData = base.getGgplotData(Me)

        If clearCanvas Then
            Call g.Clear(theme.background.TranslateColor)
        End If

        If baseData.x Is Nothing AndAlso baseData.y Is Nothing Then
            Call Draw2DElements(g, canvas, New List(Of IggplotLegendElement))
        Else
            If base.reader.isPlain2D Then
                Call chart2D.plot2D(Me, baseData, g, canvas)
            Else
                Call plot3D(baseData, g, canvas)
            End If
        End If
    End Sub

    Private Sub plot3D(baseData As ggplotData, ByRef g As IGraphics, canvas As GraphicsRegion)
        Dim x As Double() = baseData.x.ToNumeric
        Dim y As Double() = baseData.y.ToNumeric
        Dim z As Double() = baseData.z.ToNumeric
        Dim labelColor As New SolidBrush(theme.tagColor.TranslateColor)
        Dim camera As Camera = Me.Camera(canvas.PlotRegion.Size)
        Dim legends As New List(Of IggplotLegendElement)

        Call g3d.populateModels(Me, g, baseData, x, y, z, legends) _
            .IteratesALL _
            .RenderAs3DChart(
                canvas:=g,
                camera:=camera,
                region:=canvas,
                theme:=theme
            )

        Call Draw2DElements(g, canvas, legends)
    End Sub

    Protected Friend Sub Draw2DElements(g As IGraphics, canvas As GraphicsRegion, legends As List(Of IggplotLegendElement))
        Dim legendGroups = From group As IggplotLegendElement
                           In legends.SafeQuery
                           Where Not group Is Nothing

        If Not main.StringEmpty Then
            Call DrawMainTitle(g, canvas.PlotRegion, offsetFactor:=titleOffset)
        End If
        If theme.drawLegend Then
            Call DrawLegends(
                legends:=legendGroups,
                g:=g,
                canvas:=canvas,
                pos:=Nothing
            )
        End If
    End Sub

    Protected Overloads Sub DrawLegends(legends As IEnumerable(Of IggplotLegendElement),
                                        g As IGraphics,
                                        canvas As GraphicsRegion,
                                        pos As PointF)

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

                Call DrawSingle(union, g, canvas, pos)
            Else
                Call DrawMultiple(all, g, canvas, pos)
            End If
        ElseIf all.Length = 1 Then
            Call DrawSingle(all(Scan0), g, canvas, pos)
        End If
    End Sub

    Private Sub DrawMultiple(all As IggplotLegendElement(), g As IGraphics, canvas As GraphicsRegion, pos As PointF)
        Dim width As Double = canvas.Padding.Right / (all.Length + 1)
        Dim box As Rectangle = canvas.PlotRegion
        Dim x As Double
        Dim y As Double

        If pos.IsEmpty Then
            x = box.Right + width / 4
            y = box.Top + box.Height / 3
        Else
            x = pos.X
            y = pos.Y
        End If

        For i As Integer = 0 To all.Length - 1
            all(i).Draw(g, canvas, x, y, theme)
            x += width
        Next
    End Sub

    Private Sub DrawSingle(legend As IggplotLegendElement, g As IGraphics, canvas As GraphicsRegion, pos As PointF)
        If legend.size > 0 Then
            Dim size As SizeF = legend.MeasureSize(g)
            Dim rect As Rectangle = canvas.PlotRegion
            ' default is padding right / middle in height
            Dim x As Single
            Dim y As Single

            If pos.IsEmpty Then
                x = (canvas.Padding.Right - size.Width) / 2
                x = std.Max(x, 10) + rect.Right
                y = (rect.Height - size.Height) / 2 + rect.Top
            Else
                x = pos.X
                y = pos.Y
            End If

            Call legend.Draw(g, canvas, x, y, theme)
        End If
    End Sub

    Public Function Save(stream As Stream, format As ImageFormat) As Boolean Implements SaveGdiBitmap.Save
        Dim size As SizeF = graphicsPipeline.getSize(args.slots, environment, New SizeF(1920, 1600))
        Dim image As GraphicsData = Plot($"{size.Width},{size.Height}", driver:=driver)

        Return image.Save(stream)
    End Function
End Class
