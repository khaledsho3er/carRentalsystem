using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAgency.classes
{
    public class CarAdapter : ICarAdapter
    {
        private readonly car _car;

        public CarAdapter(car car)
        {
            _car = car;
        }

        public void UpdateCar(string carId)
        {
            _car.update(carId);
        }
    }
}
