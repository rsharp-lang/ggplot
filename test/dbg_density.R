require(ggplot);
options(strict = FALSE);
setwd(@dir);
set.seed(1);
let df <- data.frame(x = 1:20, y = c(rnorm(20, 5, 1)));
print(df);
bitmap(file = "./dbg1.png", size = [800, 600]) {
    ggplot(df, aes(x = "y"), padding = "padding:100px;") + geom_density() ;
}
cat("dbg done");