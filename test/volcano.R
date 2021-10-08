require(ggplot);

const volcano = read.csv(`${@dir}/log2FC.csv`);

volcano[, "factor"]  = ifelse(volcano[, "log2FC"] >  log2(1.25), "Up", "Not Sig");
volcano[, "factor"]  = ifelse(volcano[, "log2FC"] < -log2(1.25), "Down", volcano[, "factor"]);
volcano[, "factor"]  = ifelse(volcano[, "p.value"] < 0.05, volcano[, "factor"], "Not Sig");
volcano[, "p.value"] = -log10(volcano[, "p.value"]);

print("peeks of the raw data:");
print(head(volcano));
print("count of the factors:");
print(`Up:      ${sum("Up" == volcano[, "factor"])}`);
print(`Not Sig: ${sum("Not Sig" == volcano[, "factor"])}`);
print(`Down:    ${sum("Down" == volcano[, "factor"])}`);

bitmap(file = `${@dir}/volcano.png`, size = [3000, 3600]) {
	ggplot(volcano, aes(x = "log2FC", y = "p.value"), padding = "padding:350px 100px 250px 350px;")
	    + geom_point(aes(color = "factor"), color = "black", shape = "Circle", size = 21)
       + scale_colour_manual(values = list(
          Up        = "red",
          "Not Sig" = "gray",
          Down      = "skyblue"
       ))
       + labs(x = "log2(FoldChange)", y = "-log10(P-value)")
       + ggtitle("Volcano Plot")
       + scale_x_continuous(labels = "F2")
       + scale_y_continuous(labels = "F2")
	;
}