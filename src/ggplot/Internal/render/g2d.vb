﻿#Region "Microsoft.VisualBasic::22415c36ac9b63bb0a6598b47258c6df, src\ggplot\Internal\render\g2d.vb"

    ' Author:
    ' 
    '       xieguigang (I@xieguigang.me)
    ' 
    ' Copyright (c) 2021 R# language
    ' 
    ' 
    ' MIT License
    ' 
    ' 
    ' Permission is hereby granted, free of charge, to any person obtaining a copy
    ' of this software and associated documentation files (the "Software"), to deal
    ' in the Software without restriction, including without limitation the rights
    ' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    ' copies of the Software, and to permit persons to whom the Software is
    ' furnished to do so, subject to the following conditions:
    ' 
    ' The above copyright notice and this permission notice shall be included in all
    ' copies or substantial portions of the Software.
    ' 
    ' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    ' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    ' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    ' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    ' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    ' SOFTWARE.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 161
    '    Code Lines: 125 (77.64%)
    ' Comment Lines: 7 (4.35%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 29 (18.01%)
    '     File Size: 6.98 KB


    '     Module g2d
    ' 
    '         Function: (+2 Overloads) get2DScale, validateAxis
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports ggplot.elements
Imports ggplot.layers
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports Microsoft.VisualBasic.Math.Quantile
Imports SMRUCC.Rsharp.Runtime.Vectorization

Namespace render

    Module g2d

        <Extension>
        Public Function get2DScale(ggplot As ggplot,
                                   rect As Rectangle,
                                   [default] As (x As String(), y As Double()),
                                   layerData As IEnumerable(Of ggplotData)) As DataScaler

            Dim allDataset As ggplotData() = layerData.ToArray
            Dim y As Double() = allDataset _
                .Select(Function(d)
                            Return d.y.ToNumeric
                        End Function) _
                .IteratesALL _
                .ToArray
            Dim limitsY As Double() = CLRVector.asNumeric(ggplot.args.getByName("range_y"))

            y = y _
                .JoinIterates([default].y) _
                .JoinIterates(limitsY) _
                .Where(Function(d) Not d.IsNaNImaginary) _
                .ToArray
            y = validateAxis(y, ggplot)

            Dim hasViolin As Boolean = ggplot.layers _
                .Any(Function(layer)
                         Return TypeOf layer Is ggplotViolin OrElse
                             TypeOf layer Is ggplotBoxplot
                     End Function)
            Dim theme As Theme = ggplot.ggplotTheme

            If hasViolin OrElse ggplot.layers.Any(Function(layer) TypeOf layer Is ggplotBoxplot) Then
                For Each group In ggplotGroup.getDataGroups([default].x, [default].y)
                    Dim quartile As DataQuartile = group.Quartile
                    Dim lowerBound = quartile.Q1 - 1.5 * quartile.IQR
                    Dim upperBound = quartile.Q3 + 1.5 * quartile.IQR

                    If lowerBound < 0 Then
                        If Not hasViolin Then
                            lowerBound = 0
                        End If
                    End If

                    y = y.JoinIterates({upperBound}).ToArray
                    ' y = y.JoinIterates({upperBound, lowerBound}).ToArray
                Next
            End If

            y = y _
                .JoinIterates({y.Max * 1.125}) _
                .ToArray

            Dim yTicks = y.Range.CreateAxisTicks(theme.nticksY, theme.GetYAxisDecimals)
            Dim scaleX = d3js.scale.ordinal.domain(tags:=[default].x).range(integers:={rect.Left, rect.Right})
            Dim scaleY = d3js.scale.linear.domain(values:=yTicks).range(integers:={rect.Bottom, rect.Top})
            Dim scale As New DataScaler() With {
                .AxisTicks = (Nothing, yTicks.AsVector),
                .region = rect,
                .X = scaleX,
                .Y = scaleY
            }

            Return scale
        End Function

        <Extension>
        Public Function get2DScale(ggplot As ggplot,
                                   rect As Rectangle,
                                   [default] As (x As Double(), y As Double()),
                                   fixedRange As Boolean,
                                   layerData As IEnumerable(Of ggplotData)) As DataScaler

            Dim allDataset As ggplotData() = layerData.ToArray
            Dim x As Double() = allDataset.Select(Function(d) CLRVector.asNumeric(d.x)).IteratesALL.ToArray
            Dim y As Double() = allDataset.Select(Function(d) CLRVector.asNumeric(d.y)).IteratesALL.ToArray
            Dim limitsX As Double() = CLRVector.asNumeric(ggplot.args.getByName("range_x"))
            Dim limitsY As Double() = CLRVector.asNumeric(ggplot.args.getByName("range_y"))
            Dim theme As Theme = ggplot.ggplotTheme

            ' there are missing value from the 
            ' geom_vline and geom_hline
            ' function
            x = x.JoinIterates([default].x).JoinIterates(limitsX).Where(Function(d) Not d.IsNaNImaginary).ToArray
            y = y.JoinIterates([default].y).JoinIterates(limitsY).Where(Function(d) Not d.IsNaNImaginary).ToArray
            y = validateAxis(y, ggplot)

            Dim xTicks As Double() = If(x.IsNullOrEmpty, {}, x.Range.CreateAxisTicks(theme.nticksX, theme.GetXAxisDecimals))
            Dim yTicks As Double() = If(y.IsNullOrEmpty, {}, y.Range.CreateAxisTicks(theme.nticksY, theme.GetYAxisDecimals))

            If fixedRange AndAlso Not x.IsNullOrEmpty Then
                Dim raw_x As New DoubleRange(x)

                xTicks = xTicks.Where(Function(xi) raw_x.IsInside(xi)).ToArray
                xTicks = {x.Min}.JoinIterates(xTicks).JoinIterates({x.Max}).ToArray
            End If

            If fixedRange AndAlso Not y.IsNullOrEmpty Then
                Dim raw_y As New DoubleRange(y)

                yTicks = yTicks.Where(Function(yi) raw_y.IsInside(yi)).ToArray
                yTicks = {y.Min}.JoinIterates(yTicks).JoinIterates({y.Max}).ToArray
            End If

            Dim scaleX = d3js.scale.linear.domain(values:=xTicks).range(integers:={rect.Left, rect.Right})
            Dim scaleY = d3js.scale.linear.domain(values:=yTicks).range(integers:={rect.Bottom, rect.Top})
            Dim scale As New DataScaler() With {
                .AxisTicks = (xTicks.AsVector, yTicks.AsVector),
                .region = rect,
                .X = scaleX,
                .Y = scaleY
            }

            Return scale
        End Function

        Private Function validateAxis(y As Double(), ggplot As ggplot) As Double()
            Dim maps As New List(Of axisMap)
            Dim layerMap As axisMap

            For Each layer As ggplotLayer In ggplot.layers
                layerMap = layer.getYAxis(y, ggplot)

                If Not layerMap Is Nothing Then
                    Call maps.Add(layerMap)
                End If
            Next

            ' no mapping was defined, use the default value
            If maps.IsNullOrEmpty Then
                Return y
            End If

            ' check mapper type at first
            Dim check_mapper As Boolean = maps.Select(Function(a) a.mapper).Distinct.Count = 1

            If Not check_mapper Then
                Throw New InvalidOperationException("The axis mapping of the value data type is not mutually compatible!")
            End If

            ' check value range 
            Dim ys = maps.Select(Function(a) a.ToNumeric).IteratesALL.ToArray
            Return ys
        End Function
    End Module
End Namespace
