import ggplot

x = seq(-5, 5, by = 0.2)
y = sin(x)
input = data.frame(x = x, y = y)

def plotfile(filepath):
   print("previews of the plot table:")
   print(input, max.print = 13)
   
   plt = ggplot(input, aes(x = "x", y = "y"), padding = "padding: 200px 500px 200px 200px;", width = 2400, height = 1600) 
   plt = plt + geom_line(width = 8, show.legend = TRUE, color = "Jet")

   bitmap(plt, file = filepath)
   
   
plotfile(`${@dir}/line_sin_py.png`)