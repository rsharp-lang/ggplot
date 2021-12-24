require(ggplot);

x = -5:5 step 0.1;
y = sin(x);

bitmap(file = `${@dir}/line_sin.png`, width = 2100, height = 1600) {
    ggplot(data.frame(x = x, y =y), aes(x = "x", y = "y")) 
    + geom_line()
    ;
}