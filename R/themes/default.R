
#' the default white theme for ggplot
#' 
const theme_default = function() {
    ggplot2::theme(
        plot.background = "white"
    );
}

const theme_light = function(axis.text = NULL, axis.title = NULL, legend.tick = NULL) {
    ggplot2::theme(
        axis_text = axis.text,
        axis_title = axis.title,
        legend_tick = legend.tick,
        plot.background = "white"
    );
}