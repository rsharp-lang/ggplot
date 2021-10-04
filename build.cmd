@echo off

SET R_HOME=../../R-sharp/App

"%R_HOME%/Rscript.exe" --build /save ./ggplot.zip
"%R_HOME%/R#.exe" --install.packages ./ggplot.zip

pause