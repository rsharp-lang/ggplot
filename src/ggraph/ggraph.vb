Imports System.Drawing
Imports ggplot.ggraph.render
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.visualize.Network.Styling
Imports Microsoft.VisualBasic.Data.visualize.Network.Styling.FillBrushes
Imports Microsoft.VisualBasic.Data.visualize.Network.Styling.Numeric
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.DataSets
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop
Imports any = Microsoft.VisualBasic.Scripting

''' <summary>
''' # A grammar of graphics for relational data
''' 
''' ggraph is an extension of ggplot2 aimed at supporting relational 
''' data structures such as networks, graphs, and trees. While it 
''' builds upon the foundation of ggplot2 and its API it comes with 
''' its own self-contained set of geoms, facets, etc., as well as 
''' adding the concept of layouts to the grammar.
''' </summary>
''' <remarks>
''' ### The core concepts
''' 
''' ggraph builds upon three core concepts that are quite easy To 
''' understand:
'''
''' + The Layout defines how nodes are placed On the plot, that Is, 
'''   it Is a conversion Of the relational Structure into an x And 
'''   y value For Each node In the graph. ggraph has access To all 
'''   layout functions available In igraph And furthermore provides 
'''   a large selection Of its own, such As hive plots, treemaps, 
'''   And circle packing.
''' + The Nodes are the connected entities In the relational Structure. 
'''   These can be plotted Using the geom_node_*() family Of geoms. 
'''   Some node geoms make more sense For certain layouts, e.g. 
'''   geom_node_tile() For treemaps And icicle plots, While others 
'''   are more general purpose, e.g. geom_node_point().
''' + The Edges are the connections between the entities In the 
'''   relational Structure. These can be visualized Using the 
'''   geom_edge_*() family Of geoms that contain a lot Of different 
'''   edge types For different scenarios. Sometimes the edges are implied 
'''   by the layout (e.g. With treemaps) And need Not be plotted, 
'''   but often some sort Of line Is warranted.
''' </remarks>
<Package("ggraph")>
Module ggraphPkg

    <ExportAPI("geom_edge_link")>
    Public Function geom_edge_link(<RDefaultExpression()> Optional mapping As Object = "~aes()") As edgeRender
        Return New edgeRender
    End Function

    <ExportAPI("geom_node_point")>
    Public Function geom_node_point(<RDefaultExpression()>
                                    Optional mapping As Object = "~aes()",
                                    <RRawVectorArgument>
                                    Optional defaultColor As Object = NameOf(Color.SteelBlue),
                                    Optional env As Environment = Nothing) As nodeRender

        Dim fill As IGetBrush = Nothing
        Dim size As IGetSize = Nothing

        If Not mapping Is Nothing Then
            Dim arguments As list = DirectCast(mapping, ggplotReader).args

            fill = any _
                .ToString(arguments.getValue("fill", env, New Object)) _
                .DoCall(AddressOf BrushExpression.Evaluate)
            size = any _
                .ToString(arguments.getValue("size", env, New Object)) _
                .DoCall(AddressOf SizeExpression.Evaluate)
        End If

        Return New nodeRender With {
            .defaultColor = RColorPalette _
                .getColor(defaultColor, NameOf(Color.SteelBlue)) _
                .TranslateColor,
            .fill = fill,
            .radius = size
        }
    End Function

    <ExportAPI("geom_node_text")>
    Public Function geom_node_text(<RDefaultExpression()>
                                   Optional mapping As Object = "~aes()",
                                   Optional iteration As Integer = -1,
                                   Optional env As Environment = Nothing) As textRender

        Dim size As IGetSize = Nothing

        If Not mapping Is Nothing Then
            Dim arguments As list = DirectCast(mapping, ggplotReader).args

            size = any _
               .ToString(arguments.getValue("size", env, New Object)) _
               .DoCall(AddressOf SizeExpression.Evaluate)
        End If

        Return New textRender With {
            .fontsize = size,
            .iteration = iteration
        }
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
        Dim isString As Boolean = args.Length = 1 AndAlso TypeOf args(Scan0).value Is Literal

        If isVector Then
            Dim vec As vector = args(Scan0).Evaluate(env)
            Dim tokens As String() = vec.data _
                .AsObjectEnumerator _
                .Select(Function(a) any.ToString(a)) _
                .ToArray

            mapStr = $"map({key}, [{tokens.JoinBy(", ")}])"
        ElseIf isString Then
            mapStr = $"map({key}, {any.ToString(args(Scan0).Evaluate(env))})"
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
