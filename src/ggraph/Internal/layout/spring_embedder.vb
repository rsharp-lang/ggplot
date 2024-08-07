﻿#Region "Microsoft.VisualBasic::40e6083b29e91cf621610c80aba5e7ec, src\ggraph\Internal\layout\spring_embedder.vb"

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

    '   Total Lines: 23
    '    Code Lines: 13 (56.52%)
    ' Comment Lines: 6 (26.09%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 4 (17.39%)
    '     File Size: 790 B


    '     Class spring_embedder
    ' 
    '         Properties: c, canvasSize, maxRepulsiveForceDistance
    ' 
    '         Function: createAlgorithm
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts

Namespace ggraph.layout

    ''' <summary>
    ''' Spring embedder layout algorithm
    ''' </summary>
    ''' <remarks>
    ''' this module works based on the <see cref="SpringEmbedder"/>
    ''' </remarks>
    Public Class spring_embedder : Inherits ggforce

        Public Property canvasSize As Size
        Public Property maxRepulsiveForceDistance As Double = 10
        Public Property c As Double

        Protected Overrides Function createAlgorithm(g As NetworkGraph) As IPlanner
            Return New SpringEmbedder(g, canvasSize, maxRepulsiveForceDistance, c:=c)
        End Function
    End Class
End Namespace
