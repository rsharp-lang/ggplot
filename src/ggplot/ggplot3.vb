﻿#Region "Microsoft.VisualBasic::f1e0e79c3b371a3fa3020db7ab6b7378, src\ggplot\ggplot3.vb"

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

    '   Total Lines: 50
    '    Code Lines: 33 (66.00%)
    ' Comment Lines: 10 (20.00%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 7 (14.00%)
    '     File Size: 1.59 KB


    ' Module ggplot3
    ' 
    '     Function: view_camera
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot.options
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Imaging.Drawing3D
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports REnv = SMRUCC.Rsharp.Runtime

''' <summary>
''' ggplot for 3D
''' </summary>
<Package("ggplot3")>
Module ggplot3

    ''' <summary>
    ''' Create view camera for 3D plot
    ''' </summary>
    ''' <param name="angle">
    ''' should be a numeric vector with 3 elements, the camera view angle to the canvas
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("view_camera")>
    <RApiReturn(GetType(ggplotCamera))>
    Public Function view_camera(Optional view_distance As Integer = -75,
                                Optional fov As Integer = 100000,
                                <RRawVectorArgument(GetType(Double))>
                                Optional angle As Object = "31.5,65,125") As ggplotCamera

        Dim angles As Double() = CLRVector.asNumeric(angle)

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
