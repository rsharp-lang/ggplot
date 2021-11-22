require(ggplot);

options(strict = FALSE);

data = read.csv(`${@dir}/GCxGC.csv`, row.names = NULL, check.names = FALSE);

print(head(data));

bitmap(file = `${@dir}/scatter3D.png`) {
	ggplot(data, aes(x = "1st Dimension Time (minutes)", y = "2nd Dimension Time (s)", z = "Area")) 
	+ geom_point()
	+ theme_default()

   # use a 3d camera to rotate the charting plot 
   # and adjust view distance
   + view_camera(angle = [31.5,65,125], fov = 100000)
	;
}