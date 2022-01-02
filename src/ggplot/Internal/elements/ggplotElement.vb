#Region "Microsoft.VisualBasic::4c06e7503018487b1c0d1298d07e0a49, src\Internal\elements\ggplotElement.vb"

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

    '     Class ggplotElement
    ' 
    '         Properties: layout
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas

Namespace elements

    ''' <summary>
    ''' ### Theme elements
    ''' 
    ''' In conjunction with the theme system, the ``element_`` 
    ''' functions specify the display of how non-data 
    ''' components of the plot are drawn.
    ''' </summary>
    Public Class ggplotElement

        Public Overridable Property layout As Layout

    End Class
End Namespace
