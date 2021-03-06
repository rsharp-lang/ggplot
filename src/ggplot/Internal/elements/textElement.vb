#Region "Microsoft.VisualBasic::de189885b59525f6040608c3fd443dc5, src\Internal\elements\textElement.vb"

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

'     Class textElement
' 
'         Properties: style
' 
' 
' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.MIME.Html.CSS

Namespace elements

    Public Class textElement : Inherits ggplotElement

        Public Property style As CSSFont
        Public Property color As String
        Public Property angle As Single

        Public Function GetCSS() As String
            Return style.ToString
        End Function

        Public Function ConfigCSS(css As CSSFont) As CSSFont
            If style.size > 0 Then
                css.size = style.size
            End If
            If Not style.family.StringEmpty Then
                css.family = style.family
            End If

            Return css
        End Function
    End Class
End Namespace
