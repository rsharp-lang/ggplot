
Imports ggplot.options
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Imaging.Drawing3D
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime.Interop
Imports REnv = SMRUCC.Rsharp.Runtime

''' <summary>
''' ggplot for 3D
''' </summary>
<Package("ggplot3")>
Module ggplot3

    ''' <summary>
    ''' Create view camera for 3D plot
    ''' </summary>
    ''' <returns></returns>
    <ExportAPI("view_camera")>
    Public Function view_camera(Optional view_distance As Integer = -75,
                                Optional fov As Integer = 100000,
                                <RRawVectorArgument(GetType(Double))>
                                Optional angle As Object = "31.5,65,125") As ggplotCamera

        Dim angles As Double() = REnv.asVector(Of Double)(angle)

        If angles.IsNullOrEmpty Then
            angles = {0, 0, 0}
        ElseIf angles.Length = 1 Then
            angles = {angles(0), angles(0), angles(0)}
        End If

        Dim camera As New Camera With {
            .angleX = angles(0),
            .angleY = angles(1),
            .angleZ = angles(2),
            .fov = fov,
            .viewDistance = view_distance
        }

        Return New ggplotCamera With {
            .camera = camera
        }
    End Function

End Module
