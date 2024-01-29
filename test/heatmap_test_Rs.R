require(ggplot);

setwd(@dir);
spinrates <- read.csv("../data/spinrates.csv",
                      stringsAsFactors = FALSE);

let ggplot2 = function() {

p <- ggplot(spinrates, aes(x = "velocity", y = "spinrate")) +
  geom_tile(aes(fill = "swing_miss")) +
  scale_fill_distiller(palette = "YlGnBu", direction = 1) +
  theme_light() +
  labs(title = "Likelihood of swinging and missing on a fastball",
       y = "spin rate (rpm)");

      p;
}

png(filename = "./test_heatmap.png");
plot(ggplot2());
dev.off();

svg(file = "./test_heatmap.svg");
plot(ggplot2());
dev.off();