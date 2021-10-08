const volcano = read.csv(`${@dir}/log2FC.csv`);

print("peeks of the raw data:");
print(head(volcano));

bitmap(file = `${@dir}/volcano.png`, size = [2400, 3600]) {
		
	ggplot(volcano, aes(x = "log2FC", y = "p.value"), padding = "padding:150px 100px 200px 250px;")
	;
}