Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime
Imports any = Microsoft.VisualBasic.Scripting

Public MustInherit Class ggplotColorMap

    Public Property colorMap As Object

    Public MustOverride Function ColorHandler(ggplot As ggplot) As Func(Of Object, String)

    Public Shared Function CreateColorMap(map As Object, env As Environment) As ggplotColorMap
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
            Return New ggplotColorFactorMap With {
                .colorMap = DirectCast(map, list).AsGeneric(Of String)(env)
            }
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
            Return New ggplotColorLiteral With {.colorMap = map}
        ElseIf isDesigner Then
            Return New ggplotColorPalette With {.colorMap = map}
        Else
            Return New ggplotColorFactorMap With {.colorMap = map}
        End If
    End Function
End Class

Public Class ggplotColorLiteral : Inherits ggplotColorMap

    Public Function ToColor() As Color
        Return DirectCast(colorMap, String).TranslateColor
    End Function

    Public Overrides Function ColorHandler(ggplot As ggplot) As Func(Of Object, String)
        Throw New NotImplementedException()
    End Function
End Class

Public Class ggplotColorPalette : Inherits ggplotColorMap

    Public Overrides Function ColorHandler(ggplot As ggplot) As Func(Of Object, String)
        Throw New NotImplementedException()
    End Function
End Class

''' <summary>
''' used the field value as color factor
''' </summary>
Public Class ggplotColorFactorMap : Inherits ggplotColorMap

    Public Overrides Function ToString() As String
        Return DirectCast(colorMap, Dictionary(Of String, String)).GetJson
    End Function

    Public Overrides Function ColorHandler(ggplot As ggplot) As Func(Of Object, String)
        Dim colorMap As Dictionary(Of String, String) = Me.colorMap
        Return Function(keyObj) colorMap.TryGetValue(any.ToString(keyObj), [default]:="black")
    End Function
End Class