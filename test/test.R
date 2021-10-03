# require(ggplot2);
imports "ggplot2" from "ggplot";

const chic <- read.csv(`${@dir}/chicago-nmmaps.csv`);

print(head(chic, 10));

bitmap(file = `${@dir}/demo.png`, size = [2400, 1600]) {
    ggplot(chic, aes(x = "date", y = "temp"));
}

pause();