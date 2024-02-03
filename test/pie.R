require(ggplot);

let data <- data.frame("category" = c('A', 'B', 'C', 'D'),
                   "amount" = c(25, 40, 27, 8));

setwd(@dir);

print(data);

bitmap(file = "./pie.png") {
	ggplot(data, aes(x="category", y="amount", fill=category)) + geom_pie();
}

svg(file = "./pie.svg") {
	ggplot(data, aes(x="category", y="amount", fill=category)) + geom_pie();
}