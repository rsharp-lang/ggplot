#Region "Microsoft.VisualBasic::4d134998e04d84aa89c1fbcdda16fd74, G:/GCModeller/src/runtime/ggplot/src/ggraph//Internal/layout/ggforce.vb"

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

    '   Total Lines: 44
    '    Code Lines: 32
    ' Comment Lines: 0
    '   Blank Lines: 12
    '     File Size: 1.54 KB


    '     Class ggforce
    ' 
    '         Properties: [step], iterations
    ' 
    '         Function: Config
    ' 
    '         Sub: createLayout
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot.options
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts
Imports Microsoft.VisualBasic.Emit.Delegates
Imports SMRUCC.Rsharp.Runtime

Namespace ggraph.layout

    Public MustInherit Class ggforce : Inherits ggplotOption

        Public Property iterations As Integer = 10000 * 2
        Public Property [step] As Double = 0.001

        Protected MustOverride Function createAlgorithm(g As NetworkGraph) As IPlanner

        Public Sub createLayout(ByRef g As NetworkGraph, env As Environment)
            Dim algorithm As IPlanner = createAlgorithm(g.doRandomLayout)
            Dim println = env.WriteLineHandler
            Dim delta As Integer = iterations / 10

            Call println("start create layout...")

            For i As Integer = 0 To iterations
                Call algorithm.Collide(timeStep:=[step])

                If i Mod delta = 0 Then
                    Call println($"[{(i / iterations * 100).ToString("F0")}%] ... {i}/{iterations}")
                End If
            Next

            If algorithm.GetType.ImplementInterface(Of IDisposable) Then
                Call DirectCast(algorithm, IDisposable).Dispose()
            End If

            Call println(" ~done!")
        End Sub

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.args.slots(NameOf(ggforce)) = Me
            Return ggplot
        End Function

    End Class
End Namespace
