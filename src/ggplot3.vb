
Imports ggplot.options
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Imaging.Drawing3D
Imports Microsoft.VisualBasic.Scripting.MetaData

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
                                Optional fov As Integer = 10000,
                                Optional angle As Double() = Nothing) As ggplotCamera

        If angle.IsNullOrEmpty Then
            angle = {0, 0, 0}
        ElseIf angle.Length = 1 Then
            angle = {angle(0), angle(0), angle(0)}
        End If

        Dim camera As New Camera With {
            .angleX = angle(0),
            .angleY = angle(1),
            .angleZ = angle(2),
            .fov = fov,
            .viewDistance = view_distance
        }

        Return New ggplotCamera With {
            .camera = camera
        }
    End Function

End Module
