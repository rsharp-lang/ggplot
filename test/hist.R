require(ggplot);

options(strict = FALSE);

data = read.csv(file = `${@dir}/log2FC.csv`);
data[, "-log10(p-value)"] = -log10(data[, "p.value"]);

print(head(data));

bitmap(file = `${@dir}/pvalue_hist.png`) {
	ggplot(data, aes(x = "-log10(p-value)")) + geom_histogram(bins = 50);
}