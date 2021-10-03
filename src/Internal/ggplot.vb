Imports System.Drawing.Imaging
Imports System.IO
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports SMRUCC.Rsharp.Runtime.Internal.Object

Public Class ggplot : Implements SaveGdiBitmap

    Public Property data As Object
    Public Property layers As New List(Of ggplotLayer)
    Public Property args As list

    Public Function Save(stream As Stream, format As ImageFormat) As Boolean Implements SaveGdiBitmap.Save
        Throw New NotImplementedException()
    End Function
End Class
