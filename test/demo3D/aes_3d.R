require(ggplot);

options(strict = FALSE);

bitmap(file = `${@dir}/aes_3d.png`, size = [2400, 2000]) {
    
	data = read.csv(`${@dir}/UMAP3D.csv`, row.names = 1);
	data[, "class"] = `class_${data[, "class"]}`;
	
	# create ggplot layers and tweaks via ggplot style options
	ggplot(data, aes(x = "X", y = "Y", z = "Z"), padding = "padding:50px 50px 50px 50px;");

}