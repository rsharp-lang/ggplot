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

bitmap(file = `${@dir}/scatter_raw.png`) {
	ggplot(data.frame(x, y, text = texts), aes(x = "x", y = "y")) 
	+ geom_point()
	+ geom_text(aes(label = "text"))
	;

}
