Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

<Assembly: InternalsVisibleTo("MSImaging")>

Public Class ggplotPipeline

    Friend g As IGraphics
    Friend canvas As GraphicsRegion
    Friend x As Double()
    Friend y As Double()
    Friend scale As DataScaler
    Friend ggplot As ggplot
    Friend layout As Dictionary(Of String, PointF)

    Public ReadOnly Property theme As Theme
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return ggplot.ggplotTheme
        End Get
    End Property

    Public ReadOnly Property defaultTitle As String
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return ggplot.base.reader.ToString
        End Get
    End Property

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetRawData(Of T)() As T
        Return DirectCast(ggplot.data, T)
    End Function

End Class
