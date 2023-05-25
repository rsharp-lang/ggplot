require(ggplot);

bezdekIris = read.csv(system.file("data/bezdekIris.csv", package = "REnv"), row.names = NULL, check.names = FALSE);

print(bezdekIris);

let mydata = melt(bezdekIris, id.vars = "Name");

print(mydata);