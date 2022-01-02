#Region "Microsoft.VisualBasic::168bbf60b3c194f6accd8225092547f0, src\Internal\ggplotBase.vb"

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

' Class ggplotBase
' 
'     Properties: reader
' 
'     Function: getColors
' 
' /********************************************************************************/

#End Region

Imports SMRUCC.Rsharp.Runtime

''' <summary>
''' 只绘制出一个基本的坐标轴
''' </summary>
Public Class ggplotBase

    Public Property reader As ggplotReader

    Public Property data As New Dictionary(Of String, Object)

    Public Function getColors(ggplot As ggplot) As String()
        Return reader.getMapColor(ggplot.data, ggplot.environment)
    End Function

    Public Function getGgplotData(ggplot As ggplot) As ggplotData
        Dim dataXy = reader.getMapData(ggplot.data, ggplot.environment)

        If dataXy.y Is Nothing OrElse dataXy.y.Length = 0 Then
            If data.ContainsKey("y") Then
                dataXy.y = data!y
            Else
                dataXy.error = Internal.debug.stop("no axis y data mapping!", ggplot.environment)
            End If
        End If

        Return dataXy
    End Function

End Class
