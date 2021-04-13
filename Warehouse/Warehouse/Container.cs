using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warehouse.Exceptions;

namespace Warehouse
{
    class Container
    {
        private static readonly Random r = new Random();
        private uint limitation;

        public List<Box> Boxes { get; }


        /// <summary>
        /// Sum of boxes masses that are in the container.
        /// </summary>
        /// <returns></returns>
        private double GetBoxesMass() => Boxes.Sum(box => box.Mass);


        /// <summary>
        /// Sum of boxes cost that are in the container.
        /// </summary>
        /// <returns></returns>
        public double GetContainerCost() => Boxes.Sum(box => box.CostForKilo * box.Mass);


        /// <summary>
        /// Damages each one container for the fixed from Warehouse damage.
        /// </summary>
        /// <param name="damage"></param>
        public void DamageContainer(double damage) => Boxes.ForEach(box => box.CostForKilo *= (1 - damage));


        /// <summary>
        /// Constructor from the terms of reference.
        /// </summary>
        /// <param name="boxes"></param>
        public Container(List<Box> boxes)
        {
            limitation = (uint)r.Next(951) + 50;
            Console.WriteLine($"Container limitation of weight - {limitation}");
            Boxes = boxes;
            if (GetBoxesMass() > limitation)
                throw new ContainerOverweightException($"Mass of boxes given to the container was to big, so container wasn't created, limitation - {limitation}, boxes mass - {GetBoxesMass()}");
        }


        /// <summary>
        /// Method for adding box to the container.
        /// </summary>
        /// <param name="box"></param>
        public void AddBox(Box box)
        {
            if (GetBoxesMass() + box.Mass > limitation)
                throw new ContainerOverweightException($"Box given to append to container size was bigger than capacity that was left int the container, left capactiy: {limitation - GetBoxesMass()}");
            Boxes.Add(box);
        }



        /// <summary>
        /// Override to string for better usage in interface. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder boxes = new StringBuilder();
            foreach (var box in Boxes)
                boxes.Append($"         {box}");
            return $"    limitation - {limitation}, boxes amount - {Boxes.Count}, container cost - {GetContainerCost()}, container summed mass - {GetBoxesMass()}, container left space - {limitation - GetBoxesMass()}\n" +
                boxes.ToString();
        }
    }
}
