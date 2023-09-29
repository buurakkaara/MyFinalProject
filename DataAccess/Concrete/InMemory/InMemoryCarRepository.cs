using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.InMemory
{
    public class InMemoryCarRepository : ICarRepository
    {

        List<Car> _cars;

        public InMemoryCarRepository()
        {
            _cars = new List<Car> { 
                new Car{CarId=1,BrandId=1,ColorId=1,DailyPrice=100,ModelYear=1999,Description="Güzel araba"},
                new Car{CarId=2,BrandId=1,ColorId=1,DailyPrice=200,ModelYear=1999,Description="Güzel araba"},
                new Car{CarId=3,BrandId=2,ColorId=1,DailyPrice=300,ModelYear=1999,Description="Güzel araba"},
                new Car{CarId=4,BrandId=3,ColorId=1,DailyPrice=400,ModelYear=1999,Description="Güzel araba"},
                new Car{CarId=5,BrandId=2,ColorId=1,DailyPrice=500,ModelYear=1999,Description="Güzel araba"}
                };
        }

        public void Add(Car car)
        {
            _cars.Add(car);
        }

        public void Delete(Car car)
        {
            Car carToDelete = _cars.SingleOrDefault(c => c.CarId == car.CarId);
            _cars.Remove(carToDelete);
        }

        public Car Get(Expression<Func<Car, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<Car> GetAll()
        {
            return _cars;
        }

        public List<Car> GetAll(Expression<Func<Car, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public List<Car> GetAllByBrandId(int brandId)
        {
            return _cars.Where(c => c.BrandId == brandId).ToList();
        }

        public Car GetById(int id)
        {
            return _cars.SingleOrDefault(c => c.CarId == id);
        }

        public List<CarDetailDto> GetCarDetails()
        {
            throw new NotImplementedException();
        }

        public CarDetailDto GetCarDetailsById(int id)
        {
            throw new NotImplementedException();
        }

        public CarDetailDto GetCarDetailsById(Expression<Func<Car, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public void Update(Car car)
        {
            Car carToUpdate = _cars.SingleOrDefault(c => c.CarId == car.CarId);
            carToUpdate.BrandId=car.BrandId;
            carToUpdate.ColorId=car.ColorId;
            carToUpdate.ModelYear=car.ModelYear;
            carToUpdate.Description=car.Description;
            carToUpdate.DailyPrice=car.DailyPrice;
        }
    }
}
