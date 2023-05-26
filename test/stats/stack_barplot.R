imports "package_utils" from "devkit";

package_utils::attach(`${@dir}/../../`);

require(ggplot);

bezdekIris = read.csv(system.file("data/bezdekIris.csv", package = "REnv"), row.names = NULL, check.names = FALSE);
# bezdekIris[, "class"] = make.names(bezdekIris$class, unique = TRUE);

print(bezdekIris);

let mydata = melt(bezdekIris, id.vars = "class");

print(mydata);

bitmap( file = `${@dir}/stack_bar.png`) {
    ggplot(data=mydata,aes("variable","value",fill="class"),y.axis.layout = "none", x.axis.layout = "none")
    + geom_bar(stat="identity",position="stack", color="black", width=0.7,size=0.25, show.legend = FALSE)
    + scale_fill_manual(values=c("red", "blue", "green","darkgreen","black"))
    + labs(x = "",y = "Nutrient (mg/L)")
    + coord_flip()
}