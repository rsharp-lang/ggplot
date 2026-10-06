#Region "Microsoft.VisualBasic::8ea3370848885888fe208791826e236e, src\ggraph\ggforce.vb"

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

    ''' <summary>
    ''' layout_random(): a reproducible random node layout
    ''' </summary>
    ''' <param name="seed">the random seed, zero means a random seed</param>
    ''' <returns></returns>
    <ExportAPI("layout_random")>
    <RApiReturn(GetType(random))>
    Public Function layout_random(Optional seed As Integer = 0) As layout.random
        Return New layout.random With {.seed = seed}
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

    ''' <summary>
    ''' layout_circular(): places the nodes on a circle
    ''' </summary>
    ''' <param name="radius">the radius of the circle</param>
    ''' <param name="sortByDegree">sort the nodes by their degree</param>
    ''' <param name="crossingOptimization">minimize the number of the edge crossings</param>
    ''' <param name="maxSwaps">the maximum number of the swap attempts</param>
    ''' <returns></returns>
    <ExportAPI("layout_circular")>
    Public Function layout_circular(Optional radius As Double = Double.NaN,
                                    Optional sortByDegree As Boolean = True,
                                    Optional crossingOptimization As Boolean = False,
                                    Optional maxSwaps As Integer = 1000) As circular_layout

        Return New circular_layout With {
            .radius = radius,
            .sortByDegree = sortByDegree,
            .crossingOptimization = crossingOptimization,
            .maxSwaps = maxSwaps
        }
    End Function

    ''' <summary>
    ''' layout_radial(): places the nodes on concentric circles
    ''' </summary>
    ''' <param name="radius">the radius of the outermost circle</param>
    ''' <returns></returns>
    <ExportAPI("layout_radial")>
    Public Function layout_radial(Optional radius As Double = Double.NaN) As radial_layout
        Return New radial_layout With {.radius = radius}
    End Function

    ''' <summary>
    ''' layout_cola(): the constraint based stress majorization layout
    ''' </summary>
    ''' <param name="iterations">the number of the layout iterations</param>
    ''' <param name="avoidOverlaps">run the overlap removal after the layout</param>
    ''' <param name="handleDisconnected">pull the disconnected components apart</param>
    ''' <param name="flowLayout">run a layered flow layout</param>
    ''' <param name="flowAxis">the axis of the flow layout</param>
    ''' <param name="centerGraph">center the graph after the layout</param>
    ''' <returns></returns>
    <ExportAPI("layout_cola")>
    Public Function layout_cola(Optional iterations As Integer = 200,
                               Optional avoidOverlaps As Boolean = False,
                               Optional handleDisconnected As Boolean = True,
                               Optional flowLayout As Boolean = False,
                               Optional flowAxis As String = "y",
                               Optional centerGraph As Boolean = True) As cola_layout

        Return New cola_layout With {
            .iterations = iterations,
            .avoidOverlaps = avoidOverlaps,
            .handleDisconnected = handleDisconnected,
            .flowLayout = flowLayout,
            .flowAxis = flowAxis,
            .centerGraph = centerGraph
        }
    End Function

    ''' <summary>
    ''' layout_cola3d(): the three dimensional constraint layout
    ''' </summary>
    ''' <param name="iterations">the number of the layout iterations</param>
    ''' <param name="idealLinkLength">the preferred length of an edge</param>
    ''' <returns></returns>
    <ExportAPI("layout_cola3d")>
    Public Function layout_cola3d(Optional iterations As Integer = 100,
                                  Optional idealLinkLength As Double = 1) As cola3d_layout
        Return New cola3d_layout With {
            .iterations = iterations,
            .idealLinkLength = idealLinkLength
        }
    End Function

    ''' <summary>
    ''' layout_hola(): the incremental HOLA layout
    ''' </summary>
    ''' <param name="nodeGap">the minimum distance between two nodes</param>
    ''' <param name="desiredEdgeLength">the preferred length of an edge</param>
    ''' <param name="alignEpsilon">the threshold of the alignment constraint</param>
    ''' <param name="convergeEpsilon">the convergence threshold</param>
    ''' <param name="maxIterations">the maximum number of the relaxation iterations</param>
    ''' <returns></returns>
    <ExportAPI("layout_hola")>
    Public Function layout_hola(Optional nodeGap As Double = 30,
                                Optional desiredEdgeLength As Double = 60,
                                Optional alignEpsilon As Double = 4,
                                Optional convergeEpsilon As Double = 0.01,
                                Optional maxIterations As Integer = 200) As hola_layout

        Return New hola_layout With {
            .nodeGap = nodeGap,
            .desiredEdgeLength = desiredEdgeLength,
            .alignEpsilon = alignEpsilon,
            .convergeEpsilon = convergeEpsilon,
            .maxIterations = maxIterations
        }
    End Function

    ''' <summary>
    ''' layout_orthogonal(): routes the edges as orthogonal polylines
    ''' </summary>
    ''' <param name="simplify">simplify the routing result</param>
    ''' <param name="fixNonOrthogonal">fix the non orthogonal segments</param>
    ''' <returns></returns>
    <ExportAPI("layout_orthogonal")>
    Public Function layout_orthogonal(Optional simplify As Boolean = True,
                                      Optional fixNonOrthogonal As Boolean = True) As orthogonal_layout
        Return New orthogonal_layout With {
            .simplify = simplify,
            .fixNonOrthogonal = fixNonOrthogonal
        }
    End Function

    ''' <summary>
    ''' layout_force3d(): the three dimensional spring force layout
    ''' </summary>
    ''' <param name="stiffness"></param>
    ''' <param name="repulsion"></param>
    ''' <param name="damping"></param>
    ''' <param name="iterations"></param>
    ''' <param name="time_step"></param>
    ''' <returns></returns>
    <ExportAPI("layout_force3d")>
    Public Function layout_force3d(Optional stiffness As Double = 50000,
                                   Optional repulsion As Double = 100,
                                   Optional damping As Double = 0.9,
                                   Optional iterations As Integer = 1000,
                                   Optional time_step As Double = 0.0001) As force3d

        Return New force3d With {
            .stiffness = stiffness,
            .repulsion = repulsion,
            .damping = damping,
            .iterations = iterations,
            .[step] = time_step
        }
    End Function

    ''' <summary>
    ''' layout_mingle(): the MINGLE edge bundling
    ''' </summary>
    ''' <param name="k">the number of the nearest neighbours of the bundling</param>
    ''' <param name="rounds">the number of the bundling iterations</param>
    ''' <returns></returns>
    <ExportAPI("layout_mingle")>
    Public Function layout_mingle(Optional k As Integer = 10,
                                  Optional rounds As Integer = 1) As mingle
        Return New mingle With {
            .k = k,
            .rounds = rounds
        }
    End Function
End Module
