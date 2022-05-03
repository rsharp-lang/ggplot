# ggraph

# A grammar of graphics for relational data
 
 ggraph is an extension of ggplot2 aimed at supporting relational 
 data structures such as networks, graphs, and trees. While it 
 builds upon the foundation of ggplot2 and its API it comes with 
 its own self-contained set of geoms, facets, etc., as well as 
 adding the concept of layouts to the grammar.
> ### The core concepts
>  
>  ggraph builds upon three core concepts that are quite easy To 
>  understand:
> 
>  + The Layout defines how nodes are placed On the plot, that Is, 
>    it Is a conversion Of the relational Structure into an x And 
>    y value For Each node In the graph. ggraph has access To all 
>    layout functions available In igraph And furthermore provides 
>    a large selection Of its own, such As hive plots, treemaps, 
>    And circle packing.
>  + The Nodes are the connected entities In the relational Structure. 
>    These can be plotted Using the geom_node_*() family Of geoms. 
>    Some node geoms make more sense For certain layouts, e.g. 
>    geom_node_tile() For treemaps And icicle plots, While others 
>    are more general purpose, e.g. geom_node_point().
>  + The Edges are the connections between the entities In the 
>    relational Structure. These can be visualized Using the 
>    geom_edge_*() family Of geoms that contain a lot Of different 
>    edge types For different scenarios. Sometimes the edges are implied 
>    by the layout (e.g. With treemaps) And need Not be plotted, 
>    but often some sort Of line Is warranted.

+ [geom_edge_link](ggraph/geom_edge_link.1) 
+ [geom_node_point](ggraph/geom_node_point.1) 
+ [geom_node_text](ggraph/geom_node_text.1) 
+ [map](ggraph/map.1) create style mapping for do graph rendering
