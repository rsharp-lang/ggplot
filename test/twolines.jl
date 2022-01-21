using ggplot

x = seq(-5,5, by = 0.2)
y = sin(x)
z = cos(y)

table = data.frame(x = x, sin =y, cos = z)

print("preview of the input data:")
print(table, max.print = 13)

bitmap(plt, file = `${@dir}/line_3colors_julia.png`):

	plt = ggplot(table, padding = "padding: 225px 600px 300px 250px;") 
	plt += geom_line( aes(x = "x", y = "sin"), width = 8)
	plt += geom_line( aes(x = "x", y = "cos"), width = 8)
	plt += geom_line( aes(x = "x", y = "~sin + cos"), width = 8)
    plt += ggtitle("line overlaps")
    plt += xlab("time(ticks)") 
    plt += ylab("signal data")

    plot(plt, width = 2600, height = 1600)

end