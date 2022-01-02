#Region "Microsoft.VisualBasic::8b20b00426077e148b4fadcb667aad36, src\Internal\elements\legend\ggplotLegendElement.vb"

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

'     Class ggplotLegendElement
' 
'         Properties: layout, legend, shapeSize
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
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Namespace elements.legend

    Public Class ggplotLegendElement : Inherits ggplotElement
        Implements IggplotLegendElement

        Public Property legend As LegendObject
        Public Property shapeSize As New Size(120, 45)
        Public Overrides Property layout As Layout Implements IggplotLegendElement.layout

        Public Sub Draw(g As IGraphics,
                        canvas As GraphicsRegion,
                        x As Double,
                        y As Double,
                        theme As Theme) Implements IggplotLegendElement.Draw

            Call g.DrawLegends(
                topLeft:=New PointF(x, y),
                legends:={legend},
                gSize:=$"{shapeSize.Width},{shapeSize.Height}",
                fillBg:=theme.legendBoxBackground,
                titleBrush:=theme.mainTextColor.GetBrush
            )
        End Sub

        Public Function MeasureSize(g As IGraphics) As SizeF Implements IggplotLegendElement.MeasureSize
            Dim maxSizeLabel As String = legend.title
            Dim maxSize As SizeF = g.MeasureString(maxSizeLabel, legend.GetFont(g.Dpi))

            maxSize = New SizeF(shapeSize.Width + maxSize.Width, maxSize.Height)
            maxSize = New SizeF(maxSize.Width, maxSize.Height)

            Return maxSize
        End Function
    End Class
End Namespace
