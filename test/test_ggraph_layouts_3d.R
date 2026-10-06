require(ggraph);
require(igraph);
options(strict = FALSE);
setwd(@dir);

let edges <- data.frame(
    from = c("a", "b", "c", "d", "e", "a", "c", "f"),
    to = c("b", "c", "d", "e", "a", "f", "f", "g")
);
let net <- graph(from = edges[, "from"], to = edges[, "to"]) |> compute.network();

# layout_cola3d
bitmap(file = "./verify_layout_cola3d.png", size = [1200, 1000]) {
    ggplot(net, padding = "padding:60px;")
      + layout_cola3d(iterations = 40)
      + geom_edge_link()
      + geom_node_point(size = 8)
    ;
}

# layout_force3d
bitmap(file = "./verify_layout_force3d.png", size = [1200, 1000]) {
    ggplot(net, padding = "padding:60px;")
      + layout_force3d(iterations = 300)
      + geom_edge_link()
      + geom_node_point(size = 8)
    ;
}

cat("test_ggraph_layouts3 done!");