Imports System.Drawing

Namespace options

    ''' <summary>
    ''' The coordinate system of a ggplot plot.
    '''
    ''' This implementation keeps a single plot panel, so the coordinate
    ''' systems are all post-processing of the plot region:
    '''
    ''' + cartesian: the default, restricts the x/y limits
    ''' + fixed: keeps a fixed aspect ratio between the x and the y axis
    ''' + polar: maps the (r, theta) data to the canvas polar coordinates
    '''
    ''' The xlim/ylim of the cartesian coordinate system are written into the
    ''' range_x/range_y plot arguments, which are already respected by the 2D
    ''' scale builder.
    ''' </summary>
    Public MustInherit Class ggplotCoord : Inherits ggplotOption

        Public Property name As String = "cartesian"
        ''' <summary>
        ''' the multipler of the data expansion, 0 means no expansion
        ''' </summary>
        Public Property expand As Double = 0.05

        ''' <summary>
        ''' adjust the plot region before drawing
        ''' </summary>
        ''' <param name="rect">the raw plot region</param>
        ''' <returns></returns>
        Public Overridable Function Finalize(rect As Rectangle) As Rectangle
            Return rect
        End Function

        ''' <summary>
        ''' write the coordinate limits into the plot arguments
        ''' </summary>
        Public Overrides Function Config(ggplot As ggplot) As ggplot
            Return ggplot
        End Function

        ''' <summary>
        ''' apply the coordinate transform to a data point
        ''' </summary>
        Public Overridable Function Transform(point As PointF) As PointF
            Return point
        End Function
    End Class

    ''' <summary>
    ''' coord_cartesian(): the default cartesian coordinate system, expands the
    ''' plot limits without dropping the out of range observations.
    ''' </summary>
    Public Class ggplotCoordCartesian : Inherits ggplotCoord

        Public Property xlim As Double()
        Public Property ylim As Double()

        Public Sub New()
            name = "cartesian"
        End Sub

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            If Not xlim Is Nothing AndAlso xlim.Length >= 2 Then
                ggplot.args.slots("range_x") = xlim
            End If
            If Not ylim Is Nothing AndAlso ylim.Length >= 2 Then
                ggplot.args.slots("range_y") = ylim
            End If

            Return ggplot
        End Function
    End Class

    ''' <summary>
    ''' coord_fixed(): keeps a fixed aspect ratio between the x and the y axis,
    ''' so that one unit on the x axis has the same physical length as one unit
    ''' on the y axis.
    ''' </summary>
    Public Class ggplotCoordFixed : Inherits ggplotCoord

        Public Property ratio As Double = 1
        Public Property xlim As Double()
        Public Property ylim As Double()

        Public Sub New()
            name = "fixed"
        End Sub

        Public Overrides Function Finalize(rect As Rectangle) As Rectangle
            If ratio <= 0 Then Return rect

            Dim cx As Double = rect.Left + rect.Width / 2
            Dim cy As Double = rect.Top + rect.Height / 2
            Dim w As Double = rect.Width
            Dim h As Double = w / ratio

            If h > rect.Height Then
                h = rect.Height
                w = h * ratio
            End If

            Return New Rectangle(
                CInt(cx - w / 2),
                CInt(cy - h / 2),
                CInt(w),
                CInt(h)
            )
        End Function

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            If Not xlim Is Nothing AndAlso xlim.Length >= 2 Then
                ggplot.args.slots("range_x") = xlim
            End If
            If Not ylim Is Nothing AndAlso ylim.Length >= 2 Then
                ggplot.args.slots("range_y") = ylim
            End If

            Return ggplot
        End Function
    End Class

    ''' <summary>
    ''' coord_polar(): uses polar coordinates, where the x axis is mapped to
    ''' the theta(angle) and the y axis is mapped to the radius.
    '''
    ''' The transform is applied to the already scaled canvas points, so that
    ''' the underlying data coordinate system is not affected.
    '''
    ''' + theta: "x" or "y", which axis is mapped to the angle
    ''' + start: the offset of the angle range, in radians
    ''' + end: the end of the angle range, in radians
    ''' + direction: 1 for the counterclockwise, -1 for the clockwise
    ''' + trans: use the square root transform of the radius when true
    ''' </summary>
    Public Class ggplotCoordPolar : Inherits ggplotCoord

        Public Property theta As String = "x"
        Public Property start As Double = 0
        Public Property [end] As Double = Math.PI * 2
        Public Property direction As Double = 1
        Public Property trans As Boolean = False

        Public Sub New()
            name = "polar"
        End Sub

        Public Overrides Function Finalize(rect As Rectangle) As Rectangle
            Dim w As Integer = Math.Min(rect.Width, rect.Height)

            Return New Rectangle(
                rect.Left + (rect.Width - w) \ 2,
                rect.Top + (rect.Height - w) \ 2,
                w,
                w
            )
        End Function

        ''' <summary>
        ''' project a normalized point(in the unit square) to the polar plane
        ''' </summary>
        ''' <param name="point">
        ''' the normalized point, both components are in the range [0,1]
        ''' </param>
        ''' <returns>the normalized point on the polar plane</returns>
        Public Function Project(point As PointF) As PointF
            Dim r As Double = If(trans, Math.Sqrt(point.Y), point.Y)
            Dim a As Double = start + (point.X * ([end] - start) * direction)
            Dim x As Double = 0.5 + r * Math.Cos(a) / 2
            Dim y As Double = 0.5 - r * Math.Sin(a) / 2

            Return New PointF(CSng(x), CSng(y))
        End Function
    End Class
End Namespace