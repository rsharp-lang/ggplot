require(ggplot);

data <- data.frame("category" = c('A', 'B', 'C', 'D'),
                   "amount" = c(25, 40, 27, 8));
				   
bitmap(file = `${@dir}/pie.png`) {

	ggplot(data, aes(x="category", y="amount", fill=category)) + geom_pie();

}