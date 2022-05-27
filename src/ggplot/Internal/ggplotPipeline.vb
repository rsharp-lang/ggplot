Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

<Assembly: InternalsVisibleTo("MSImaging")>
<Assembly: InternalsVisibleTo("ggraph")>

Public Class ggplotPipeline

    Friend g As IGraphics
    Friend canvas As GraphicsRegion
    Friend x As Array
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

    Public Function TranslateX() As Double()
        If scale.X.type = d3js.scale.scalers.linear Then
            Return (From xi As Object
                    In x.AsQueryable
                    Select scale.TranslateX(CDbl(xi))).ToArray
        Else
            Return (From xi As String
                    In DirectCast(x, String())
                    Select scale.TranslateX(xi)).ToArray
        End If
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetRawData(Of T)() As T
        Return DirectCast(ggplot.data, T)
    End Function

End Class
