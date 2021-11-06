require(ggplot);

bitmap(file = `${@dir}/UMAP3d.png`, size = [3000, 3000]) {
    # create ggplot layers and tweaks via ggplot style options
	ggplot(read.csv(`${@dir}/UMAP3D.csv`, row.names = 1), aes(x = "X", y = "Y", z = "Z"), padding = "padding:50px 50px 50px 50px;")
	+ geom_point(aes(color = "class"), color = "Set1", shape = "circle", size = 25)
	+ view_camera(angle = [32,65,125], fov = 100000)
	;

}