imports ["dataset", "umap"] from "MLkit";

options(strict = FALSE);

raw = read.csv(`${@dir}/HR2MSI mouse urinary bladder S096_top3.csv`, check_modes = TRUE, check_names = TRUE, row_names = 1);

print(rownames(raw));
print(colnames(raw));

const manifold = raw
:> umap(
	dimension            = 3, 
	numberOfNeighbors    = 15,
    localConnectivity    = 1,
    KnnIter              = 64,
    bandwidth            = 1
)
;

manifold$umap
|> as.data.frame(
	labels = manifold$labels, 
	dimension = ["X", "Y", "Z"]
)
|> write.csv(file = `${@dir}/UMAP3D.csv`)
;