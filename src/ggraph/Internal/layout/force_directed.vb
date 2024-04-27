#Region "Microsoft.VisualBasic::fe7722c4c18410ca2fe6ce1691007599, G:/GCModeller/src/runtime/ggplot/src/ggraph//Internal/layout/force_directed.vb"

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

    '   Total Lines: 56
    '    Code Lines: 39
    ' Comment Lines: 12
    '   Blank Lines: 5
    '     File Size: 2.06 KB


    '     Class force_directed
    ' 
    '         Properties: algorithm
    ' 
    '         Function: createAlgorithm
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Data.visualize.Network.Layouts

Namespace ggraph.layout

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' based on the <see cref="algorithm"/> parameter, this module run based on the
    ''' <see cref="ForceDirected.Planner"/> for "naive" or <see cref="ForceDirected.GroupPlanner"/>
    ''' for "group_weighted"
    ''' </remarks>
    Public Class force_directed : Inherits ggforce

        Public ejectFactor As Integer = 6,
            condenseFactor As Integer = 3,
            maxtx As Integer = 4,
            maxty As Integer = 3,
            dist_threshold$ = "30,250",
            size$ = "1000,1000"

        ''' <summary>
        ''' force_directed|degree_weighted|group_weighted|edge_weighted
        ''' </summary>
        ''' <returns></returns>
        Public Property algorithm As String = "naive"

        Protected Overrides Function createAlgorithm(g As NetworkGraph) As IPlanner
            If algorithm = "naive" OrElse algorithm = "force_directed" Then
                Return New ForceDirected.Planner(
                    g:=g,
                    ejectFactor:=ejectFactor,
                    condenseFactor:=condenseFactor,
                    maxtx:=maxtx,
                    maxty:=maxty,
                    dist_threshold:=dist_threshold,
                    size:=size
                )
            ElseIf algorithm = "group_weighted" Then
                Return New ForceDirected.GroupPlanner(
                    g:=g,
                    ejectFactor:=ejectFactor,
                    condenseFactor:=condenseFactor,
                    maxtx:=maxtx,
                    maxty:=maxty,
                    dist_threshold:=dist_threshold,
                    size:=size,
                    groupAttraction:=10
                )
            Else
                Throw New NotImplementedException
            End If
        End Function
    End Class
End Namespace
