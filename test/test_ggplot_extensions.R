require(ggplot);
options(strict = FALSE);
setwd(@dir);

set.seed(1234);
let n = 120;
let grp = rep(c("A", "B", "C"), each = n / 3);
let val = c(rnorm(n / 3, 10, 2), rnorm(n / 3, 13, 3), rnorm(n / 3, 9, 1.5));
let df <- data.frame(x = 1:length(val), y = val, g = grp);

# 1. geom_area + geom_path
bitmap(file = "./verify_geom_area.png", size = [1600, 1200]) {
    ggplot(df, aes(x = "x", y = "y"), padding = "padding:120px 120px 120px 160px;")
      + geom_area(alpha = 0.4, color = "steelblue")
      + labs(x = "index", y = "value")
      + ggtitle("geom_area")
    ;
}

# 2. geom_density
bitmap(file = "./verify_geom_density.png", size = [1600, 1200]) {
    ggplot(df, aes(x = "y", color = "g"), padding = "padding:120px 120px 120px 160px;")
      + geom_density(bw = "silverman", outline = TRUE)
      + labs(x = "value", y = "density")
      + ggtitle("geom_density")
    ;
}

# 3. geom_freqpoly + position_fill
bitmap(file = "./verify_geom_freqpoly.png", size = [1600, 1200]) {
    ggplot(df, aes(x = "g", y = "y"), padding = "padding:120px 120px 120px 160px;")
      + geom_bar(position = position_fill(), color = "steelblue")
      + labs(x = "group", y = "value")
      + ggtitle("geom_bar + position_fill")
    ;
}

# 4. position_dodge + stat_compare_means
bitmap(file = "./verify_position_dodge.png", size = [1600, 1200]) {
    ggplot(df, aes(x = "g", y = "y", color = "g"), padding = "padding:140px 120px 120px 160px;")
      + geom_boxplot()
      + stat_compare_means(method = "t.test")
      + labs(x = "group", y = "value")
      + ggtitle("geom_boxplot + stat_compare_means")
    ;
}

# 5. geom_smooth (lm + loess) with the confidence band
bitmap(file = "./verify_geom_smooth.png", size = [1600, 1200]) {
    ggplot(df, aes(x = "x", y = "y"), padding = "padding:120px 120px 120px 160px;")
      + geom_point(color = "black", size = 8)
      + geom_smooth(method = "lm", se = TRUE, color = "red")
      + labs(x = "index", y = "value")
      + ggtitle("geom_smooth(lm)")
    ;
}

bitmap(file = "./verify_geom_smooth_loess.png", size = [1600, 1200]) {
    ggplot(df, aes(x = "x", y = "y"), padding = "padding:120px 120px 120px 160px;")
      + geom_point(color = "black", size = 8)
      + geom_smooth(method = "loess", span = 0.4, se = FALSE, color = "blue")
      + labs(x = "index", y = "value")
      + ggtitle("geom_smooth(loess)")
    ;
}

# 6. geom_dotplot
bitmap(file = "./verify_geom_dotplot.png", size = [1600, 1200]) {
    ggplot(df, aes(x = "g", y = "y", color = "g"), padding = "padding:120px 120px 120px 160px;")
      + geom_dotplot(dotsize = 5, stackdir = "center")
      + labs(x = "group", y = "value")
      + ggtitle("geom_dotplot")
    ;
}

# 7. coord_cartesian + coord_fixed
bitmap(file = "./verify_coord_cartesian.png", size = [1600, 1200]) {
    ggplot(df, aes(x = "x", y = "y"), padding = "padding:120px 120px 120px 160px;")
      + geom_point(color = "steelblue", size = 12)
      + coord_cartesian(xlim = c(20, 90))
      + labs(x = "index", y = "value")
      + ggtitle("coord_cartesian")
    ;
}

# 8. guides
bitmap(file = "./verify_guides.png", size = [1600, 1200]) {
    ggplot(df, aes(x = "g", y = "y", color = "g"), padding = "padding:120px 200px 120px 160px;")
      + geom_boxplot()
      + guides("legend")
      + labs(x = "group", y = "value")
      + ggtitle("guides(guide_legend)")
    ;
}

cat("test_ggplot_extensions done!");