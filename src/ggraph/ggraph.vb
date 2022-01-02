Imports System.Drawing
Imports ggplot.ggraph.render
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.DataSets
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop
Imports any = Microsoft.VisualBasic.Scripting

<Package("ggraph")>
Module ggraphPkg

    <ExportAPI("geom_edge_link")>
    Public Function geom_edge_link() As edgeRender
        Return New edgeRender
    End Function

    <ExportAPI("geom_node_point")>
    Public Function geom_node_point(<RRawVectorArgument>
                                    Optional defaultColor As Object = NameOf(Color.SteelBlue),
                                    Optional env As Environment = Nothing) As nodeRender
        Return New nodeRender With {
            .defaultColor = RColorPalette _
                .getColor(defaultColor, NameOf(Color.SteelBlue)) _
                .TranslateColor
        }
    End Function

    <ExportAPI("geom_node_text")>
    Public Function geom_node_text() As textRender
        Return New textRender
    End Function

    ''' <summary>
    ''' create style mapping for do graph rendering
    ''' </summary>
    ''' <param name="key">the graph data property name key</param>
    ''' <param name="vals">mapping values</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("map")>
    Public Function map(key As String,
                        <RRawVectorArgument>
                        <RListObjectArgument>
                        Optional vals As Object = Nothing,
                        Optional env As Environment = Nothing) As String

        Dim mapStr As String
        Dim args As InvokeParameter() = DirectCast(vals, InvokeParameter())
        Dim isVector As Boolean = args.Length = 1 AndAlso TypeOf args(Scan0).value Is VectorLiteral

        If isVector Then
            Dim vec As vector = args(Scan0).Evaluate(env)
            Dim tokens As String() = vec.data _
                .AsObjectEnumerator _
                .Select(Function(a) any.ToString(a)) _
                .ToArray

            mapStr = $"map({key}, [{tokens.JoinBy(", ")}])"
        Else
            Dim listData As NamedValue(Of Object)() = RListObjectArgumentAttribute _
                .getObjectList(vals, env) _
                .ToArray
            Dim keyVals As String() = listData _
                .Select(Function(t)
                            Return $"{t.Name}={any.ToString(t.Value)}"
                        End Function) _
                .ToArray

            mapStr = $"map({key}, {keyVals.JoinBy(", ")})"
        End If

        Return mapStr
    End Function

End Module
