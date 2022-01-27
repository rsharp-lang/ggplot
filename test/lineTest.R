require(ggplot);

setwd(@dir);

x = seq(-5,5, by = 0.2);
y = sin(x);
line_data = data.frame(x, y);

print(line_data, max.print = 13);

bitmap(file = "./line_sin.png", width = 2400, height = 1600) {
    ggplot(line_data, aes(x = "x", y = "y"), padding = "padding: 200px 500px 200px 200px;")
    + geom_line(width = 8, show.legend = TRUE, color = "Jet")
    ;
}