require(ggplot);

setwd(@dir);

myeloma = read.csv("../../data/myeloma.csv", row.names = 1);

i = myeloma$molecular_group;
i = (i == "MAF") || (i == "Proliferation") || (i == "Cyclin D-1");

print(i);

myeloma = myeloma[i, ];

print(myeloma, max.print = 13);
print("myeloma$DEPDC1");
print(myeloma$DEPDC1);

bitmap(file = "./myeloma_box_ttest.png", size = [1800,2400]) {
	
	ggplot(myeloma, aes(x = "molecular_group", y = "DEPDC1"))
	+ geom_hline(yintercept = mean(myeloma$DEPDC1), linetype="dash", line.width = 6, color = "red")# Add horizontal line at base mean 
	+ geom_boxplot(width = 0.65)
	+ geom_jitter(width = 0.3)	
	+ ggtitle("DEPDC1 ~ molecular_group")
	+ ylab("DEPDC1")
	+ xlab("")
	+ scale_y_continuous(labels = "F0")
	+ stat_compare_means(method = "anova", label.y = 1600) # Add global annova p-value 
    # + stat_compare_means(label = "p.signif", method = "t.test", ref.group = ".all.", hide.ns = TRUE)# Pairwise comparison against all
	+ geom_signif(list(		
v4 = ["MAF","Proliferation"]
	))	
	+ theme(axis.text.x = element_text(angle = 45), plot.title = element_text(family = "Cambria Math", size = 16))
	;

}