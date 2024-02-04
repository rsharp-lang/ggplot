// export R# package module type define for javascript/typescript language
//
//    imports "ggpubr" from "ggpubr";
//
// ref=ggpubr.Rscript@ggpubr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
*/
declare namespace ggpubr {
   /**
    * ### Compute normal data ellipses
    * 
    * 
     * @param data The data to be displayed in this layer. There are three options:
     *  If NULL, the Default, the data Is inherited from the plot data As specified In the Call To ggplot().
     *  A data.frame, Or other Object, will override the plot data. All objects will be fortified To produce a data frame. See fortify() For which variables will be created.
     *  A Function will be called With a Single argument, the plot data. The Return value must be a data.frame, And will be used As the layer data. A Function can be created from a formula (e.g. ~ head(.x, 10)).
     * 
     * + default value Is ``null``.
     * @param color -
     * 
     * + default value Is ``null``.
     * @param level The level at which to draw an ellipse, or, if type="euclid", the radius of the circle to be drawn.
     * 
     * + default value Is ``0.95``.
     * @param alpha -
     * 
     * + default value Is ``0.6``.
   */
   function stat_ellipse(data?: any, color?: any, level?: number, alpha?: number): object;
}
