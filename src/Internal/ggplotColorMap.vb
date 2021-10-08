Imports Microsoft.VisualBasic.Imaging
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime

Public MustInherit Class ggplotColorMap

    Public Property colorMap As Object

    Public MustOverride Function ColorHandler(ggplot As ggplot) As Func(Of Integer, String)

    Public Shared Function CreateColorMap(map As Object) As ggplotColorMap
        If TypeOf map Is String Then
            Return stringMap(DirectCast(map, String))
        ElseIf map.GetType.IsArray Then
            Dim strArray As String() = REnv.asVector(Of String)(map)

            If strArray.Length = 1 Then
                Return stringMap(strArray(Scan0))
            Else
                Return directMap(strArray)
            End If
        ElseIf TypeOf map Is list Then

        Else
            Throw New NotImplementedException(map.GetType.FullName)
        End If
    End Function

    Private Shared Function directMap(maps As String()) As ggplotColorMap
        Throw New NotImplementedException
    End Function

    Private Shared Function stringMap(map As String) As ggplotColorMap
        Dim isColor As Boolean = False
        Dim isDesigner As Boolean = False

        Call map.TranslateColor(throwEx:=False, success:=isColor)

        If isColor Then
            Return New ggplotUnifyColor With {.colorMap = map}
        ElseIf isDesigner Then
            Return New ggplotColorPalette With {.colorMap = map}
        Else
            Return New ggplotColorFactorMap With {.colorMap = map}
        End If
    End Function
End Class

Public Class ggplotUnifyColor : Inherits ggplotColorMap

    Public Overrides Function ColorHandler(ggplot As ggplot) As Func(Of Integer, String)
        Throw New NotImplementedException()
    End Function
End Class

Public Class ggplotColorPalette : Inherits ggplotColorMap

    Public Overrides Function ColorHandler(ggplot As ggplot) As Func(Of Integer, String)
        Throw New NotImplementedException()
    End Function
End Class

''' <summary>
''' used the field value as color factor
''' </summary>
Public Class ggplotColorFactorMap : Inherits ggplotColorMap

    Public Overrides Function ColorHandler(ggplot As ggplot) As Func(Of Integer, String)
        Throw New NotImplementedException()
    End Function
End Class