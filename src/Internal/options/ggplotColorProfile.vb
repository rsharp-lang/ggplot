#Region "Microsoft.VisualBasic::520e15b2604ef5d41e1ad904675f43a2, src\Internal\options\ggplotColorProfile.vb"

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

    '     Class ggplotColorProfile
    ' 
    '         Properties: profile
    ' 
    '         Function: Config
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace options

    Public Class ggplotColorProfile : Inherits ggplotOption

        Public Property profile As ggplotColorMap

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            ggplot.layers.Last.colorMap = profile
            Return ggplot
        End Function
    End Class
End Namespace
