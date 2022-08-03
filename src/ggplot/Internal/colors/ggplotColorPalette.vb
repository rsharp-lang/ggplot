#Region "Microsoft.VisualBasic::e50f8035e8cb428b0efb6a8f8e879fe7, ggplot\src\ggplot\Internal\colors\ggplotColorPalette.vb"

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

    '   Total Lines: 94
    '    Code Lines: 81
    ' Comment Lines: 2
    '   Blank Lines: 11
    '     File Size: 3.87 KB


    '     Class ggplotColorPalette
    ' 
    '         Function: ColorHandler, ToString, TryGetFactorLegends
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Linq
Imports SMRUCC.Rsharp.Runtime
Imports any = Microsoft.VisualBasic.Scripting
Imports REnv = SMRUCC.Rsharp.Runtime

Namespace colors

    Public Class ggplotColorPalette : Inherits ggplotColorMap

        Public Overrides Function ToString() As String
            Return any.ToString(colorMap)
        End Function

        Public Overrides Function ColorHandler(ggplot As ggplot, factors As Array) As Func(Of Object, String)
            If factors.GetType.GetRTypeCode.IsNumeric Then
                ' level mapping
                Dim colors As String() = Designer _
                    .GetColors(
                        term:=any.ToString(colorMap),
                        n:=100,
                        alpha:=alpha * 255
                    ) _
                    .Select(Function(c) c.ARGBExpression) _
                    .ToArray
                Dim valueRange As New DoubleRange(data:=DirectCast(REnv.asVector(Of Double)(factors), Double()))
                Dim indexRange As New DoubleRange({0, 100})

                Return Function(any)
                           Dim dbl As Double = CDbl(any)
                           Dim i As Integer = CInt(valueRange.ScaleMapping(dbl, indexRange))

                           Return colors(i)
                       End Function
            Else
                ' factor mapping
                Dim factorList As Index(Of String) = factors _
                    .AsObjectEnumerator _
                    .Select(AddressOf any.ToString) _
                    .Distinct _
                    .OrderBy(Function(str) str) _
                    .Indexing
                Dim colors As String() = Designer _
                    .GetColors(
                        term:=any.ToString(colorMap),
                        n:=factorList.Count,
                        alpha:=alpha * 255
                    ) _
                    .Select(Function(c) c.ARGBExpression) _
                    .ToArray

                Return Function(obj)
                           Dim factor As String = any.ToString(obj)
                           Dim i As Integer = factorList.IndexOf(factor)

                           Return colors(i)
                       End Function
            End If
        End Function

        Public Overrides Function TryGetFactorLegends(factors As Array, shape As LegendStyles, theme As Theme) As LegendObject()
            If factors.GetType.GetRTypeCode.IsNumeric Then
                Return Nothing
            End If

            Dim factorList As String() = factors _
                .AsObjectEnumerator _
                .Select(AddressOf any.ToString) _
                .Distinct _
                .OrderBy(Function(str) str) _
                .ToArray
            Dim colors As String() = Designer _
                .GetColors(any.ToString(colorMap), n:=factorList.Length) _
                .Select(Function(c) c.ToHtmlColor) _
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
