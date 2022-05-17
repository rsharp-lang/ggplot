require(ggplot);

setwd(@dir);

# 	mean_region_3	sd_region_3	mean_region_4	sd_region_4	Fold Change	log2(FC)	p.value	FDR	VIP	pcorr

const volcano    = read.csv("./pvalue.csv", row.names = 1, check.names = FALSE);
const foldchange = 1.5;

# create color factor for scatter points
volcano[, "factor"]  = ifelse(volcano[, "log2(FC)"] >  log2(foldchange), "Up", "Not Sig");
volcano[, "factor"]  = ifelse(volcano[, "log2(FC)"] < -log2(foldchange), "Down", volcano[, "factor"]);
volcano[, "factor"]  = ifelse(volcano[, "p.value"] < 0.05, volcano[, "factor"], "Not Sig");

# transform of the pvalue scale
volcano[, "p.value"] = -log10(volcano[, "p.value"]);
volcano[, "ID"] = rownames(volcano);

print("peeks of the raw data:");
print(head(volcano));
print("count of the factors:");
print(`Up:      ${sum("Up"      == volcano[, "factor"])}`);
print(`Not Sig: ${sum("Not Sig" == volcano[, "factor"])}`);
print(`Down:    ${sum("Down"    == volcano[, "factor"])}`);

# [1] "peeks of the raw data:"
#         ID        p.value   log2FC    factor
# <mode>  <string>  <double>  <double>  <string>
# [1, ]   "Q5XJ10"   9.81     -0.205    "Not Sig"
# [2, ]   "A8WG05"   5.21      0.0472   "Not Sig"
# [3, ]   "Q8JH71"   11.6     -0.38     "Not Sig"
# [4, ]   "Q7T3L3"   8.85      0.165    "Not Sig"
# [5, ]   "Q567C8"   21        0.837    "Up"
# [6, ]   "Q92005"   0.091    -0.00514  "Not Sig"
# 
# [1] "count of the factors:"
# [1]     "Up:      90"
# [1]     "Not Sig: 2643"
# [1]     "Down:    93"

bitmap(file = "./volcano.png", size = [2700, 1800]) {

   # create ggplot layers and tweaks via ggplot style options
	ggplot(volcano, aes(x = "log2(FC)", y = "p.value"), padding = "padding:250px 500px 250px 300px;")
	   + geom_point(aes(color = "factor"), color = "black", shape = "circle", size = 30,alpha = 0.3)
       + scale_colour_manual(values = list(
          Up        = "red",
          "Not Sig" = "gray",
          Down      = "steelblue"
       ), alpha = 0.3)
       + geom_text(aes(label = "ID"), which = ~(factor != "Not Sig") && (p.value >= 19.5) )
       + geom_hline(yintercept = -log10(0.05),      color = "red", line.width = 5, linetype = "dash")
       + geom_vline(xintercept =  log2(foldchange), color = "red", line.width = 5, linetype = "dash")
       + geom_vline(xintercept = -log2(foldchange), color = "red", line.width = 5, linetype = "dash")
       + labs(x = "log2(FoldChange)", y = "-log10(P-value)")
       + ggtitle("Volcano Plot (A vs B)")
       + scale_x_continuous(labels = "F2")
       + scale_y_continuous(labels = "F2")
	;
}