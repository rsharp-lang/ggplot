require(ggplot);

let data = data.frame(
    value = runif(1000, 4, 8) |> append(rexp(1000,1)) |> append(rexp(1000,2)) |> append(-rexp(1000,1)),
    serials = rep(["middle","right","right-2","left"], each = 1000)
);

ggplot(data, aes(x = "value", fill = "serials")) + 
    geom_histogram(position="identity", alpha=0.8, binwidth=0.1) +
    labs(title="Multiple Series Distribution", x="Value", y="Frequency");

ggsave(filename = file.path(@dir,"hist1.png"), width = 800,height = 600);
ggsave(filename = file.path(@dir,"hist1.svg"), width = 800,height = 600);
ggsave(filename = file.path(@dir,"hist1.pdf"), width = 800,height = 600);