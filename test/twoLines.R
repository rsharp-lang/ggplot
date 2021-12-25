require(ggplot);

x = seq(-5,5, by = 0.2);
y = sin(x);
z = cos(y);

bitmap(file = `${@dir}/line_3colors.png`, width = 2400, height = 1600) {
    ggplot(data.frame(x = x, sin =y, cos = z), padding = "padding: 200px 500px 200px 200px;") 
    + geom_line( aes(x = "x", y = "sin"), width = 8)
	 + geom_line( aes(x = "x", y = "cos"), width = 8)
	  + geom_line( aes(x = "x", y = "~sin + cos"), width = 8)
    ;
}