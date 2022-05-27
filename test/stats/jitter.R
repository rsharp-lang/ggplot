require(ggplot);

setwd(@dir);

data = read.csv("../../data/demo/sampleData.csv", row.names = FALSE, check.names = FALSE);
sampleinfo = read.csv("../../data/demo/sampleinfo.csv", row.names = NULL, check.names = FALSE);
sampleinfo = as.list(sampleinfo, byrow = TRUE)
|> groupBy(i -> i$sample_info)
|> lapply(function(list) {
	id = sapply(list, i -> i$ID);
	color = sapply(list, i -> i$color) |> unique();
	
	list(id, color = `#${color}`);
}, names = i -> i$key)
;

str(sampleinfo);

data = as.list(data, byrow = TRUE);

# str(data);

target = data[[1]];
name = target[[1]];
target[[1]] = NULL;

str(target);

groups = lapply(sampleinfo, function(i) {
	list(data = unlist(target[i$id]), color = i$color);
});
group_names = names(groups);
group_datas = lapply(groups, function(v) v$data) |> as.data.frame();

print(group_datas, max.print = 13);

print(name);

bitmap(file = "./jitter.png") {

	ggplot(groups, aes())

}

