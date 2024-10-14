REM @echo off

SET drive=%~d0
SET R_HOME=%drive%/GCModeller\src\R-sharp\App\net8.0
SET pkg=./ggplot.zip
SET js_url="https://rdocumentation.rsharp.net/assets/R_syntax.js"

%R_HOME%/Rscript.exe --build /src ../ /save %pkg% --skip-src-build  --github-page %js_url%
%R_HOME%/R#.exe --install.packages %pkg%

pause