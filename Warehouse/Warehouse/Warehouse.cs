using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Exceptions;

namespace Warehouse
{


    /// <summary>
    /// Singleton class for showing warehouse state.
    /// </summary>
    class Warehouse
    {

        private static Warehouse instance = null;

        private static readonly Random r = new Random();

        public double StorageCost { get; }
        public List<Container> Containers { get; set; }

        private int containerCount;

        public int ContainerLimit { get; }

        /// <summary>
        /// Constructor that is called once.
        /// </summary>
        /// <param name="storageCost"> Storage cost. </param>
        /// <param name="containerLimit"> Container limit. </param>
        private Warehouse(double storageCost, int containerLimit)
        {
            StorageCost = storageCost;
            Containers = new List<Container>(0);
            containerCount = 0;
            ContainerLimit = containerLimit;
        }


        /// <summary>
        /// Clears warehouse for the next iteration of the program.
        /// </summary>
        public void ClearWarehouse()
        {
            Containers = new List<Container>(0);
            containerCount = 0;
        }


        /// <summary>
        /// Method for getting the sinleton copy of warehouse.
        /// </summary>
        /// <param name="storageCost"> Storeage cost, optional. </param>
        /// <param name="containerLimit">Container limit, optional. </param>
        /// <returns> Link to the warehouse. </returns>
        public static Warehouse GetWarehouse(double storageCost = 0, int containerLimit = 0)
        {
            if (instance == null)
                instance = new Warehouse(storageCost, containerLimit);
            return instance;
        }


        /// <summary>
        /// Creates and adds container to the list.
        /// </summary>
        /// <param name="container"></param>
        public void AddContainer(Container container)
        {
            double damage = r.NextDouble() * 0.5;
            container.DamageContainer(damage);
            Console.WriteLine($"Added container is damaged for {damage}!");
            if (container.GetContainerCost() < StorageCost)
                throw new UnprofitableContainerException("Cost of storing container is less than it's value");
            AddContainerToList(container);
        }


        /// <summary>
        /// Adds container  to the list of containers
        /// </summary>
        /// <param name="container"></param>
        private void AddContainerToList(Container container)
        {
            // Deleting last container and adding new one
            if (containerCount == ContainerLimit)
            {
                if (ContainerLimit == 0)
                {
                    Console.WriteLine("container limit is 0 no contatiners could be added");
                    return;
                }
                Console.WriteLine($"Removed container #{Containers.Count - 1}, added container #{Containers.Count - 1}");
                Containers[Containers.Count - 1] = null;
                Containers.Add(container);
            }
            else
            {
                Console.WriteLine($"Container #{Containers.Count} was added to warehouse");
                Containers.Add(container);
                containerCount++;
            }
        }


        /// <summary>
        /// Deletes container from the list.
        /// </summary>
        /// <param name="id"> Which container to delete. </param>
        public void DeleteContainer(int id)
        {
            if (id >= 0 && id < Containers.Count && Containers[id] != null)
            {
                Containers[id] = null;
                containerCount--;
                Console.WriteLine($"Container #{id} was deleted");
            }
            else
            {
                Console.WriteLine($"There is no container with id {id}");
            }
        }


        /// <summary>
        /// Adds box to the specifiyed container
        /// </summary>
        /// <param name="id"></param>
        /// <param name="box"></param>
        public void AddBox(int id, Box box)
        {
            if (id >= 0 && id < Containers.Count && Containers[id] != null)
            {
                Containers[id].AddBox(box);
            }
            else
            {
                throw new ArgumentException("Wrong id was given to AddBox - there is no such container");
            }
        }


        /// <summary>
        /// Overriding to string for better visualisation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            StringBuilder containersInfo = new StringBuilder();
            for (int i = 0; i < Containers.Count; i++)
                if (Containers[i] != null)
                    containersInfo.Append($"Container #{i} information:\n" +
                        $"----------\n" +
                        $"{Containers[i]}" +
                        $"----------\n");
            return
                "Warehouse information:\n" +
                $"Storage cost: {StorageCost}, container limit: {ContainerLimit}, container count - {containerCount}\n" +
                $"Containers:\n" +
                $"{containersInfo}";

        }
    }
}
