require(ggplot);

let x = seq(-5,5, by = 0.2);
let y = sin(x);
let line_data = data.frame(x, y);

print(line_data, max.print = 13);

let plot_line = function() {
    ggplot(line_data, aes(x = "x", y = "y"), padding = "padding: 200px 500px 200px 200px;")
    + geom_line(width = 8, show.legend = TRUE, color = "Jet")
    + theme(axis.text = element_text(family = "Microsoft YaHei",
                                 size = 16,
                                 color = "red"),
            axis.title = element_text(family = "Microsoft YaHei",
                                 size = 24,
                                 color = "red")
        )
    ;
}

let do_plot = function() {
    let w = 2400;
    let h = 1600;

    bitmap(file = file.path(@dir,  "line_sin.png"), width = w, height = h) {
        plot_line();
    }

    svg(file = file.path (@dir,  "line_sin.svg"), width = w, height = h) {
        plot_line();
    }
}

do_plot();