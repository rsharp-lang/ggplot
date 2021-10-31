#Region "Microsoft.VisualBasic::87cf2d226c36bdf8224c3ec3e2376f81, src\Internal\options\ggplotTitle.vb"

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

    '     Class ggplotTitle
    ' 
    '         Properties: title
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Config
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace options

    Public Class ggplotTitle : Inherits ggplotOption

        Public Property title As String

        Sub New(title As String)
            Me.title = title
        End Sub

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.main = title
            Return ggplot
        End Function
    End Class
End Namespace
