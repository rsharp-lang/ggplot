require(igraph);
require(ggplot);
require(JSON);

json = json_decode(readText(`${@dir}/TCACycle.json`));

# str(json);

nodes = as.data.frame(json$nodeDataArray);
links = as.data.frame(json$linkDataArray);

print(nodes);
print(links);
