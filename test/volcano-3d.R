require(ggplot);

const volcano    = read.csv(`${@dir}/log2FC.csv`);
const foldchange = 1.5;

# create color factor for scatter points
volcano[, "factor"]  = ifelse(volcano[, "log2FC"] >  log2(foldchange), "Up", "Not Sig");
volcano[, "factor"]  = ifelse(volcano[, "log2FC"] < -log2(foldchange), "Down", volcano[, "factor"]);
volcano[, "factor"]  = ifelse(volcano[, "p.value"] < 0.05, volcano[, "factor"], "Not Sig");

# transform of the pvalue scale
volcano[, "p.value"] = -log10(volcano[, "p.value"]);
volcano[, "impact"]  = volcano[, "p.value"] * abs(volcano[, "log2FC"]);

print("peeks of the raw data:");
print(head(volcano));
print("count of the factors:");
print(`Up:      ${sum("Up"      == volcano[, "factor"])}`);
print(`Not Sig: ${sum("Not Sig" == volcano[, "factor"])}`);
print(`Down:    ${sum("Down"    == volcano[, "factor"])}`);

bitmap(file = `${@dir}/volcano-3d.png`, size = [3000, 3000]) {
    # create ggplot layers and tweaks via ggplot style options
	ggplot(volcano, aes(x = "log2FC", y = "impact", z = "p.value"), padding = "padding:250px 500px 250px 300px;")
	+ geom_point(aes(color = "factor"), color = "black", shape = "circle", size = 25)
	 + scale_colour_manual(values = list(
          Up        = "red",
          "Not Sig" = "gray",
          Down      = "steelblue"
       ))
}