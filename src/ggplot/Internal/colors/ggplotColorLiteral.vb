#Region "Microsoft.VisualBasic::3d18fb2d4a63a2ff8c0774a850947042, src\ggplot\Internal\colors\ggplotColorLiteral.vb"

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

    '   Total Lines: 43
    '    Code Lines: 34
    ' Comment Lines: 3
    '   Blank Lines: 6
    '     File Size: 1.66 KB


    '     Class ggplotColorLiteral
    ' 
    '         Function: ColorHandler, ToColor, ToString, TryGetFactorLegends
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports REnv = SMRUCC.Rsharp.Runtime

Namespace colors

    ''' <summary>
    ''' A single color literal value mapping
    ''' </summary>
    Public Class ggplotColorLiteral : Inherits ggplotColorMap

        Public Overrides Function ToString() As String
            Return ToColor.ToHtmlColor
        End Function

        Public Function ToColor() As Color
            Return DirectCast(colorMap, String) _
                .TranslateColor _
                .Alpha(alpha * 255)
        End Function

        Public Overrides Function ColorHandler(ggplot As ggplot, factors As Array) As Func(Of Object, String)
            Dim literal As String = ToColor.ARGBExpression
            Return Function(any) literal
        End Function

        Public Overrides Function TryGetFactorLegends(factors As Array, shape As LegendStyles, theme As Theme) As LegendObject()
            Return CLRVector.asCharacter(factors) _
                .Select(Function(name)
                            Return New LegendObject With {
                                .color = DirectCast(colorMap, String),
                                .fontstyle = theme.legendLabelCSS,
                                .style = shape,
                                .title = name
                            }
                        End Function) _
                .ToArray
        End Function
    End Class
End Namespace
