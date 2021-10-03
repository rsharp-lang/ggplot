# require(ggplot2);
imports "ggplot2" from "ggplot";

const chic <- read.csv(`${@dir}/chicago-nmmaps.csv`);

print(head(chic, 10));

const g <- ggplot(chic, aes(x = date, y = temp));

pause();