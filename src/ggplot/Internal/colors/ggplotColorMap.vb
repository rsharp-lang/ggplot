#Region "Microsoft.VisualBasic::363b633989d1db011338c524b91fc6e0, ggplot\src\ggplot\Internal\colors\ggplotColorMap.vb"

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

    '   Total Lines: 75
    '    Code Lines: 52
    ' Comment Lines: 11
    '   Blank Lines: 12
    '     File Size: 2.91 KB


    '     Class ggplotColorMap
    ' 
    '         Properties: alpha, colorMap
    ' 
    '         Function: CreateColorMap, directMap, stringMap
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime

Namespace colors

    Public MustInherit Class ggplotColorMap

        Public Property colorMap As Object
        Public Property alpha As Double

        Public MustOverride Function ColorHandler(ggplot As ggplot, factors As Array) As Func(Of Object, String)
        Public MustOverride Function TryGetFactorLegends(factors As Array, shape As LegendStyles, theme As Theme) As LegendObject()

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="map"></param>
        ''' <param name="alpha">
        ''' color alpha channel value between [0,1]
        ''' </param>
        ''' <param name="env"></param>
        ''' <returns>
        ''' returns nothing if the given color <paramref name="map"/> data is nothing
        ''' </returns>
        Public Shared Function CreateColorMap(map As Object, alpha As Double, env As Environment) As ggplotColorMap
            If TypeOf map Is vector Then
                map = DirectCast(map, vector).data
            End If

            If map Is Nothing Then
                Return Nothing
            ElseIf TypeOf map Is String Then
                Return stringMap(DirectCast(map, String), alpha)
            ElseIf map.GetType.IsArray Then
                Dim strArray As String() = REnv.asVector(Of String)(map)

                If strArray.Length = 1 Then
                    Return stringMap(strArray(Scan0), alpha)
                Else
                    Return directMap(strArray, alpha)
                End If
            ElseIf TypeOf map Is list Then
                Return New ggplotColorFactorMap With {
                    .colorMap = DirectCast(map, list).AsGeneric(Of String)(env),
                    .alpha = alpha
                }
            Else
                Throw New NotImplementedException(map.GetType.FullName)
            End If
        End Function

        Private Shared Function directMap(maps As String(), alpha As Double) As ggplotColorMap
            Throw New NotImplementedException
        End Function

        Private Shared Function stringMap(map As String, alpha As Double) As ggplotColorMap
            Dim isColor As Boolean = False

            Call map.TranslateColor(throwEx:=False, success:=isColor)

            If isColor Then
                Return New ggplotColorLiteral With {.colorMap = map, .alpha = alpha}
            ElseIf TypeOf map Is String Then
                Return New ggplotColorPalette With {.colorMap = map, .alpha = alpha}
            Else
                Return New ggplotColorFactorMap With {.colorMap = map, .alpha = alpha}
            End If
        End Function
    End Class

End Namespace
