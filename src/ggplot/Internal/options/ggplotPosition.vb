Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace options

    ''' <summary>
    ''' The layout result of a grouped bar plot: for each category and each
    ''' series, where the bar starts and how wide/high it is.
    '''
    ''' + slot(category, series * 2): the start fraction of the bar inside the band
    ''' + slot(category, series * 2 + 1): the width fraction of the bar
    ''' + base(series, category): the starting value of the bar along the value axis
    ''' + size(series, category): the length of the bar along the value axis
    ''' </summary>
    Public Class BarLayout

        Public Property band As Double = 0.9
        Public Property slot As Double(,)
        Public Property base As Double(,)
        Public Property size As Double(,)
    End Class

    ''' <summary>
    ''' Position adjustments for the grouped geom_layers.
    '''
    ''' ggplot2 arranges the bars of the same category by the position
    ''' adjustments: side by side(dodge), stacked(stack), proportionally
    ''' stacked(fill) or just as they are(identity).
    ''' </summary>
    Public MustInherit Class ggplotPosition : Inherits ggplotOption

        Public Property name As String = "identity"

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            Return ggplot
        End Function

        Public Overridable Function Arrange(nCategory As Integer, nSeries As Integer, values As Double(,)) As BarLayout
            Return arrangeIdentity(nCategory, nSeries, values, 0.9)
        End Function

        Protected Shared Function arrangeIdentity(nCategory As Integer,
                                                  nSeries As Integer,
                                                  values As Double(,),
                                                  band As Double) As BarLayout
            Dim layout As New BarLayout With {
                .band = band,
                .base = New Double(nSeries - 1, nCategory - 1) {},
                .size = New Double(nSeries - 1, nCategory - 1) {}
            }

            layout.slot = fullBand(nCategory, nSeries, band)

            For i As Integer = 0 To nSeries - 1
                For j As Integer = 0 To nCategory - 1
                    layout.size(i, j) = values(i, j)
                Next
            Next

            Return layout
        End Function

        Protected Shared Function fullBand(nCategory As Integer, nSeries As Integer, band As Double) As Double(,)
            Dim slots As Double(,) = New Double(nCategory - 1, nSeries * 2 - 1) {}
            Dim w As Double = band / nSeries

            For j As Integer = 0 To nCategory - 1
                For i As Integer = 0 To nSeries - 1
                    slots(j, i * 2) = (i + (1 - band) / 2) * w
                    slots(j, i * 2 + 1) = w
                Next
            Next

            Return slots
        End Function

        Protected Shared Function arrangeStacked(nCategory As Integer,
                                                 nSeries As Integer,
                                                 values As Double(,),
                                                 proportional As Boolean,
                                                 reverse As Boolean) As BarLayout
            Dim layout As New BarLayout With {
                .band = 0.9,
                .base = New Double(nSeries - 1, nCategory - 1) {},
                .size = New Double(nSeries - 1, nCategory - 1) {}
            }

            layout.slot = fullBand(nCategory, nSeries, layout.band)

            For j As Integer = 0 To nCategory - 1
                Dim total As Double = 0

                For i As Integer = 0 To nSeries - 1
                    total += Math.Abs(values(i, j))
                Next

                Dim cursor As Double = 0

                For k As Integer = 0 To nSeries - 1
                    Dim i As Integer = If(reverse, nSeries - 1 - k, k)
                    Dim v As Double = values(i, j)

                    If proportional AndAlso total > 0 Then
                        layout.size(i, j) = Math.Abs(v) / total
                    Else
                        layout.size(i, j) = v
                    End If

                    layout.base(i, j) = cursor
                    cursor += layout.size(i, j)
                Next
            Next

            Return layout
        End Function

        ''' <summary>
        ''' resolve the position adjustment object from its name
        ''' </summary>
        Public Shared Function Resolve(position As Object) As ggplotPosition
            Dim obj As ggplotPosition = TryCast(position, ggplotPosition)

            If Not obj Is Nothing Then
                Return obj
            End If

            If position Is Nothing Then
                Return New ggplotPositionIdentity
            End If

            Dim name As String

            Try
                Dim vec As String() = CLRVector.asCharacter(position)

                If vec.IsNullOrEmpty Then
                    Return New ggplotPositionIdentity
                End If

                name = vec(Scan0)
            Catch
                Return New ggplotPositionIdentity
            End Try

            Select Case name.Trim.ToLower
                Case "stack" : Return New ggplotPositionStack
                Case "fill" : Return New ggplotPositionFill
                Case "dodge" : Return New ggplotPositionDodge
                Case Else : Return New ggplotPositionIdentity
            End Select
        End Function
    End Class
End Namespace
