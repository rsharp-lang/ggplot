Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Imaging.d3js.scale

Namespace elements

    Public Class axisMap

        Public ReadOnly Property mapper As MapperTypes
        Public ReadOnly Property range As DoubleRange

        Public Overrides Function ToString() As String
            If mapper = MapperTypes.Continuous Then
                Return $"[{mapper.Description}] value in range {range.ToString}"
            Else
                Return $"[{mapper.Description}] with {range.Max} group factors"
            End If
        End Function

        Public Shared Function FromFactors(factors As String()) As axisMap
            Dim range As New DoubleRange(New Double() {0, factors.Distinct.Count})
            Dim mapper As New axisMap With {
                ._mapper = MapperTypes.Discrete,
                ._range = range
            }

            Return mapper
        End Function

        Public Shared Function FromNumeric(x As Double()) As axisMap
            Dim range As New DoubleRange(x)
            Dim mapper As New axisMap With {
                ._mapper = MapperTypes.Continuous,
                ._range = range
            }

            Return mapper
        End Function

    End Class
End Namespace