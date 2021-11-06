
#' The black theme for ggplot
#' 
const theme_black as function() {
    ggplot2::theme(
        text = element_text(color = "white"),
        plot.background = "black",
        panel.background = "black",
        panel.grid = "stroke: white; stroke-width: 3px; stroke-dash: dash;"
    );
}