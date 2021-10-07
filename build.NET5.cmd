@echo off

SET R_HOME=../../R-sharp/App/net5.0

"%R_HOME%/Rscript.exe" --build /save ./ggplot.zip
"%R_HOME%/R#.exe" --install.packages ./ggplot.zip

pause