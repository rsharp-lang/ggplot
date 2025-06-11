require(ggplot);
require(Matrix);

# load data
data(volcano);

volcano = as.matrix(volcano);
volcano = melt(volcano, varnames = c("X", "Y"), value_name = "Height");

print(volcano, max.print = 13);

let ggplot2 = function(colors) {
  p <- ggplot(volcano, aes(x = "X", y = "Y"),padding = "padding:10% 10% 10% 12%;") +
     geom_tile(aes(fill = "Height")) +
     scale_fill_distiller(palette = colors, direction = 1) +
     theme_light(
        axis.text = element_text(family = "Cambria Math", size = 20),
        axis.title = element_text(family = "Cambria Math", size = 36),
        legend.tick = element_text(family = "Cambria Math", size = 18)
     ) +
     labs(title = "Volcano Height Map");
  p;
}

for(let colorname in [
   # internal built-in color palette
   "jet" "hot" "cool" "grays" "autumn" "spring" "summer" "winter" 
   "FlexImaging" "Typhoon" "Icefire" "Seismic" "Rainbow" 
   "viridis" "viridis:inferno" "viridis:magma" "viridis:plasma" 
   "viridis:cividis" "viridis:mako" "viridis:rocket" "viridis:turbo"
   # paletteer imported
   # colorBlindness
   "colorBlindness::Green2Magenta16Steps" "colorBlindness::Brown2Blue12Steps" 
   "colorBlindness::Brown2Blue10Steps" "colorBlindness::Blue2OrangeRed14Steps" 
   "colorBlindness::Blue2Gray8Steps"
   "colorBlindness::Blue2DarkRed18Steps" "colorBlindness::Blue2Green14Steps" 
   "colorBlindness::paletteMartin" 
   "colorBlindness::ModifiedSpectralScheme11Steps" 
   "colorBlindness::PairedColor12Steps"
   # ggsci
   "ggsci::legacy_tron" "ggsci::default_gsea" "ggsci::uniform_startrek" 
   "ggsci::red_material" 
   "ggsci::pink_material" "ggsci::purple_material"
   # ggthemes
   "ggthemes::Jewel_Bright" "ggthemes::Summer" "ggthemes::Traffic" 
   "ggthemes::Hue_Circle" "ggthemes::Classic_Cyclic" 
   # ggthemes excel
   "ggthemes::excel_Atlas"  "ggthemes::excel_Vapor_Trail" "ggthemes::excel_Celestial" 
   "ggthemes::excel_Depth" 
   "ggthemes::excel_Facet" 
   "ggthemes::excel_Ion" "ggthemes::excel_Ion_Boardroom" "ggthemes::excel_Parallax"
   "ggthemes::excel_Slice" "ggthemes::excel_Savon" "ggthemes::excel_Aspect" 
   "ggthemes::excel_Green_Yellow" 
   "ggthemes::excel_Marquee" "ggthemes::excel_Slipstream"
   ]) {

   png(filename = relative_work(`volcano_${normalizeFileName(colorname,FALSE)}.png`), 
      width = 600, 
      height = 400, 
      bg = "white");
      
   plot(ggplot2(colorname ));
   dev.off();
}

