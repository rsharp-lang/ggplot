require(ggplot);

setwd(@dir);

data = read.csv("../../data/demo/sampleData.csv", row.names = FALSE, check.names = FALSE);
sampleinfo = read.csv("../../data/demo/sampleinfo.csv", row.names = NULL, check.names = FALSE);
sampleinfo = as.list(sampleinfo, byrow = TRUE)
|> groupBy(i -> i$sample_info)
|> lapply(function(list) {
	id = sapply(list, i -> i$ID);
	id = id[1:32];
	tag = list$key;
	color = sapply(list, i -> i$color) |> unique();
	
	list(id, color = `#${color}`, tag = tag);
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
	list(data = unlist(target[i$id]), color = i$color, tag = i$tag);
});

tags = [];
data = [];
colors = [];

for(i in groups) {
	x = i$data;
	tags = append(tags, rep(i$tag, times = length(x)));
	data = append(data, x);
	colors = append(colors, rep(i$color, times = length(x)));
}

groups = data.frame(
	tags, data, colors
);

print(groups);

print(name);

bitmap(file = "./jitter.png", size = [2400,1600]) {

	ggplot(groups, aes(x = "tags", y = "data", color = "colors"))
	+ geom_violin()
	+ geom_jitter()
	+ theme(axis.text.x = element_text(angle = 45))
	;

}

