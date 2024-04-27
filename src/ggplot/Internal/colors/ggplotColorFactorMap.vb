#Region "Microsoft.VisualBasic::ed650df4ce5abe1b01a9a210c8f69ce4, G:/GCModeller/src/runtime/ggplot/src/ggplot//Internal/colors/ggplotColorFactorMap.vb"

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

    '   Total Lines: 49
    '    Code Lines: 38
    ' Comment Lines: 3
    '   Blank Lines: 8
    '     File Size: 2.00 KB


    '     Class ggplotColorFactorMap
    ' 
    '         Function: ColorHandler, GetLegends, ToString, TryGetFactorLegends
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization.JSON
Imports any = Microsoft.VisualBasic.Scripting

Namespace colors

    ''' <summary>
    ''' used the field value as color factor
    ''' </summary>
    Public Class ggplotColorFactorMap : Inherits ggplotColorMap

        Private Iterator Function GetLegends(shape As LegendStyles, cssfont As String) As IEnumerable(Of LegendObject)
            For Each [class] In DirectCast(colorMap, Dictionary(Of String, String))
                Yield New LegendObject With {
                    .color = [class].Value,
                    .style = shape,
                    .title = [class].Key,
                    .fontstyle = cssfont
                }
            Next
        End Function

        Public Overrides Function ToString() As String
            Return DirectCast(colorMap, Dictionary(Of String, String)).GetJson
        End Function

        Public Overrides Function ColorHandler(ggplot As ggplot, factors As Array) As Func(Of Object, String)
            Dim colorMap As Dictionary(Of String, String) = Me.colorMap
            Dim alphaColors As New Dictionary(Of String, String)

            For Each factor In colorMap
                alphaColors(factor.Key) = factor _
                    .Value _
                    .TranslateColor _
                    .Alpha(alpha * 255) _
                    .ARGBExpression
            Next

            Return Function(keyObj) alphaColors.TryGetValue(any.ToString(keyObj), [default]:="black")
        End Function

        Public Overrides Function TryGetFactorLegends(factors As Array, shape As LegendStyles, theme As Theme) As LegendObject()
            Return GetLegends(shape, theme.legendLabelCSS).ToArray
        End Function
    End Class
End Namespace
