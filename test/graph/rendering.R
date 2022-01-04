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

svg(file = `${@dir}/graph.svg`, size = [1920, 1200], dpi = 300) {
    ggplot(g) 
    + geom_edge_link() 
    + geom_node_point(aes(
        size = ggraph::map("degree", [9, 50]), 
        fill = ggraph::map("group", "paper"))
    ) 
    + geom_node_text(aes(size = ggraph::map("degree", [4, 9]))) 
    + layout_springforce(iterations = 10000)
    ;
}