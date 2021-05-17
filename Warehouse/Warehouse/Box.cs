using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse
{
    class Box
    {
        public double Mass { get; }
        public double CostForKilo { get; set; }


        /// <summary>
        /// Constructor like in the terms of reference.
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="costForKilo"></param>
        public Box(double mass, double costForKilo)
        {
            Mass = mass;
            CostForKilo = costForKilo;
        }


        /// <summary>
        /// Override to string for better usage in interface. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Box info: mass - {Mass}, cost for kilo - {CostForKilo};\n\n";
        }
    }
}
