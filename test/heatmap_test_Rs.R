require(ggplot);

setwd(@dir);
spinrates <- read.csv("../data/spinrates.csv",
                      stringsAsFactors = FALSE);

spinrates[, "swing_miss"] = as.numeric(spinrates$swing_miss);

print(spinrates);

let ggplot2 = function() {

p <- ggplot(spinrates, aes(x = "velocity", y = "spinrate"),padding = "padding:250px 400px 250px 300px;") +
  geom_tile(aes(fill = "swing_miss")) +
  scale_fill_distiller(palette = "YlGnBu", direction = 1) +
  theme_light() +
  labs(title = "Likelihood of swinging and missing on a fastball",
       y = "spin rate (rpm)");

      p;
}

png(filename = "./test_heatmap.png",  width = 2000, height = 1200);
plot(ggplot2());
dev.off();

svg(file = "./test_heatmap.svg",  width = 2000, height = 1200);
plot(ggplot2());
dev.off();