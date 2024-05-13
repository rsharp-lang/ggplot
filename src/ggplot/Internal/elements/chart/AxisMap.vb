#Region "Microsoft.VisualBasic::59dc0da347aecc93b0e1487224a96ccd, src\ggplot\Internal\elements\chart\AxisMap.vb"

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

    '   Total Lines: 128
    '    Code Lines: 96
    ' Comment Lines: 12
    '   Blank Lines: 20
    '     File Size: 4.56 KB


    '     Class axisMap
    ' 
    '         Properties: mapper, range, size, value
    ' 
    '         Function: Create, FromArray, FromFactors, FromNumeric, ToFactors
    '                   ToFloat, ToInteger, ToLong, ToNumeric, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports REnv = SMRUCC.Rsharp.Runtime

Namespace elements

    ''' <summary>
    ''' the axis value mapper
    ''' </summary>
    Public Class axisMap : Implements ICTypeVector

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
        Public Function ToInteger() As Integer() Implements ICTypeVector.ToInteger
            Return CLRVector.asInteger(value)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function ToLong() As Long() Implements ICTypeVector.ToLong
            Return CLRVector.asLong(value)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function ToNumeric() As Double() Implements ICTypeVector.ToNumeric
            Return CLRVector.asNumeric(value)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function ToFloat() As Single() Implements ICTypeVector.ToFloat
            Return CLRVector.asFloat(value)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function ToFactors() As String() Implements ICTypeVector.ToFactors
            Return CLRVector.asCharacter(value)
        End Function

        Public Overrides Function ToString() As String
            If mapper = MapperTypes.Continuous Then
                Return $"[{mapper.Description}] value in range {range.ToString}"
            Else
                Return $"[{mapper.Description}] with {range.Max} group factors"
            End If
        End Function

        Public Shared Function Create(x As Object) As axisMap
            If TypeOf x Is axisMap Then
                Return x
            ElseIf x.GetType.IsArray Then
                Return FromArray(x)
            Else
                Return FromArray({x.ToString})
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
