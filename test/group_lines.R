require(ggplot);

let data = read.csv("G:\GCModeller\src\runtime\ggplot\data\BCAAs.csv", row.names = 1, check.names = FALSE);

print(data);

setwd(@dir);

svg(file = "./multiple_group_line.svg", size = [1600,1080]) {
    ggplot(data, aes(x ="min", y="relative", group = "group", color = "group"))
    + geom_line();
}