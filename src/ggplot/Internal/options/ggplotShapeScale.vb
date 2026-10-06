Imports Microsoft.VisualBasic.Data.Plots.Plot3D.Legend
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports Microsoft.VisualBasic.Linq

Namespace options

    ''' <summary>
    ''' The shape(marker) scale of the plot.
    '''
    ''' + scale_shape(): a set of the built-in marker shapes
    ''' + scale_shape_manual(values): map the shape aesthetic onto the given
    '''   marker shape names
    '''
    ''' The resolved shape of each observation is used as the marker style of
    ''' the corresponding plot point, so that the shape aesthetic is honored
    ''' by the scatter-like layers.
    ''' </summary>
    Public MustInherit Class ggplotShapeScale : Inherits ggplotOption

        Public Property name As String = "shape"

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            If Not ggplot.args Is Nothing Then
                ggplot.args.slots("scale_shape") = Me
            End If

            Return ggplot
        End Function

        ''' <summary>
        ''' resolve the marker shape of each observation
        ''' </summary>
        Public Overridable Function Resolve(values As String()) As LegendStyles()
            If values.IsNullOrEmpty Then Return New LegendStyles() {}

            Return values.Select(Function(v) parse(v)).ToArray
        End Function

        Protected Shared Function parse(name As String) As LegendStyles
            Dim style As LegendStyles

            If System.Enum.TryParse(Of LegendStyles)(name, True, style) Then
                Return style
            End If

            Select Case name.Trim.ToLower
                Case "circle", "o" : Return LegendStyles.Circle
                Case "square", "s", "rect" : Return LegendStyles.Rectangle
                Case "triangle", "^" : Return LegendStyles.Triangle
                Case "diamond", "d" : Return LegendStyles.Diamond
                Case "hexagon", "h" : Return LegendStyles.Hexagon
                Case "line", "l" : Return LegendStyles.SolidLine
                Case "dash", "dashed" : Return LegendStyles.DashLine
                Case "none", "na" : Return LegendStyles.Rectangle
                Case Else : Return LegendStyles.Circle
            End Select
        End Function
    End Class

    ''' <summary>
    ''' scale_shape(): a set of the built-in marker shapes
    ''' </summary>
    Public Class ggplotShapeBuiltIn : Inherits ggplotShapeScale

        Public Sub New()
            name = "shape"
        End Sub
    End Class

    ''' <summary>
    ''' scale_shape_manual(values): maps the shape aesthetic onto the given
    ''' marker shape names
    ''' </summary>
    Public Class ggplotShapeManual : Inherits ggplotShapeScale

        Public Property values As String()

        Public Sub New()
            name = "shape"
        End Sub

        Public Overrides Function Resolve(values As String()) As LegendStyles()
            If Me.values.IsNullOrEmpty OrElse values.IsNullOrEmpty Then
                Return MyBase.Resolve(values)
            End If

            Dim index As New Dictionary(Of String, LegendStyles)

            For Each v As String In Me.values
                If Not index.ContainsKey(v) Then
                    Call index.Add(v, parse(v))
                End If
            Next

            Dim fallback As LegendStyles = parse(Me.values(Me.values.Length \ 2))

            Return values _
                .Select(Function(v) lookup(index, v, fallback)) _
                .ToArray
        End Function

        Private Shared Function lookup(index As Dictionary(Of String, LegendStyles), v As String, fallback As LegendStyles) As LegendStyles
            If index.ContainsKey(v) Then
                Return index(v)
            End If

            Return fallback
        End Function
    End Class
End Namespace
