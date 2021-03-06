#Region "Microsoft.VisualBasic::4bf33348446e9b093784431cbdb68391, src\Internal\layers\ggplotABLine.vb"

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

'     Class ggplotABLine
' 
'         Properties: abline
' 
'         Function: constraint, Plot
' 
' 
' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging.d3js.scale
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Shapes

Namespace layers

    Public Class ggplotABLine : Inherits ggplotLayer

        Public Property abline As Line

        Protected Friend Overrides Function initDataSet(ggplot As ggplot) As ggplotData
            data = New ggplotData With {
                .x = New Double() {abline.A.X, abline.B.X},
                .y = New Double() {abline.A.Y, abline.B.Y}
            }

            Return data
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim scale As DataScaler = stream.scale
            Dim a As PointF = constraint(abline.A, scale)
            Dim b As PointF = constraint(abline.B, scale)

            If scale.xscale <> scalers.ordinal Then
                a = scale.Translate(a)
                b = scale.Translate(b)
            Else
                a = translateOrdinal(a, scale)
                b = translateOrdinal(b, scale)
            End If

            Call stream.g.DrawLine(abline.Stroke, a, b)

            Return Nothing
        End Function

        Private Shared Function translateOrdinal(a As PointF, scale As DataScaler) As PointF
            If a.X < Single.MinValue Then
                a = New PointF With {.X = scale.X.rangeMin, .Y = scale.TranslateY(a.Y)}
            ElseIf a.X > Single.MaxValue Then
                a = New PointF With {.X = scale.X.rangeMax, .Y = scale.TranslateY(a.Y)}
            Else
                ' 直接是绘图数据了？
                ' 不做任何转换
            End If

            Return a
        End Function

        Private Shared Function constraint(pf As PointF, scale As DataScaler) As PointF
            Dim x, y As Single

            If scale.xscale <> scalers.ordinal Then
                If pf.X < Single.MinValue Then
                    x = scale.xmin
                ElseIf pf.X > Single.MaxValue Then
                    x = scale.xmax
                Else
                    x = pf.X
                End If
            Else
                x = pf.X
            End If

            If pf.Y < Single.MinValue Then
                y = scale.ymin
            ElseIf pf.Y > Single.MaxValue Then
                y = scale.ymax
            Else
                y = pf.Y
            End If

            Return New PointF(x, y)
        End Function
    End Class
End Namespace
