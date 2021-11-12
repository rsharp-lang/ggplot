require(ggplot);

options(strict = FALSE);

data = read.csv(file = `${@dir}/log2FC.csv`);
data[, "-log10(p-value)"] = -log10(data[, "p.value"]);

print(head(data));

bitmap(file = `${@dir}/pvalue_hist.png`, size = [3000, 2400]) {
	ggplot(data, aes(x = "-log10(p-value)"), padding = "padding:250px 600px 250px 300px;")
	 + geom_histogram(bins = 50,  color = "steelblue")
	 + ggtitle("Frequency of -logP")
	 + theme_black()
	 ;
}