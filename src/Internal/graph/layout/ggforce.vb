Imports ggplot.options
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph

Namespace ggraph.layout

    Public MustInherit Class ggforce : Inherits ggplotOption

        Public MustOverride Sub createLayout(g As NetworkGraph)

        Public Overrides Function Config(ggplot As ggplot) As ggplot
            Return ggplot
        End Function

    End Class
End Namespace