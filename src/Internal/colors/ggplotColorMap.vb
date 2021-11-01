#Region "Microsoft.VisualBasic::eaf449a477467fc640da90c87b1d6690, src\Internal\ggplotColorMap.vb"

' Author:
' 
'       asuka (amethyst.asuka@gcmodeller.org)
'       xie (genetics@smrucc.org)
'       xieguigang (xie.guigang@live.com)
' 
' Copyright (c) 2018 GPL3 Licensed
' 
' 
' GNU GENERAL PUBLIC LICENSE (GPL3)
' 
' 
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
' 
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
' 
' You should have received a copy of the GNU General Public License
' along with this program. If not, see <http://www.gnu.org/licenses/>.



' /********************************************************************************/

' Summaries:

' Class ggplotColorMap
' 
'     Properties: colorMap
' 
'     Function: CreateColorMap, directMap, stringMap
' 
' Class ggplotColorLiteral
' 
'     Function: ColorHandler, ToColor, TryGetFactorLegends
' 
' Class ggplotColorPalette
' 
'     Function: ColorHandler, TryGetFactorLegends
' 
' Class ggplotColorFactorMap
' 
'     Function: ColorHandler, GetLegends, ToString, TryGetFactorLegends
' 
' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports REnv = SMRUCC.Rsharp.Runtime

Namespace colors

    Public MustInherit Class ggplotColorMap

        Public Property colorMap As Object

        Public MustOverride Function ColorHandler(ggplot As ggplot, factors As Array) As Func(Of Object, String)
        Public MustOverride Function TryGetFactorLegends(factors As Array, shape As LegendStyles, theme As Theme) As LegendObject()

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

            Call map.TranslateColor(throwEx:=False, success:=isColor)

            If isColor Then
                Return New ggplotColorLiteral With {.colorMap = map}
            ElseIf TypeOf map Is String Then
                Return New ggplotColorPalette With {.colorMap = map}
            Else
                Return New ggplotColorFactorMap With {.colorMap = map}
            End If
        End Function
    End Class

End Namespace