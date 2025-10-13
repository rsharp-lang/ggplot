#Region "Microsoft.VisualBasic::26437d02bd67083568f5b753c6a20d79, src\ggplot\Internal\layers\scatter\ggplotSegments.vb"

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

    '   Total Lines: 34
    '    Code Lines: 28 (82.35%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 6 (17.65%)
    '     File Size: 1.12 KB


    '     Class ggplotSegments
    ' 
    '         Function: Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace layers

    Public Class ggplotSegments : Inherits ggplotScatterLayer

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim xend As Single(), yend As Single()
            Dim x As Single() = CLRVector.asFloat(stream.x)
            Dim y As Single() = CLRVector.asFloat(stream.y)

            If useCustomData Then
                xend = data.x.ToFloat
                yend = data.y.ToFloat
            Else
                xend = CLRVector.asFloat(stream.x)
                yend = CLRVector.asFloat(stream.y)
            End If

            For i As Integer = 0 To x.Length - 1
                Call RenderShape.RenderBoid(stream.g, x(i), y(i), xend(i), yend(i), Color.Black, maxL:=minCell)
            Next

            If showLegend Then
                Return Nothing
            Else
                Return Nothing
            End If
        End Function
    End Class
End Namespace
