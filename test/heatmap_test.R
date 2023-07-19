library(ggplot2);

spinrates <- read.csv("../data/spinrates.csv",
                      stringsAsFactors = FALSE)

bitmap( file = "./test_heatmap.bmp");

p <- ggplot(spinrates, aes(x=velocity, y=spinrate)) +
  geom_tile(aes(fill = swing_miss)) +
  scale_fill_distiller(palette = "YlGnBu", direction = 1) +
  theme_light() +
  labs(title = "Likelihood of swinging and missing on a fastball",
       y = "spin rate (rpm)")

plot(p);

dev.off();