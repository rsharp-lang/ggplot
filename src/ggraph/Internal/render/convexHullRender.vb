Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.visualize.Network.FileStream.Generic
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports any = Microsoft.VisualBasic.Scripting
Imports REnv = SMRUCC.Rsharp.Runtime

Namespace ggraph.render

    Public Class convexHullRender : Inherits ggplotConvexhull

        Protected Overrides Function getClassTags(stream As ggplotPipeline) As String()
            Dim [class] As Array = REnv.asVector(Of Object)(reader.class)

            If [class].Length = 1 Then
                Return classFromGraphData(stream, any.ToString([class].GetValue(Scan0)))
            Else
                Return REnv.asVector(Of String)([class])
            End If
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