require(ggplot);

const volcano    = read.csv(`${@dir}/log2FC.csv`);
const foldchange = 1.5;

volcano[, "factor"]  = ifelse(volcano[, "log2FC"] >  log2(foldchange), "Up", "Not Sig");
volcano[, "factor"]  = ifelse(volcano[, "log2FC"] < -log2(foldchange), "Down", volcano[, "factor"]);
volcano[, "factor"]  = ifelse(volcano[, "p.value"] < 0.05, volcano[, "factor"], "Not Sig");
volcano[, "p.value"] = -log10(volcano[, "p.value"]);

print("peeks of the raw data:");
print(head(volcano));
print("count of the factors:");
print(`Up:      ${sum("Up"      == volcano[, "factor"])}`);
print(`Not Sig: ${sum("Not Sig" == volcano[, "factor"])}`);
print(`Down:    ${sum("Down"    == volcano[, "factor"])}`);

bitmap(file = `${@dir}/volcano.png`, size = [3000, 3000]) {
	ggplot(volcano, aes(x = "log2FC", y = "p.value"), padding = "padding:250px 500px 250px 300px;")
	    + geom_point(aes(color = "factor"), color = "black", shape = "circle", size = 25)
       + scale_colour_manual(values = list(
          Up        = "red",
          "Not Sig" = "gray",
          Down      = "steelblue"
       ))
       + geom_text(aes(label = "ID"), which = ~(factor != "Not Sig") && (p.value >= 15) )
       + geom_hline(yintercept = -log10(0.05),      color = "red", line.width = 5, linetype = "dash")
       + geom_vline(xintercept =  log2(foldchange), color = "red", line.width = 5, linetype = "dash")
       + geom_vline(xintercept = -log2(foldchange), color = "red", line.width = 5, linetype = "dash")
       + labs(x = "log2(FoldChange)", y = "-log10(P-value)")
       + ggtitle("Volcano Plot (A vs B)")
       + scale_x_continuous(labels = "F2")
       + scale_y_continuous(labels = "F2")
	;
}