Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("ggplot2")>
Public Module ggplot2

    <ExportAPI("ggplot")>
    Public Function ggplot(<RRawVectorArgument>
                           Optional data As Object = Nothing,
                           Optional mapping As Object = "~aes()",
                           <RListObjectArgument>
                           Optional args As list = Nothing,
                           Optional environment As Environment = Nothing)

        Dim base As ggplotLayer = New ggplotBase With {.reader = mapping}
        Dim theme As New Theme

        Return New ggplot(theme) With {
            .data = data,
            .layers = New Queue(Of ggplotLayer),
            .base = base,
            .args = args,
            .environment = environment,
            .xlabel = base.reader.x,
            .ylabel = base.reader.y
        }
    End Function

    <ExportAPI("aes")>
    Public Function aes(x As Object, y As Object, Optional env As Environment = Nothing) As Object
        Return New ggplotReader With {.x = x, .y = y}
    End Function

End Module
