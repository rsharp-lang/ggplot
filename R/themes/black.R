
#' The black theme for ggplot
#' 
const theme_black as function() {
    ggplot2::theme(
        text = element_text(color = "white"),
        plot.background = "black",
        panel.background = "black",
        panel.grid = "stroke: white; stroke-width: 3px; stroke-dash: dash;",
        axis.line = "stroke: white; stroke-width: 6px; stroke-dash: solid;",
        axis.text = element_text(color = "white"),
        legend.text = element_text(color = "white", size = 12)
    );
}