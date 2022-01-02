Imports ggplot
Imports SMRUCC.Rsharp.Runtime

Namespace ggraph

    Public Class graphReader : Inherits ggplotReader

        Public Overrides Function getMapData(data As Object, env As Environment) As ggplotData
            Return MyBase.getMapData(data, env)
        End Function
    End Class
End Namespace