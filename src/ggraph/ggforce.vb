
Imports ggplot.ggraph
Imports ggplot.ggraph.layout
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Interop
Imports REnv = SMRUCC.Rsharp.Runtime

<Package("ggforce")>
Module ggforcePkg

    <ExportAPI("layout_random")>
    Public Function layout_random() As ggforce
        Return New layout.random
    End Function

    <ExportAPI("layout_springforce")>
    Public Function spring_force(Optional stiffness# = 80,
                                 Optional repulsion# = 4000,
                                 Optional damping# = 0.83,
                                 Optional iterations% = 1000) As spring_force

        Return New spring_force With {
            .damping = damping,
            .iterations = iterations,
            .repulsion = repulsion,
            .stiffness = stiffness
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
                                         <RRawVectorArgument(GetType(String))>
                                         Optional algorithm As Object = "force_directed|degree_weighted|group_weighted|edge_weighted",
                                         Optional env As Environment = Nothing) As force_directed

        algorithm = DirectCast(REnv.asVector(Of String)(algorithm), String())(Scan0)

        Return New force_directed With {
            .condenseFactor = condenseFactor,
            .dist_threshold = InteropArgumentHelper.getSize(dist_threshold, env, "35,250"),
            .ejectFactor = ejectFactor,
            .maxtx = maxtx,
            .maxty = maxty,
            .size = InteropArgumentHelper.getSize(size, env, "1000,1000"),
            .iterations = iterations,
            .algorithm = algorithm
        }
    End Function
End Module
