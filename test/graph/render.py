import igraph
import ggplot
import JSON

def graph_ggplot(g):
    plt = ggplot(g) + geom_edge_link()
    plt = plt + geom_node_point(aes(size = ggraph::map("degree", [9, 50]), fill = ggraph::map("group", "paper"))) 
    plt = plt + geom_node_text(aes(size = ggraph::map("degree", [4, 9]))) 
    plt = plt + layout_springforce(iterations = 10000)

    svg(plt, file = `${@dir}/graph.svg`, size = [1920, 1200], dpi = 300)

def create_graph(json):
    nodes = as.data.frame(json$nodeDataArray)
    nodes = nodes[nodes[, "category"] != "valve", ]

    links      = as.data.frame(json$linkDataArray)
    groupNames = nodes[as.logical(nodes[, "isGroup"]), ]
    groupNames = as.list(groupNames, byrow = TRUE)
    groupNames = lapply(groupNames, r -> r$text, names = r -> r$key)

    str(groupNames)
    print(nodes, max.print = 8)

    g = graph(from = links[, "from"], to = links[, "to"])
    g = compute.network(g)
    v = V(g)

    print(xref(v))

    xref = xref(v)
    i    = sapply(nodes[, "key"], id -> which(id == xref))

    print(i)

    v$label = (nodes[, "label"])[i]
    v$group = sapply(nodes[, "group"][i], key -> ifelse(key in groupNames, groupNames[[key]], "undefined"))

    print(v$label)
    print(v$group)

    return g

json = json_decode(readText(`${@dir}/TCACycle.json`))
g    = create_graph(json)

graph_ggplot(g)