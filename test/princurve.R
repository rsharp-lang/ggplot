require(ggplot);

let n <- 200;
let t <- seq(0, 4 * PI, length.out = n);

str(t);
str(rnorm(n, sd = 0.1));
str(cos(t));
str(sin(t));

let x <- (t + 1) * cos(t) + rnorm(n, sd = 0.1);
let y <- (t + 1) * sin(t) + rnorm(n, sd = 0.1);
let data <- data.frame(x = x, y = y);

bitmap(file = relative_work("pc_test.png")) {
    ggplot(data, aes(x = "x", y = "y")) +
        geom_point(alpha = 0.6, color = "steelblue", size = 12) +
        geom_princurve(color = "red", size = 1.2) +
        labs(title = "Scatter Plot with Principal Curve",
            x = "X", y = "Y")
}
