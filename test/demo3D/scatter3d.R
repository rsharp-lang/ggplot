require(ggplot);

options(strict = FALSE);

bitmap(file = `${@dir}/UMAP3d.png`, size = [2400, 2000]) {
    
	data = read.csv(`${@dir}/UMAP3D.csv`, row.names = 1);
	data[, "class"] = `class_${data[, "class"]}`;
	
	# create ggplot layers and tweaks via ggplot style options
	ggplot(data, aes(x = "X", y = "Y", z = "Z"), padding = "padding:250px 500px 100px 100px;")
	+ geom_point(aes(color = "class"), color = "paper", shape = "triangle", size = 20)
	+ view_camera(angle = [31.5,65,125], fov = 100000)
	+ ggtitle("Scatter UMAP 3D")
	+ theme_black()
	;

}