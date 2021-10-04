# require(ggplot2);
imports "ggplot2" from "ggplot";

const chic <- read.csv(`${@dir}/chicago-nmmaps.csv`);

print(head(chic, 10));

bitmap(file = `${@dir}/demo.png`, size = [2400, 1600]) {

    ggplot(chic, aes(x = "time", y = "temp"), padding = "padding:150px 100px 200px 250px;") + 
        geom_point(color = "steelblue", shape = "Triangle", size = 21) +
        labs(x = "Time", y = "Temperature (°F)") + 
        ggtitle("Temperatures in Chicago") + 
		scale_x_continuous(labels = "F0") + 
		scale_y_continuous(labels = "F0")
    ;
}

pause();