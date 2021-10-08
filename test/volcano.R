require(ggplot);

const volcano = read.csv(`${@dir}/log2FC.csv`);

volcano[, "class"]   = ifelse(as.numeric(volcano[, "p.value"]) < 0.05, "sig", "no-sig");
volcano[, "class"]   = ifelse(abs(volcano[, "log2FC"]) < 1, "no-sig", volcano[, "class"]);
volcano[, "p.value"] = -log10(volcano[, "p.value"]);

print("peeks of the raw data:");
print(head(volcano));

bitmap(file = `${@dir}/volcano.png`, size = [3000, 3600]) {
	ggplot(volcano, aes(x = "log2FC", y = "p.value", color = "class"), padding = "padding:150px 100px 200px 250px;")
	   + geom_point(color = "black", shape = "Circle", size = 21)
       + labs(x = "log2(FoldChange)", y = "-log10(P-value)")
       + ggtitle("Volcano Plot")
       + scale_x_continuous(labels = "F2")
       + scale_y_continuous(labels = "F2")
	;
}