Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Imaging.d3js.scale

Namespace layers

    Public Class ggplotStatsLayer : Inherits ggplotGroup

        ' compare_means(len~dose, data=ToothGrowth)
        ' ## A tibble: 3 x 8
        ' #  .y.   group1 group2           p    p.adj p.format p.signif
        ' #  <chr> <chr>  <chr>        <dbl>    <dbl> <chr>    <chr>   
        ' #1 len   0.5    1          7.02e-6   1.4e-5 7.0e-06  ****    
        ' #2 len   0.5    2          8.41e-8   2.5e-7 8.4e-08  ****    
        ' #3 len   1      2          1.77e-4   1.8e-4 0.00018  ***     
        ' ## … with 1 more variable: method <chr>

        Protected Overrides Function PlotOrdinal(stream As ggplotPipeline, x As OrdinalScale) As IggplotLegendElement
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace