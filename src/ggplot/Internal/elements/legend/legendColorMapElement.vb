#Region "Microsoft.VisualBasic::40fa9bfc638d8e683ca3b2ce8f4945a7, src\Internal\elements\legend\legendColorMapElement.vb"

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

'     Class legendColorMapElement
' 
'         Properties: colorMapLegend, height, layout, width
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
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors

Namespace elements.legend

    Public Class legendColorMapElement : Inherits ggplotElement
        Implements IggplotLegendElement

        Public Overrides Property layout As Layout Implements IggplotLegendElement.layout
        Public Property colorMapLegend As ColorMapLegend
        Public Property width As Single
        Public Property height As Single

        Public ReadOnly Property size As Integer Implements IggplotLegendElement.size
            Get
                If colorMapLegend Is Nothing Then
                    Return 0
                Else
                    Return 1
                End If
            End Get
        End Property

        Public Sub Draw(g As IGraphics, canvas As GraphicsRegion, x As Double, y As Double, theme As Theme) Implements IggplotLegendElement.Draw
            Call colorMapLegend.Draw(g, New Rectangle(x, y, width, height))
        End Sub

        Public Function MeasureSize(g As IGraphics) As SizeF Implements IggplotLegendElement.MeasureSize
            Dim titleSize = g.MeasureString(colorMapLegend.title, colorMapLegend.titleFont)
            Dim maxWidth As Single = {titleSize.Width, width}.Max

            Return New SizeF(maxWidth, height)
        End Function
    End Class
End Namespace
