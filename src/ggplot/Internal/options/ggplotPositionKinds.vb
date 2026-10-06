Namespace options

    ''' <summary>
    ''' position_identity(): the default, every bar is drawn from the baseline
    ''' </summary>
    Public Class ggplotPositionIdentity : Inherits ggplotPosition
        Public Sub New()
            name = "identity"
        End Sub
    End Class

    ''' <summary>
    ''' position_stack(): stack the bars of the same category on top of each other
    ''' </summary>
    Public Class ggplotPositionStack : Inherits ggplotPosition
        Public Property reverse As Boolean = False

        Public Sub New()
            name = "stack"
        End Sub

        Public Overrides Function Arrange(nCategory As Integer, nSeries As Integer, values As Double(,)) As BarLayout
            Return arrangeStacked(nCategory, nSeries, values, False, reverse)
        End Function
    End Class

    ''' <summary>
    ''' position_fill(): stack the bars proportionally so that every category
    ''' fills the same height
    ''' </summary>
    Public Class ggplotPositionFill : Inherits ggplotPosition
        Public Property reverse As Boolean = False

        Public Sub New()
            name = "fill"
        End Sub

        Public Overrides Function Arrange(nCategory As Integer, nSeries As Integer, values As Double(,)) As BarLayout
            Return arrangeStacked(nCategory, nSeries, values, True, reverse)
        End Function
    End Class

    ''' <summary>
    ''' position_dodge(): place the bars of the same category side by side
    ''' </summary>
    Public Class ggplotPositionDodge : Inherits ggplotPosition
        Public Property width As Double = 0.9

        Public Sub New()
            name = "dodge"
        End Sub

        Public Overrides Function Arrange(nCategory As Integer, nSeries As Integer, values As Double(,)) As BarLayout
            Dim layout As New BarLayout With {
                .band = 1,
                .base = New Double(nSeries - 1, nCategory - 1) {},
                .size = New Double(nSeries - 1, nCategory - 1) {}
            }
            layout.slot = fullBand(nCategory, nSeries, width)
            layout.band = 1

            For i As Integer = 0 To nSeries - 1
                For j As Integer = 0 To nCategory - 1
                    layout.size(i, j) = values(i, j)
                Next
            Next

            Return layout
        End Function
    End Class

    ''' <summary>
    ''' position_jitter(): adds a small amount of random noise to the point
    ''' position so that the overlapping observations become visible.
    '''
    ''' + width: the amount of the horizontal jitter
    ''' + height: the amount of the vertical jitter
    ''' + seed: the random seed, so that the jitter is reproducible
    ''' </summary>
    Public Class ggplotPositionJitter : Inherits ggplotPosition

        Public Property jitterWidth As Double = 0.4
        Public Property jitterHeight As Double = 0
        Public Property seed As Integer

        Public Sub New()
            name = "jitter"
        End Sub

        ''' <summary>
        ''' generate the random offsets of each observation
        ''' </summary>
        ''' <param name="count">the number of the observations</param>
        ''' <returns>the (dx, dy) offsets of each observation</returns>
        Public Function Offsets(count As Integer) As (dx As Double(), dy As Double())
            Dim dx As Double() = New Double(count - 1) {}
            Dim dy As Double() = New Double(count - 1) {}

            If count = 0 Then
                Return (dx, dy)
            End If

            Dim random As New Random(seed)

            For i As Integer = 0 To count - 1
                dx(i) = (random.NextDouble() - 0.5) * jitterWidth
                dy(i) = (random.NextDouble() - 0.5) * jitterHeight
            Next

            Return (dx, dy)
        End Function
    End Class
End Namespace
