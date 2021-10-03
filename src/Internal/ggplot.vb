Imports System.Drawing
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop.CType

Public Class ggplot : Inherits IPlot

    Public Property data As Object
    Public Property layers As New Queue(Of ggplotLayer)
    Public Property base As ggplotLayer
    Public Property args As list

    Protected Overrides Function CreatePlot() As Image
        Throw New NotImplementedException()
    End Function
End Class
