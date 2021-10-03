Imports System.Drawing.Imaging
Imports System.IO
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object

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
    End Sub

    Public Function Save(stream As Stream, format As ImageFormat) As Boolean Implements SaveGdiBitmap.Save
        Dim image = Plot()
        Return image.Save(stream)
    End Function
End Class
