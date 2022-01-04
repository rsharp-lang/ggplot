Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Diagnostics
Imports Microsoft.VisualBasic.Imaging.Driver
Imports SMRUCC.Rsharp.Runtime

Module driver

    <Extension>
    Public Function getDriver(env As Environment) As Drivers
        For Each stack As StackFrame In env.stackTrace
            If stack.Method.Namespace = "grDevices" AndAlso stack.Method.Method = "svg" Then
                Return Drivers.SVG
            End If
        Next

        Return Drivers.GDI
    End Function
End Module
