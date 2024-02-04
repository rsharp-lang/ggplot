#Region "Microsoft.VisualBasic::90c90d4a823208ba598e2ef6dac2ccd1, ggplot\src\ggraph\Internal\layout\spring_force.vb"

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

    '   Total Lines: 17
    '    Code Lines: 13
    ' Comment Lines: 0
    '   Blank Lines: 4
    '     File Size: 640 B


    '     Class spring_force
    ' 
    '         Properties: damping, repulsion, stiffness
    ' 
    '         Function: createAlgorithm
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts.SpringForce

Namespace ggraph.layout

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' this module works based on the <see cref="ForceDirected2D"/> algorithm module
    ''' </remarks>
    Public Class spring_force : Inherits ggforce

        Public Property stiffness As Double = 80
        Public Property repulsion As Double = 4000
        Public Property damping As Double = 0.83

        Protected Overrides Function createAlgorithm(g As NetworkGraph) As IPlanner
            Return New ForceDirected2D(g, stiffness, repulsion, damping)
        End Function
    End Class
End Namespace
