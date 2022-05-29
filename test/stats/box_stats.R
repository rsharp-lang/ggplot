require(ggplot);

setwd(@dir);

myeloma = read.csv("../../data/myeloma.csv", row.names = 1);

print(myeloma, max.print = 13);
print("myeloma$DEPDC1");
print(myeloma$DEPDC1);

bitmap(file = "./myeloma_box.png") {
	
	ggplot(myeloma, aes(x = "molecular_group", y = "DEPDC1"))
	+ geom_boxplot(width = 0.8)
	+ geom_jitter(width = 0.3)
	+ geom_hline(yintercept = mean(myeloma$DEPDC1), linetype=2)# Add horizontal line at base mean 
	+ ggtitle(name)
	+ ylab("DEPDC1")
	+ xlab("")
	+ scale_y_continuous(labels = "G2")
	+ stat_compare_means(method = "anova", label.y = 1600) # Add global annova p-value 
    + stat_compare_means(label = "p.signif", method = "t.test", ref.group = ".all.", hide.ns = TRUE)# Pairwise comparison against all
	+ theme(axis.text.x = element_text(angle = 45))
	;

}