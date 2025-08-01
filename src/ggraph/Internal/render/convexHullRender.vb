﻿#Region "Microsoft.VisualBasic::d2ca024ae1f5795e7bebef277ecfe242, src\ggraph\Internal\render\convexHullRender.vb"

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

    '   Total Lines: 111
    '    Code Lines: 93 (83.78%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 18 (16.22%)
    '     File Size: 4.54 KB


    '     Class convexHullRender
    ' 
    '         Function: classFromGraphData, createColorMaps, getClassTags
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.visualize.Network.FileStream.Generic
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports any = Microsoft.VisualBasic.Scripting
Imports REnv = SMRUCC.Rsharp.Runtime
Imports Microsoft.VisualBasic.ComponentModel.DataStructures
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors



#If NET48 Then
Imports Pen = System.Drawing.Pen
Imports Pens = System.Drawing.Pens
Imports Brush = System.Drawing.Brush
Imports Font = System.Drawing.Font
Imports Brushes = System.Drawing.Brushes
Imports SolidBrush = System.Drawing.SolidBrush
Imports DashStyle = System.Drawing.Drawing2D.DashStyle
Imports Image = System.Drawing.Image
Imports Bitmap = System.Drawing.Bitmap
Imports GraphicsPath = System.Drawing.Drawing2D.GraphicsPath
Imports LineCap = System.Drawing.Drawing2D.LineCap
Imports TextureBrush = System.Drawing.TextureBrush
#Else
Imports Pen = Microsoft.VisualBasic.Imaging.Pen
Imports Pens = Microsoft.VisualBasic.Imaging.Pens
Imports Brush = Microsoft.VisualBasic.Imaging.Brush
Imports Font = Microsoft.VisualBasic.Imaging.Font
Imports Brushes = Microsoft.VisualBasic.Imaging.Brushes
Imports SolidBrush = Microsoft.VisualBasic.Imaging.SolidBrush
Imports DashStyle = Microsoft.VisualBasic.Imaging.DashStyle
Imports Image = Microsoft.VisualBasic.Imaging.Image
Imports Bitmap = Microsoft.VisualBasic.Imaging.Bitmap
Imports GraphicsPath = Microsoft.VisualBasic.Imaging.GraphicsPath
Imports LineCap = Microsoft.VisualBasic.Imaging.LineCap
Imports TextureBrush = Microsoft.VisualBasic.Imaging.TextureBrush
#End If

Namespace ggraph.render

    Public Class convexHullRender : Inherits ggplotConvexhull

        Protected Overrides Function getClassTags(stream As ggplotPipeline) As String()
            Dim [class] As String() = CLRVector.asCharacter(reader.class)

            If [class].Length = 1 Then
                Return classFromGraphData(stream, [class](Scan0))
            Else
                Return CLRVector.asCharacter([class])
            End If
        End Function

        Protected Overrides Function createColorMaps(class_tags() As String, stream As ggplotPipeline, ngroups As Integer) As Dictionary(Of String, Color)
            Dim maps As New Dictionary(Of String, Color)
            Dim group As String
            Dim g As NetworkGraph = DirectCast(stream.ggplot.data, NetworkGraph)

            If class_tags.IsNullOrEmpty Then
                Dim nodes As Node() = g.vertex.ToArray

                For Each v As Node In nodes
                    group = v.data(NamesOf.REFLECTION_ID_MAPPING_NODETYPE)
                    group = If(group, "")

                    If Not maps.ContainsKey(group) Then
                        Call maps.Add(group, DirectCast(v.data.color, SolidBrush).Color)
                    End If
                Next
            Else
                Dim group_colors As Color() = Designer.GetColors("paper", class_tags.Distinct.Count)
                Dim unique_groups = class_tags.Distinct.ToArray

                For i As Integer = 0 To unique_groups.Length - 1
                    Call maps.Add(If(unique_groups(i), "No Group"), group_colors(i))
                Next
            End If

            Return maps
        End Function

        Private Function classFromGraphData(stream As ggplotPipeline, sourceMap As String) As String()
            Dim g As NetworkGraph = stream.ggplot.data
            Dim layouts = stream.layout
            Dim groups As String()
            Dim keyName As String

            Select Case sourceMap.ToLower
                Case "group" : keyName = NamesOf.REFLECTION_ID_MAPPING_NODETYPE
                Case Else
                    keyName = sourceMap
            End Select

            groups = layouts _
                .Select(Function(v)
                            Dim vex As Node = g.GetElementByID(v.Key)
                            Dim tag As String = vex.data(keyName)

                            Return tag
                        End Function) _
                .ToArray

            If groups.All(Function(str) str Is Nothing) Then
                Throw New MissingMemberException($"missing group class mapping source, property attribute '{keyName}' should be existed in node metadata!")
            Else
                Return groups
            End If
        End Function
    End Class
End Namespace
