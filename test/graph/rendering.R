require(igraph);
require(ggplot);
require(JSON);

json = json_decode(readText(`${@dir}/TCACycle.json`));

# str(json);

nodes = as.data.frame(json$nodeDataArray);
nodes = nodes[(nodes[, "category"] != "valve"), ];

links = as.data.frame(json$linkDataArray);
groupNames = nodes[as.logical(nodes[, "isGroup"]), ];
groupNames = as.list(groupNames, byrow = TRUE);
groupNames = lapply(groupNames, r -> r$text, names = r -> r$key);

str(groupNames);

print(nodes);
# print(links);

g = graph(from = links[, "from"], to = links[, "to"])
|> compute.network()
;
v = V(g);

print(xref(v));

xref = xref(v);

i = sapply(nodes[, "key"], id -> which(id == xref));
print(i);
v$label = (nodes[, "label"])[i];
v$group = sapply((nodes[, "group"])[i], key -> ifelse(key in groupNames, groupNames[[key]], "undefined"));

print(v$label);
print(v$group);

bitmap(file = `${@dir}/graph.png`, size = [3200, 2700]) {
    ggplot(g) 
    + geom_edge_link() 
    + geom_node_point(aes(
        size = ggraph::map("degree", [10, 120]), 
        fill = ggraph::map("group", "paper"))
    ) 
    + geom_node_text(aes(size = ggraph::map("degree", [6, 12]))) 
    + layout_forcedirected(
        ejectFactor = 6,
        condenseFactor = 3,
        maxtx = 4,
        maxty = 3,
        dist_threshold = [30,250],
        size = [1000,1000],
    )
    ;
}