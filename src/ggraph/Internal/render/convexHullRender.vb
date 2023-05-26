#Region "Microsoft.VisualBasic::6748c9fd0c85d1e3f61946a05bc95cd0, ggplot\src\ggraph\Internal\render\convexHullRender.vb"

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

    '   Total Lines: 58
    '    Code Lines: 47
    ' Comment Lines: 0
    '   Blank Lines: 11
    '     File Size: 2.35 KB


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

Namespace ggraph.render

    Public Class convexHullRender : Inherits ggplotConvexhull

        Protected Overrides Function getClassTags(stream As ggplotPipeline) As String()
            Dim [class] As Array = REnv.asVector(Of Object)(reader.class)

            If [class].Length = 1 Then
                Return classFromGraphData(stream, any.ToString([class].GetValue(Scan0)))
            Else
                Return CLRVector.asCharacter([class])
            End If
        End Function

        Protected Overrides Function createColorMaps(class_tags() As String, stream As ggplotPipeline, ngroups As Integer) As Dictionary(Of String, Color)
            Dim nodes As Node() = DirectCast(stream.ggplot.data, NetworkGraph).vertex.ToArray
            Dim maps As New Dictionary(Of String, Color)
            Dim group As String

            For Each v As Node In nodes
                group = v.data(NamesOf.REFLECTION_ID_MAPPING_NODETYPE)
                group = If(group, "")

                If Not maps.ContainsKey(group) Then
                    Call maps.Add(group, DirectCast(v.data.color, SolidBrush).Color)
                End If
            Next

            Return maps
        End Function

        Private Function classFromGraphData(stream As ggplotPipeline, sourceMap As String) As String()
            Dim g As NetworkGraph = stream.ggplot.data
            Dim layouts = stream.layout

            Select Case sourceMap.ToLower
                Case "group"
                    Return layouts _
                        .Select(Function(v)
                                    Dim vex As Node = g.GetElementByID(v.Key)
                                    Dim tag As String = vex.data(NamesOf.REFLECTION_ID_MAPPING_NODETYPE)

                                    Return tag
                                End Function) _
                        .ToArray
                Case Else
                    Throw New NotImplementedException(sourceMap)
            End Select
        End Function
    End Class
End Namespace
