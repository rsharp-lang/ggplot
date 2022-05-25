require(ggplot);

texts = ["Coca_cola", "IBM", "Microsoft", "Google", "General_Electric",
			"Mc_Donalds", "Intel", "Nokia", "Disney", "HP", "Toyota",
			"Mercedes-benz", "Gilette", "Cisco", "BMW", "Apple", "Marlboro",
			"Samsung", "Honda", "H&M", "Oracle", "Pepsi", "Nike", "SAP",
			"Nescafe", "IKEA", "Jp_Morgan", "Budweiser", "UPS", "HSBC", "Canon",
			"Sony", "Kellog's", "Amazon", "Goldman_Sachs", "Nintendo", "DELL",
			"Ebay", "Gucci", "LVMH", "Heinz", "Zara", "Siemens", "Netflix",
			"Louis_Vuitton", "Channel", "Facebook", "Tesla", "Spotify",
			"Porsche", "Starbucks", "UBER", "KFC", "Linkedin"];
			
x = runif(length(texts), -10, 10);
y = runif(length(texts), -10, 10);

bitmap(file = `${@dir}/scatter_raw.png`, size = [3000, 2100]) {
	ggplot(data.frame(x, y, text = texts), aes(x = "x", y = "y"), padding = "padding: 200px 400px 250px 300px;") 
	+ geom_point(size = 30, color = "red")
	+ geom_text(aes(label = "text"), check_overlap = FALSE)
	;
}

bitmap(file = `${@dir}/scatter_nudge.png`, size = [3000, 2100]) {
	ggplot(data.frame(x, y, text = texts), aes(x = "x", y = "y"), padding = "padding: 200px 400px 250px 300px;") 
	+ geom_point(size = 30, color = "red")
	+ geom_text(aes(label = "text"), check_overlap = TRUE )
	;
}