Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Linq
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports any = Microsoft.VisualBasic.Scripting

Namespace colors

    ''' <summary>
    ''' use a character vector that contains multiple color values for make item category mapping or scale mapping
    ''' </summary>
    ''' <remarks>
    ''' ``scale_fill_manual``
    ''' </remarks>
    Public Class ggplotColorCustomSet : Inherits ggplotColorMap

        Public Overrides Function ColorHandler(ggplot As ggplot, factors As Array) As Func(Of Object, String)
            If factors.GetType.GetRTypeCode.IsNumeric Then
                ' level mapping
                Return ggplotColorCustomSet.NumericFactorMapping(CLRVector.asNumeric(factors), getColors())
            Else
                Return ggplotColorCustomSet.StringFactorMapping(CLRVector.asCharacter(factors), getColors())
            End If
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overrides Function TryGetFactorLegends(factors As Array, shape As LegendStyles, theme As Theme) As LegendObject()
            If factors.GetType.GetRTypeCode.IsNumeric Then
                Return Nothing
            Else
                Return TryGetFactorLegends(CLRVector.asCharacter(factors), getColors, shape, theme)
            End If
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Function getColors() As String()
            Return CLRVector.asCharacter(colorMap)
        End Function

        Public Shared Function NumericFactorMapping(factors As Double(), colors As String()) As Func(Of Object, String)
            Dim valueRange As New DoubleRange(data:=factors)
            Dim indexRange As New DoubleRange({0, 100})

            Return Function(any)
                       Dim dbl As Double = CDbl(any)
                       Dim i As Integer = CInt(valueRange.ScaleMapping(dbl, indexRange))

                       Return colors(i)
                   End Function
        End Function

        Public Shared Function StringFactorMapping(factors As String(), colors As String()) As Func(Of Object, String)
            ' factor mapping
            Dim factorList As Index(Of String) = factors _
                .Distinct _
                .OrderBy(Function(str) str) _
                .Indexing

            Return Function(obj)
                       Dim factor As String = any.ToString(obj)
                       Dim i As Integer = factorList.IndexOf(factor)

                       Return colors(i)
                   End Function
        End Function

        Public Overloads Shared Function TryGetFactorLegends(factors As String(), colors As String(), shape As LegendStyles, theme As Theme) As LegendObject()
            Dim factorList As String() = factors _
                .Distinct _
                .OrderBy(Function(str) str) _
                .ToArray

            Return factorList _
                .Select(Function(factor, i)
                            Return New LegendObject With {
                                .color = colors(i),
                                .style = shape,
                                .title = factor,
                                .fontstyle = theme.legendLabelCSS
                            }
                        End Function) _
                .ToArray
        End Function
    End Class
End Namespace