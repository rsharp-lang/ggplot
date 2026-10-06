require(ggraph);
require(igraph);
options(strict = FALSE);
setwd(@dir);

# 构建一个小图数据源
let edges <- data.frame(
    from = c("a", "b", "c", "d", "e", "a", "c", "f"),
    to = c("b", "c", "d", "e", "a", "f", "f", "g")
);
let net <- graph(from = edges[, "from"], to = edges[, "to"]) |> compute.network();

# 1. layout_random
bitmap(file = "./verify_layout_random.png", size = [1200, 1000]) {
    ggplot(net, padding = "padding:60px;")
      + layout_random(2024)
      + geom_edge_link()
      + geom_node_point(size = 8)
    ;
}

# 2. layout_forcedirected
bitmap(file = "./verify_layout_forcedirected.png", size = [1200, 1000]) {
    ggplot(net, padding = "padding:60px;")
      + layout_forcedirected(iterations = 800)
      + geom_edge_link()
      + geom_node_point(size = 8)
    ;
}

# 3. layout_circular
bitmap(file = "./verify_layout_circular.png", size = [1200, 1000]) {
    ggplot(net, padding = "padding:60px;")
      + layout_circular(crossingOptimization = TRUE)
      + geom_edge_link()
      + geom_node_point(size = 8)
    ;
}

# 4. layout_radial
bitmap(file = "./verify_layout_radial.png", size = [1200, 1000]) {
    ggplot(net, padding = "padding:60px;")
      + layout_radial()
      + geom_edge_link()
      + geom_node_point(size = 8)
    ;
}

# 5. layout_springembedder
bitmap(file = "./verify_layout_springembedder.png", size = [1200, 1000]) {
    ggplot(net, padding = "padding:60px;")
      + layout_springembedder([1000, 1000], iterations = 100)
      + geom_edge_link()
      + geom_node_point(size = 8)
    ;
}

cat("test_ggraph_layouts done!");