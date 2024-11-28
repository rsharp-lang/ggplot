require(ggplot);

# run this script for heatmap plot in morden R# language

setwd(@dir);
spinrates <- read.csv("../data/spinrates.csv",
                      stringsAsFactors = FALSE);

spinrates[, "swing_miss"] = as.numeric(spinrates$swing_miss);

print(spinrates);

let ggplot2 = function() {
  p <- ggplot(spinrates, aes(x = "velocity", y = "spinrate"),padding = "padding:10% 10% 10% 12%;") +
     geom_tile(aes(fill = "swing_miss")) +
     scale_fill_distiller(palette = "rev(ggthemes::excel_Vapor_Trail)", direction = 1) +
     theme_light(
        axis.text = element_text(family = "Cambria Math", size = 20),
        axis.title = element_text(family = "Cambria Math", size = 36),
        legend.tick = element_text(family = "Cambria Math", size = 18)
     ) +
     labs(title = "Likelihood of swinging and missing on a fastball",
          y = "spin rate (rpm)");
  p;
}

png(filename = "./test_heatmap.png",  width = 2000, height = 1200);
plot(ggplot2());
dev.off();

svg(file = "./test_heatmap.svg",  width = 1600, height = 800);
plot(ggplot2());
dev.off();