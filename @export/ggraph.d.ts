// export R# package module type define for javascript/typescript language
//
// ref=ggplot.ggraphPkg@ggraph, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * # A grammar of graphics for relational data
 *  
 *  ggraph is an extension of ggplot2 aimed at supporting relational 
 *  data structures such as networks, graphs, and trees. While it 
 *  builds upon the foundation of ggplot2 and its API it comes with 
 *  its own self-contained set of geoms, facets, etc., as well as 
 *  adding the concept of layouts to the grammar.
 * 
 * > ### The core concepts
 * >  
 * >  ggraph builds upon three core concepts that are quite easy To 
 * >  understand:
 * > 
 * >  + The Layout defines how nodes are placed On the plot, that Is, 
 * >    it Is a conversion Of the relational Structure into an x And 
 * >    y value For Each node In the graph. ggraph has access To all 
 * >    layout functions available In igraph And furthermore provides 
 * >    a large selection Of its own, such As hive plots, treemaps, 
 * >    And circle packing.
 * >  + The Nodes are the connected entities In the relational Structure. 
 * >    These can be plotted Using the geom_node_*() family Of geoms. 
 * >    Some node geoms make more sense For certain layouts, e.g. 
 * >    geom_node_tile() For treemaps And icicle plots, While others 
 * >    are more general purpose, e.g. geom_node_point().
 * >  + The Edges are the connections between the entities In the 
 * >    relational Structure. These can be visualized Using the 
 * >    geom_edge_*() family Of geoms that contain a lot Of different 
 * >    edge types For different scenarios. Sometimes the edges are implied 
 * >    by the layout (e.g. With treemaps) And need Not be plotted, 
 * >    but often some sort Of line Is warranted.
*/
declare namespace ggraph {
   /**
     * @param mapping default value Is ``null``.
     * @param color default value Is ``'lightgray'``.
     * @param width default value Is ``'2,5'``.
     * @param env default value Is ``null``.
   */
   function geom_edge_link(mapping?: any, color?: any, width?: any, env?: object): object;
   /**
     * @param mapping default value Is ``null``.
     * @param alpha default value Is ``1``.
     * @param scale default value Is ``1.125``.
     * @param stroke_width default value Is ``3``.
     * @param spline default value Is ``0``.
   */
   function geom_node_convexHull(mapping?: object, alpha?: number, scale?: number, stroke_width?: number, spline?: number): object;
   /**
     * @param mapping default value Is ``null``.
     * @param defaultColor default value Is ``'SteelBlue'``.
     * @param env default value Is ``null``.
   */
   function geom_node_point(mapping?: any, defaultColor?: any, env?: object): object;
   /**
     * @param mapping default value Is ``null``.
     * @param iteration default value Is ``-1``.
     * @param env default value Is ``null``.
   */
   function geom_node_text(mapping?: any, iteration?: object, env?: object): object;
   /**
    * create style mapping for do graph rendering
    * 
    * 
     * @param key the graph data property name key
     * @param vals mapping values
     * 
     * + default value Is ``null``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function map(key: string, vals?: any, env?: object): string;
}
