#Region "Microsoft.VisualBasic::9b8dcb6393996f316330d5ea3251ad28, src\ggplot\Internal\ggplotBase.vb"

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

    '   Total Lines: 36
    '    Code Lines: 25 (69.44%)
    ' Comment Lines: 3 (8.33%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 8 (22.22%)
    '     File Size: 1.20 KB


    ' Class ggplotBase
    ' 
    '     Properties: data, reader
    ' 
    '     Function: getColors, getGgplotData
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot.elements
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports SMRUCC.Rsharp.Runtime

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

    Public Function getGgplotData(ggplot As ggplot) As ggplotData
        Dim dataXy = reader.getMapData(ggplot.data, ggplot.environment)

        If dataXy.y Is Nothing OrElse dataXy.y.size = 0 Then
            If data.ContainsKey("y") Then
                dataXy.y = axisMap.Create(data!y)
            Else
                dataXy.error = Internal.debug.stop("no axis y data mapping!", ggplot.environment, suppress:=True)
            End If
        End If

        Return dataXy
    End Function

End Class
