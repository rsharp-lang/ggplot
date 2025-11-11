require(ggplot);

data(UMAP3D);

UMAP3D[,"class"] = `number_${UMAP3D$class}`;

print(UMAP3D, max.print = 20);

bitmap(file = relative_work("pc_test.png"), size = []) {
    ggplot(UMAP3D, aes(x = "X", y = "Y",color = "class")) +
        geom_point(aes(color = "class"),alpha = 0.3, color = "paper", size = 24) +
        geom_princurve(color = "paper", size = 1) +
        labs(title = "Scatter Plot with Principal Curve",
            x = "X", y = "Y")
}
