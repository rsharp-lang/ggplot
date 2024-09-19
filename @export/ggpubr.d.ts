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
     * @param mapping default value Is ``null``.
     * @param data default value Is ``null``.
     * @param stat default value Is ``'identity'``.
     * @param position default value Is ``'identity'``.
     * @param parse default value Is ``false``.
     * @param nudge_x default value Is ``0``.
     * @param nudge_y default value Is ``0``.
     * @param na_rm default value Is ``false``.
     * @param show_legend default value Is ``false``.
     * @param inherit_aes default value Is ``true``.
     * @param color default value Is ``'steelblue'``.
     * @param which default value Is ``null``.
     * @param alpha default value Is ``1``.
     * @param size default value Is ``null``.
     * @param args default value Is ``null``.
     * @param env default value Is ``null``.
   */
   function geom_text_repel(mapping?: object, data?: any, stat?: string, position?: string, parse?: boolean, nudge_x?: number, nudge_y?: number, na_rm?: boolean, show_legend?: boolean, inherit_aes?: boolean, color?: any, which?: object, alpha?: number, size?: object, args?: object, env?: object): object;
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
