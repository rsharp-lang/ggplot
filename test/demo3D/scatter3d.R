require(ggplot);

bitmap(file = `${@dir}/UMAP3d.png`, size = [3000, 3000]) {
    # create ggplot layers and tweaks via ggplot style options
	ggplot(read.csv(`${@dir}/UMAP3D.csv`), aes(x = "X", y = "Y", z = "Z"), padding = "padding:250px 500px 250px 300px;")
	+ geom_point(color = "black", shape = "circle", size = 25)
	+ view_camera(angle = [30,65,125])
	;

}