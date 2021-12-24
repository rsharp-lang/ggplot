require(ggplot);

x = seq(-5,5, by = 0.2);
y = sin(x);

bitmap(file = `${@dir}/line_sin.png`, width = 2400, height = 1600) {
    ggplot(data.frame(x = x, y =y), aes(x = "x", y = "y"), padding = "padding: 200px 500px 200px 200px;") 
    + geom_line(width = 8, show.legend = TRUE, color = "Jet")
    ;
}