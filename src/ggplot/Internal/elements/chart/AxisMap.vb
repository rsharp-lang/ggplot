Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports REnv = SMRUCC.Rsharp.Runtime

Namespace elements

    ''' <summary>
    ''' the axis value mapper
    ''' </summary>
    Public Class axisMap

        ''' <summary>
        ''' the axis data type
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property mapper As MapperTypes
        Public ReadOnly Property range As DoubleRange
        Public ReadOnly Property value As Array
        Public ReadOnly Property size As Integer
            Get
                If value.IsNullOrEmpty Then
                    Return 0
                Else
                    Return value.Length
                End If
            End Get
        End Property

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function ToNumeric() As Double()
            Return CLRVector.asNumeric(value)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function ToFloat() As Single()
            Return CLRVector.asFloat(value)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function ToFactors() As String()
            Return CLRVector.asCharacter(value)
        End Function

        Public Overrides Function ToString() As String
            If mapper = MapperTypes.Continuous Then
                Return $"[{mapper.Description}] value in range {range.ToString}"
            Else
                Return $"[{mapper.Description}] with {range.Max} group factors"
            End If
        End Function

        ''' <summary>
        ''' Create axis mapping with type auto mapping
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        Public Shared Function FromArray(value As Array) As axisMap
            If value Is Nothing Then
                Return Nothing
            End If

            value = REnv.MeltArray(value)

            If DataFramework.IsNumericType(value.GetType.GetElementType) Then
                Return FromNumeric(CLRVector.asNumeric(value))
            Else
                Return FromFactors(CLRVector.asCharacter(value))
            End If
        End Function

        Public Shared Function FromFactors(factors As String()) As axisMap
            Dim range As New DoubleRange(New Double() {0, factors.Distinct.Count})
            Dim mapper As New axisMap With {
                ._mapper = MapperTypes.Discrete,
                ._range = range,
                ._value = factors
            }

            Return mapper
        End Function

        Public Shared Function FromNumeric(x As Double()) As axisMap
            Dim range As New DoubleRange(x)
            Dim mapper As New axisMap With {
                ._mapper = MapperTypes.Continuous,
                ._range = range,
                ._value = x
            }

            Return mapper
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(axis As axisMap) As String()
            Return axis.ToFactors
        End Operator

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(axis As axisMap) As Double()
            Return axis.ToNumeric
        End Operator

    End Class
End Namespace