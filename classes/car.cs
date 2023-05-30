using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAgency.classes
{
    // Define the target interface that the client expects
    public interface ICarAdapter
    {
        void UpdateCar(string carId);
    }

    // Implement the adapter class that adapts the car class to the target interface
   

    // Modify the original car class to implement the target interface
    internal abstract class car : ICarAdapter
    {
        public car()
        {
        }

        private string cbrand { get; set; }
        private string model { get; set; }
        private string modelyear { get; set; }
        private string cqty { get; set; }
        private string cprice { get; set; }
        private string description { get; set; }
        private string csupplier { get; set; }

        private string createdby { get; set; }
        private string createddate { get; set; }

        public abstract void update(string cID);

        // Implement the interface method
        public void UpdateCar(string carId)
        {
            update(carId);
        }
    }

}
