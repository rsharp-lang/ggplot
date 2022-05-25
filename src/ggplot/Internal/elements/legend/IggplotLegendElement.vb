#Region "Microsoft.VisualBasic::7a5841abba05ae940e497899198ce739, src\Internal\elements\legend\IggplotLegendElement.vb"

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

    '     Interface IggplotLegendElement
    ' 
    '         Properties: layout
    ' 
    '         Function: MeasureSize
    ' 
    '         Sub: Draw
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Namespace elements.legend

    Public Interface IggplotLegendElement

        Property layout As Layout

        ''' <summary>
        ''' the number of the legend object that 
        ''' contains in current legend group 
        ''' element object.
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property size As Integer

        Sub Draw(g As IGraphics, canvas As GraphicsRegion, x As Double, y As Double, theme As Theme)
        Function MeasureSize(g As IGraphics) As SizeF

    End Interface
End Namespace
