#Region "Microsoft.VisualBasic::cfb3585fe0b35bd096fc88b1c700dd64, ggplot\src\ggplot\Internal\layers\scatter\ggplotScatterLayer.vb"

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

    '   Total Lines: 32
    '    Code Lines: 21
    ' Comment Lines: 5
    '   Blank Lines: 6
    '     File Size: 1.05 KB


    '     Class ggplotScatterLayer
    ' 
    '         Properties: minCell
    ' 
    '         Function: getCellsize, getMeanCell
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Math
Imports stdNum = System.Math

Namespace layers

    Public MustInherit Class ggplotScatterLayer : Inherits ggplotLayer

        ''' <summary>
        ''' the min cell width/height
        ''' </summary>
        ''' <returns></returns>
        Public Property minCell As Integer = 16

        Protected Function getMeanCell(data As Double(), top_n As Integer) As Double
            Return NumberGroups.diff(data.OrderBy(Function(xi) xi).ToArray) _
                .OrderByDescending(Function(a) a) _
                .Take(top_n) _
                .Average
        End Function

        Protected Function getCellsize(x As Double(), y As Double()) As SizeF
            ' evaluate cells grid
            Dim topN As Integer = stdNum.Min(x.Length / 5, 100)
            Dim cellWidth = getMeanCell(x, topN)
            Dim cellHeight = getMeanCell(y, topN)

            Return New SizeF(cellWidth, cellHeight)
        End Function
    End Class
End Namespace
