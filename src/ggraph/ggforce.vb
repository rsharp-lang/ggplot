﻿#Region "Microsoft.VisualBasic::8ea3370848885888fe208791826e236e, src\ggraph\ggforce.vb"

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

    '   Total Lines: 82
    '    Code Lines: 72 (87.80%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 10 (12.20%)
    '     File Size: 3.67 KB


    ' Module ggforcePkg
    ' 
    '     Function: layout_forcedirected, layout_random, layout_springembedder, spring_force
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.ggraph
Imports ggplot.ggraph.layout
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization

<Package("ggforce")>
Module ggforcePkg

    <ExportAPI("layout_springembedder")>
    Public Function layout_springembedder(<RRawVectorArgument>
                                          canvas As Object,
                                          Optional maxRepulsiveForceDistance As Double = 10,
                                          Optional c As Double = 2,
                                          Optional iterations As Integer = 100,
                                          Optional env As Environment = Nothing) As spring_embedder

        Dim sizeDesc As String = InteropArgumentHelper.getSize(canvas, env)
        Dim size As Size = sizeDesc.SizeParser

        Return New spring_embedder With {
            .canvasSize = size,
            .iterations = iterations,
            .c = c,
            .maxRepulsiveForceDistance = maxRepulsiveForceDistance
        }
    End Function

    <ExportAPI("layout_random")>
    <RApiReturn(GetType(random))>
    Public Function layout_random() As ggforce
        Return New layout.random
    End Function

    <ExportAPI("layout_springforce")>
    Public Function spring_force(Optional stiffness# = 50000,
                                 Optional repulsion# = 100,
                                 Optional damping# = 0.9,
                                 Optional iterations% = 1000,
                                 Optional time_step As Double = 0.0001) As spring_force

        Return New spring_force With {
            .damping = damping,
            .iterations = iterations,
            .repulsion = repulsion,
            .stiffness = stiffness,
            .[step] = time_step
        }
    End Function

    <ExportAPI("layout_forcedirected")>
    Public Function layout_forcedirected(Optional ejectFactor As Integer = 6,
                                         Optional condenseFactor As Integer = 3,
                                         Optional maxtx As Integer = 4,
                                         Optional maxty As Integer = 3,
                                         <RRawVectorArgument> Optional dist_threshold As Object = "30,250",
                                         <RRawVectorArgument> Optional size As Object = "1000,1000",
                                         Optional iterations As Integer = 20000,
                                         Optional time_step As Double = 0.00001,
                                         <RRawVectorArgument(GetType(String))>
                                         Optional algorithm As Object = "force_directed|degree_weighted|group_weighted|edge_weighted",
                                         Optional env As Environment = Nothing) As force_directed

        algorithm = CLRVector.asCharacter(algorithm).First

        Return New force_directed With {
            .condenseFactor = condenseFactor,
            .dist_threshold = InteropArgumentHelper.getSize(dist_threshold, env, "35,250"),
            .ejectFactor = ejectFactor,
            .maxtx = maxtx,
            .maxty = maxty,
            .size = InteropArgumentHelper.getSize(size, env, "1000,1000"),
            .iterations = iterations,
            .algorithm = algorithm,
            .[step] = time_step
        }
    End Function
End Module
