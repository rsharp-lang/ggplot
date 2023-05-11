// export R# package module type define for javascript/typescript language
//
// ref=ggplot.ggforcePkg@ggraph, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
*/
declare namespace ggforce {
   /**
     * @param ejectFactor default value Is ``6``.
     * @param condenseFactor default value Is ``3``.
     * @param maxtx default value Is ``4``.
     * @param maxty default value Is ``3``.
     * @param dist_threshold default value Is ``'30,250'``.
     * @param size default value Is ``'1000,1000'``.
     * @param iterations default value Is ``20000``.
     * @param time_step default value Is ``1E-05``.
     * @param algorithm default value Is ``["force_directed","degree_weighted","group_weighted","edge_weighted"]``.
     * @param env default value Is ``null``.
   */
   function layout_forcedirected(ejectFactor?:object, condenseFactor?:object, maxtx?:object, maxty?:object, dist_threshold?:any, size?:any, iterations?:object, time_step?:number, algorithm?:any, env?:object): object;
   /**
   */
   function layout_random(): object;
   /**
     * @param maxRepulsiveForceDistance default value Is ``10``.
     * @param c default value Is ``2``.
     * @param iterations default value Is ``100``.
     * @param env default value Is ``null``.
   */
   function layout_springembedder(canvas: any, maxRepulsiveForceDistance?: number, c?: number, iterations?: object, env?: object): object;
   /**
     * @param stiffness default value Is ``50000``.
     * @param repulsion default value Is ``100``.
     * @param damping default value Is ``0.9``.
     * @param iterations default value Is ``1000``.
     * @param time_step default value Is ``0.0001``.
   */
   function layout_springforce(stiffness?:number, repulsion?:number, damping?:number, iterations?:object, time_step?:number): object;
   function layout_springforce(stiffness?: number, repulsion?: number, damping?: number, iterations?: object, time_step?: number): object;
   /**
     * @param ejectFactor default value Is ``6``.
     * @param condenseFactor default value Is ``3``.
     * @param maxtx default value Is ``4``.
     * @param maxty default value Is ``3``.
     * @param dist_threshold default value Is ``'30,250'``.
     * @param size default value Is ``'1000,1000'``.
     * @param iterations default value Is ``20000``.
     * @param time_step default value Is ``1E-05``.
     * @param algorithm default value Is ``["force_directed","degree_weighted","group_weighted","edge_weighted"]``.
     * @param env default value Is ``null``.
   */
   function layout_forcedirected(ejectFactor?: object, condenseFactor?: object, maxtx?: object, maxty?: object, dist_threshold?: any, size?: any, iterations?: object, time_step?: number, algorithm?: any, env?: object): object;
}
