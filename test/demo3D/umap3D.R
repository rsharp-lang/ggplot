imports ["dataset", "umap"] from "MLkit";

options(strict = FALSE);

const filename as string = "E:\GCModeller\src\R-sharp\Library\demo\machineLearning\umap\MNIST-LabelledVectorArray-60000x100.msgpack";
const MNIST_LabelledVectorArray = filename
|> read.mnist.labelledvector(takes = 5000)
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
	KDsearch          = FALSE
)
;

manifold$umap
|> as.data.frame(dimension = ["X", "Y", "Z"])
|> write.csv( 
	file      = `${@dir}/UMAP3D.csv`, 
	row.names = tags
);