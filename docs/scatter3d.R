#' # 3D charting in ggplot
#'
#' Just load ggplot package via package handler function:
#'
#' ```r
#' library(ggplot);
#' require(ggplot);
#' ```
;

#region "load package/config"
require(ggplot);

# config for R# script engine
# disable strict mode will enable programming in R#
# script use a symbol without declared at first explicitly 
options(strict = FALSE);
#end region
;

#' Almost all of the data source for ggplot chartting
#' is using the data.frame.
#' we get the charting data source from a demo data file
#' in ggplot package.
#'

#region "read dataset"
data = system.file("data/UMAP3D.csv", package = "ggplot");
data = read.csv(data, row.names = 1);
data[, "class"] = `class_${data[, "class"]}`;

# take a peeks on what data that we've loaded
# at here
print(head(data));
#end region
;

#' rendering a 3d chart in ggplot package is just simply 
#' enough as create a 2d chart plot. we just needs add
#' a data mapping of the z axis at here!
#' 

#region "ggplot 3d"
# create ggplot layers and tweaks via ggplot style options
ggplot(data, aes(x = "X", y = "Y", z = "Z"), padding = "padding:250px 500px 100px 100px;")
   
   # use scatter points for visual our data
   + geom_point(aes(color = "class"), color = "paper", shape = "triangle", size = 20)   
   + ggtitle("Scatter UMAP 3D")
   # use the default white theme from ggplot
   + theme_default()

   # use a 3d camera to rotate the charting plot 
   # and adjust view distance
   + view_camera(angle = [31.5,65,125], fov = 100000)
   ;
   
#end region
;