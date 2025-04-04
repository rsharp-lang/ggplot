const theme_void = function() {
    theme(
        panel.grid = element_blank(), 
        axis.line = element_blank(),
        axis.ticks = element_blank(text_blank=TRUE),
        axis.text = element_blank(text_blank=TRUE),
        axis.title = element_blank(text_blank=TRUE),
        legend.position = "none"
    );
} 