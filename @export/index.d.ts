// export R# source type define for javascript/typescript language
//
// package_source=ggplot

declare namespace ggplot {
   module _ {
      /**
      */
      function onLoad(): object;
   }
   /**
     * @param accents default value Is ``["#44546a", "#e7e6e6", "#5b9bd5", "#ed7d31", "#a5a5a5", "#ffc000", "#4472c4", "#70ad47"]``.
   */
   function msoffice(accents?: any): object;
   /**
   */
   function theme_black(): object;
   /**
   */
   function theme_default(): object;
   /**
     * @param axis.text default value Is ``null``.
     * @param axis.title default value Is ``null``.
     * @param legend.tick default value Is ``null``.
   */
   function theme_light(axis.text?: any, axis.title?: any, legend.tick?: any): object;
   /**
   */
   function theme_void(): object;
}
