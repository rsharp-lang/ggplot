Imports Microsoft.VisualBasic.CommandLine.Reflection
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

        Return New ggplot With {
            .data = data,
            .layers = New Queue(Of ggplotLayer),
            .base = base,
            .args = args
        }
    End Function

    <ExportAPI("aes")>
    Public Function aes(x As Object, y As Object, Optional env As Environment = Nothing) As Object
        Return New ggplotReader With {.x = x, .y = y}
    End Function

End Module
