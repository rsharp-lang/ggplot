require(ggpubr);
options(strict = FALSE);
setwd(@dir);

let dummy <- data.frame(x = c(0, 1), y = c(0, 1));
set.seed(4321);
let df <- data.frame(
    g = rep(c("control", "treatA", "treatB"), each = 40),
    v = c(rnorm(40, 5, 1.2), rnorm(40, 6.5, 1.4), rnorm(40, 7.2, 1.1))
);

# 1. ggboxplot
bitmap(file = "./verify_ggboxplot.png", size = [1600, 1200]) {
    ggplot(df, aes(x = "g", y = "v", color = "g"), padding = "padding:140px 160px 120px 160px;")
      + ggboxplot(width = 0.5)
      + stat_compare_means()
      + labs(x = "group", y = "value")
      + ggtitle("ggboxplot + significance")
    ;
}

# 2. ggviolin
bitmap(file = "./verify_ggviolin.png", size = [1600, 1200]) {
    ggplot(df, aes(x = "g", y = "v", color = "g"), padding = "padding:120px 160px 120px 160px;")
      + ggviolin()
      + labs(x = "group", y = "value")
      + ggtitle("ggviolin")
    ;
}

# 3. ggbeeswarm
bitmap(file = "./verify_ggbeeswarm.png", size = [1600, 1200]) {
    ggplot(df, aes(x = "g", y = "v", color = "g"), padding = "padding:120px 160px 120px 160px;")
      + ggbeeswarm(size = 8)
      + labs(x = "group", y = "value")
      + ggtitle("ggbeeswarm")
    ;
}

# 4. ggerrorbar (mean_se)
bitmap(file = "./verify_ggerrorbar.png", size = [1600, 1200]) {
    ggplot(df, aes(x = "g", y = "v", color = "g"), padding = "padding:120px 160px 120px 160px;")
      + ggerrorbar(errorType = "mean_se", width = 0.3)
      + labs(x = "group", y = "value")
      + ggtitle("ggerrorbar(mean_se)")
    ;
}

# 5. ggpaired
bitmap(file = "./verify_ggpaired.png", size = [1600, 1200]) {
    ggplot(df, aes(x = "g", y = "v"), padding = "padding:120px 160px 120px 160px;")
      + ggpaired()
      + labs(x = "timepoint", y = "value")
      + ggtitle("ggpaired")
    ;
}

# 6. ggcorr
bitmap(file = "./verify_ggcorr.png", size = [1200, 1200]) {
    ggplot(dummy, aes(x = "x", y = "y"), padding = "padding:60px;")
      + ggcorr(df)
      + ggtitle("ggcorr")
    ;
}

# 7. ggforest
bitmap(file = "./verify_ggforest.png", size = [1600, 1200]) {
    ggplot(dummy, aes(x = "x", y = "y"), padding = "padding:120px 400px 120px 160px;")
      + ggforest(
            effect = c(0.35, 0.62, 0.28, 0.81),
            lower = c(0.10, 0.40, 0.05, 0.60),
            upper = c(0.60, 0.84, 0.51, 1.02),
            label = c("studyA", "studyB", "studyC", "studyD")
        )
      + ggtitle("ggforest")
    ;
}

# 8. funnel
bitmap(file = "./verify_funnel.png", size = [1400, 1200]) {
    ggplot(dummy, aes(x = "x", y = "y"), padding = "padding:120px 200px 120px 200px;")
      + funnel(
            proportion = c(1000, 620, 380, 210, 96),
            label = c("screened", "eligible", "included", "analysed", "final")
        )
      + ggtitle("funnel")
    ;
}

# 9. ggroc
bitmap(file = "./verify_ggroc.png", size = [1400, 1200]) {
    set.seed(99);
    let score = c(rnorm(60, 0.2, 1), rnorm(60, 2.4, 1));
    let truth = c(rep(FALSE, 60), rep(TRUE, 60));
    ggplot(dummy, aes(x = "x", y = "y"), padding = "padding:120px 200px 120px 160px;")
      + ggroc(score, truth)
      + ggtitle("ggroc")
    ;
}

cat("test_ggpubr done!");