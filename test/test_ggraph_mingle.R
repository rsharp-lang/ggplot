require(ggraph);
require(igraph);
options(strict = FALSE);
setwd(@dir);

let edges <- data.frame(
    from = c("a", "b", "c", "d", "e", "a", "c", "f", "b", "e"),
    to = c("b", "c", "d", "e", "a", "f", "f", "g", "g", "g")
);
let net <- graph(from = edges[, "from"], to = edges[, "to"]) |> compute.network();

# layout_mingle + geom_edge_bundle
bitmap(file = "./verify_layout_mingle.png", size = [1200, 1000]) {
    ggplot(net, padding = "padding:60px;")
      + layout_mingle()
      + geom_edge_bundle(color = "steelblue")
      + geom_node_point(size = 8)
    ;
}

cat("test_ggraph_mingle done!");