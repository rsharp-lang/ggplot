#Region "Microsoft.VisualBasic::7e389e79f25a29fb72fdb3ded1ce2bc2, src\ggplot\Internal\ggplotBase.vb"

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

    '   Total Lines: 52
    '    Code Lines: 30 (57.69%)
    ' Comment Lines: 13 (25.00%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 9 (17.31%)
    '     File Size: 1.89 KB


    ' Class ggplotBase
    ' 
    '     Properties: data, reader
    ' 
    '     Function: checkMultipleLegendGroup, getColors, getGgplotData
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports RInternal = SMRUCC.Rsharp.Runtime.Internal

''' <summary>
''' 只绘制出一个基本的坐标轴
''' </summary>
Public Class ggplotBase

    Public Property reader As ggplotReader

    Public Property data As New Dictionary(Of String, Object)

    Public Function getColors(ggplot As ggplot, ByRef legends As IggplotLegendElement, shape As LegendStyles) As String()
        With reader.getMapColor(ggplot.data, shape, ggplot.ggplotTheme, ggplot.environment)
            legends = .legends
            Return .htmlColors
        End With
    End Function

    ''' <summary>
    ''' Check of the given legend obejct is a group of multiple data serials?
    ''' </summary>
    ''' <param name="legend"></param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function checkMultipleLegendGroup(legend As IggplotLegendElement) As Boolean
        Return TypeOf legend Is legendGroupElement AndAlso DirectCast(legend, legendGroupElement).legends.TryCount > 1
    End Function

    ''' <summary>
    ''' create new ggplot data source
    ''' </summary>
    ''' <param name="ggplot"></param>
    ''' <returns></returns>
    Public Function getGgplotData(ggplot As ggplot) As ggplotData
        Dim dataXy = reader.getMapData(ggplot.data, ggplot.environment)

        If dataXy.y Is Nothing OrElse dataXy.y.size = 0 Then
            If data.ContainsKey("y") Then
                dataXy.y = axisMap.Create(data!y)
            Else
                dataXy.error = RInternal.debug.stop("no axis y data mapping!", ggplot.environment, suppress:=True)
            End If
        End If

        Return dataXy
    End Function

End Class
