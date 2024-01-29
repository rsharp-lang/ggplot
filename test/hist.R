require(ggplot);

options(strict = FALSE);

let data = read.csv(file = `${@dir}/log2FC.csv`);
data[, "-log10(p-value)"] = -log10(data[, "p.value"]);

print(head(data));
setwd(@dir);

bitmap(file = "./pvalue_hist.png", size = [2700, 2000]) {
	ggplot(data, aes(x = "-log10(p-value)"), padding = "padding:250px 600px 250px 300px;")
	 + geom_histogram(bins = 50,  color = "steelblue")
	 + ggtitle("Frequency of -logP")
	        + scale_x_continuous(labels = "F2")
       + scale_y_continuous(labels = "F0")
	 + theme_default()
	 ;
}

svg(file = "./pvalue_hist.svg", size = [2700, 2000]) {
	ggplot(data, aes(x = "-log10(p-value)"), padding = "padding:250px 600px 250px 300px;")
	 + geom_histogram(bins = 50,  color = "steelblue")
	 + ggtitle("Frequency of -logP")
	        + scale_x_continuous(labels = "F2")
       + scale_y_continuous(labels = "F0")
	 + theme_default()
	 ;
}