require(ggraph);
require(igraph);
options(strict = FALSE);
setwd(@dir);

let edges <- data.frame(
    from = c("a", "b", "c", "d", "e", "a", "c", "f"),
    to = c("b", "c", "d", "e", "a", "f", "f", "g")
);
let net <- graph(from = edges[, "from"], to = edges[, "to"]) |> compute.network();

# layout_cola
bitmap(file = "./verify_layout_cola.png", size = [1200, 1000]) {
    ggplot(net, padding = "padding:60px;")
      + layout_cola(avoidOverlaps = TRUE)
      + geom_edge_link()
      + geom_node_point(size = 8)
    ;
}

# layout_hola
bitmap(file = "./verify_layout_hola.png", size = [1200, 1000]) {
    ggplot(net, padding = "padding:60px;")
      + layout_hola()
      + geom_edge_link()
      + geom_node_point(size = 8)
    ;
}

# layout_orthogonal + geom_edge_orth
bitmap(file = "./verify_layout_orthogonal.png", size = [1200, 1000]) {
    ggplot(net, padding = "padding:60px;")
      + layout_orthogonal()
      + geom_edge_orth(color = "steelblue")
      + geom_node_point(size = 8)
    ;
}

# layout_mingle + geom_edge_bundle
bitmap(file = "./verify_layout_mingle.png", size = [1200, 1000]) {
    ggplot(net, padding = "padding:60px;")
      + layout_mingle()
      + geom_edge_bundle(color = "steelblue")
      + geom_node_point(size = 8)
    ;
}

# layout_force3d
bitmap(file = "./verify_layout_force3d.png", size = [1200, 1000]) {
    ggplot(net, padding = "padding:60px;")
      + layout_force3d(iterations = 500)
      + geom_edge_link()
      + geom_node_point(size = 8)
    ;
}

# layout_cola3d
bitmap(file = "./verify_layout_cola3d.png", size = [1200, 1000]) {
    ggplot(net, padding = "padding:60px;")
      + layout_cola3d(iterations = 60)
      + geom_edge_link()
      + geom_node_point(size = 8)
    ;
}

cat("test_ggraph_layouts2 done!");