Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Language.Vectorization
Imports Microsoft.VisualBasic.Linq
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Internal.Object.Converts
Imports REnv = SMRUCC.Rsharp.Runtime

Public MustInherit Class ggplotLayer

    Public Property reader As ggplotReader
    Public Property colorMap As ggplotColorMap
    Public Property showLegend As Boolean = True
    Public Property which As Expression

    Protected ReadOnly Property useCustomData As Boolean
        Get
            If reader Is Nothing Then Return False

            Return Not (reader.y.StringEmpty AndAlso reader.label.StringEmpty)
        End Get
    End Property

    Protected ReadOnly Property useCustomColorMaps As Boolean
        Get
            Return (Not reader Is Nothing) AndAlso Not reader.color Is Nothing
        End Get
    End Property

    Public MustOverride Function Plot(
        g As IGraphics,
        canvas As GraphicsRegion,
        baseData As ggplotData,
        x As Double(),
        y As Double(),
        scale As DataScaler,
        ggplot As ggplot,
        theme As Theme
    ) As legendGroupElement

    Public Function getFilter(ggplot As ggplot) As BooleanVector
        Dim i As New List(Of Object)
        Dim measure As New Environment(ggplot.environment, ggplot.environment.stackFrame, isInherits:=False)
        Dim x = DirectCast(ggplot.data, dataframe).colnames _
            .SeqIterator _
            .ToArray

        For Each var In x
            Call measure.Push(var.value, Nothing, [readonly]:=False)
        Next

        For Each row As NamedCollection(Of Object) In DirectCast(ggplot.data, dataframe).forEachRow(x.Select(Function(xi) xi.value).ToArray)
            For Each var In x
                Call measure(var.value).SetValue(row(var), measure)
            Next

            i.Add(which.Evaluate(measure))
        Next

        Return New BooleanVector(REnv.asLogical(i.ToArray))
    End Function

End Class