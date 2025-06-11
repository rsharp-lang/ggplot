require(ggplot);
require(Matrix);

# load data
data(volcano);

volcano = as.matrix(volcano);
volcano = melt(volcano, varnames = c("X", "Y"), value.name = "Height");

print(volcano, max.print = 13);

let ggplot2 = function() {
  p <- ggplot(volcano, aes(x = "X", y = "Y"),padding = "padding:10% 10% 10% 12%;") +
     geom_tile(aes(fill = "Height")) +
     scale_fill_distiller(palette = "rev(ggthemes::excel_Vapor_Trail)", direction = 1) +
     theme_light(
        axis.text = element_text(family = "Cambria Math", size = 20),
        axis.title = element_text(family = "Cambria Math", size = 36),
        legend.tick = element_text(family = "Cambria Math", size = 18)
     ) +
     labs(title = "Volcano Height Map");
  p;
}

png(filename = relative_work("volcano.png"),  width = 2000, height = 1200);
plot(ggplot2());
dev.off();