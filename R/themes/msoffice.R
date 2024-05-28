#' Excel liked theme style for ggplot
#' 
const msoffice = function(accents = ["#44546a" "#e7e6e6" "#5b9bd5" "#ed7d31" "#a5a5a5" 
                                     "#ffc000" "#4472c4" "#70ad47"]) {
    ggplot2::theme(
        text = element_text(color = "white"),
        plot.background = "white",
        panel.background = "white",
        panel.grid = "stroke: grey; stroke-width: 3px; stroke-dash: dash;",
        axis.line = "stroke: grey; stroke-width: 6px; stroke-dash: solid;",
        axis.text = element_text(color = "grey"),
        legend.text = element_text(color = "grey", size = 12)
    );    
}