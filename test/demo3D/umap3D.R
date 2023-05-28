imports ["dataset", "umap"] from "MLkit";

options(strict = FALSE);

const filename as string = "F:\GCModeller\src\R-sharp\test\demo\machineLearning\umap\MNIST-LabelledVectorArray-60000x100.msgpack";
const MNIST_LabelledVectorArray = filename
|> read.mnist.labelledvector(takes = 50000)
;
const tags as string = rownames(MNIST_LabelledVectorArray);

rownames(MNIST_LabelledVectorArray) = `X${1:nrow(MNIST_LabelledVectorArray)}`;

const manifold = umap(MNIST_LabelledVectorArray,
	dimension         = 3, 
	numberOfNeighbors = 15,
	localConnectivity = 1,
	KnnIter           = 64,
	bandwidth         = 1,
	debug             = TRUE,
	KDsearch          = FALSE,
	spectral_cos      = TRUE
)
;

manifold$umap
|> as.data.frame(dimension = ["X", "Y", "Z"])
|> write.csv( 
	file      = `${@dir}/UMAP3D.csv`, 
	row.names = tags
);