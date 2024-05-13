#Region "Microsoft.VisualBasic::7d3d1500aa2e0d48e88f4365eea891e2, src\ggplot\Internal\colors\ggplotColorPalette.vb"

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

    '   Total Lines: 71
    '    Code Lines: 56
    ' Comment Lines: 4
    '   Blank Lines: 11
    '     File Size: 3.00 KB


    '     Class ggplotColorPalette
    ' 
    '         Function: ColorHandler, getColors, ToString, TryGetFactorLegends
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Linq
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports any = Microsoft.VisualBasic.Scripting

Namespace colors

    ''' <summary>
    ''' A color palette literal mapping, mapping from a term to a set of the color names
    ''' </summary>
    Public Class ggplotColorPalette : Inherits ggplotColorCustomSet

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overrides Function ToString() As String
            Return any.ToString(colorMap)
        End Function

        Public Overrides Function ColorHandler(ggplot As ggplot, factors As Array) As Func(Of Object, String)
            If factors.GetType.GetRTypeCode.IsNumeric Then
                ' level mapping
                Dim colors As String() = getColors(factorSize:=100)

                Return ggplotColorCustomSet.NumericFactorMapping(CLRVector.asNumeric(factors), colors)
            Else
                Dim factorList As String() = CLRVector.asCharacter(factors)
                Dim factorSize As Integer = factorList.Distinct.Count
                Dim colors As String() = getColors(factorSize)
                Dim check_warn As String = factor.checkSize(factorSize, factorList)

                If Not check_warn.StringEmpty Then
                    Call check_warn.Warning
                End If

                Return ggplotColorCustomSet.StringFactorMapping(factorList, colors)
            End If
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Function getColors(factorSize As Integer) As String()
            Return Designer _
                .GetColors(
                    term:=any.ToString(colorMap),
                    n:=factorSize,
                    alpha:=alpha * 255
                ) _
                .Select(Function(c) c.ARGBExpression) _
                .ToArray
        End Function

        Public Overrides Function TryGetFactorLegends(factors As Array, shape As LegendStyles, theme As Theme) As LegendObject()
            If factors.GetType.GetRTypeCode.IsNumeric Then
                Return Nothing
            End If

            Dim factorList As String() = CLRVector.asCharacter(factors)
            Dim colors As String() = Designer _
                .GetColors(any.ToString(colorMap), n:=factorList.Distinct.Count) _
                .Select(Function(c) c.ToHtmlColor) _
                .ToArray

            Return ggplotColorCustomSet.TryGetFactorLegends(factorList, colors, shape, theme)
        End Function
    End Class
End Namespace
