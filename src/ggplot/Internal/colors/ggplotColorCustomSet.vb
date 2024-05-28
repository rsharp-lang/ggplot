#Region "Microsoft.VisualBasic::a034c8f7767dd61575f337abe586cd6c, src\ggplot\Internal\colors\ggplotColorCustomSet.vb"

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

    '   Total Lines: 95
    '    Code Lines: 68 (71.58%)
    ' Comment Lines: 14 (14.74%)
    '    - Xml Docs: 85.71%
    ' 
    '   Blank Lines: 13 (13.68%)
    '     File Size: 4.03 KB


    '     Class ggplotColorCustomSet
    ' 
    '         Function: ColorHandler, getColors, NumericFactorMapping, StringFactorMapping, (+2 Overloads) TryGetFactorLegends
    ' 
    ' 
    ' /********************************************************************************/

#End Region

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

        ''' <summary>
        ''' get color mapper from a character vector or numeric vector
        ''' </summary>
        ''' <param name="ggplot"></param>
        ''' <param name="factors"></param>
        ''' <returns></returns>
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
