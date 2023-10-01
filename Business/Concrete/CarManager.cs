using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CarManager : ICarService
    {

        ICarRepository _carRepository;
        IBrandService _brandService;

        public CarManager(ICarRepository carRepository, IBrandService brandService)
        {
            _carRepository = carRepository;
            _brandService = brandService;
        }

        [CacheRemoveAspect("ICarService.Get")]
        [SecuredOperation("admin,car.add")]
        [ValidationAspect(typeof(CarValidator))]
        public IResult Add(Car car)
        {
            IResult result = BusinessRules.Run(CheckIfCarAddingLimitForABrandExceeded(car.BrandId),
                                               CheckIfBrandModelExisted(car.BrandModel),
                                               CheckIfBrandLimitExceeded());

            if (result != null)
            {
                return result;
            }

            _carRepository.Add(car);
            return new SuccessResult(Messages.DataAdded);
        }

        [ValidationAspect(typeof(CarValidator))]
        public IResult Update(Car car)
        {
            _carRepository.Update(car);
            return new SuccessResult(Messages.DataUpdated);
        }

        public IResult Delete(int id)
        {
            Car carToDelete = _carRepository.Get(c => c.CarId == id);
            _carRepository.Delete(carToDelete);

            return new SuccessResult(Messages.DataDeleted);
        }

        [CacheAspect]
        public IDataResult<List<Car>> GetAll()
        {
            if(DateTime.Now.Hour == 23)
            {
                return new ErrorDataResult<List<Car>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<Car>>(_carRepository.GetAll(),Messages.DataListed);
        }

        public IDataResult<List<Car>> GetAllByBrandId(int id)
        {
            return new SuccessDataResult<List<Car>>(_carRepository.GetAll(c => c.BrandId == id));
        }

        public IDataResult<List<Car>> GetAllByColorId(int id)
        {
            return new SuccessDataResult<List<Car>>(_carRepository.GetAll(c => c.ColorId == id));
        }

        [CacheAspect]
        public IDataResult<Car> GetById(int id)
        {
            return new SuccessDataResult<Car>(_carRepository.Get(c=> c.CarId == id));
        }

        public IDataResult<List<CarDetailDto>> GetCarDetails()
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carRepository.GetCarDetails());
        }

        public IDataResult<List<Car>> GetCarsByDailyPrice(int min, int max)
        {
            return new SuccessDataResult<List<Car>>
                (_carRepository.GetAll(c => c.DailyPrice >= min && c.DailyPrice <= max));
        }

        public IDataResult<CarDetailDto> GetCarDetailsById(int id)
        {
            return new SuccessDataResult<CarDetailDto>(_carRepository.GetCarDetails().SingleOrDefault(c => c.CarId == id));
        }

        private IResult CheckIfCarAddingLimitForABrandExceeded(int brandId)
        {
            var result = _carRepository.GetAll(c => c.BrandId == brandId).Count;

            if (result > 10)
            {
                return new ErrorResult(Messages.CarAddingLimitExceeded);
            }

            return new SuccessResult();
        }

        private IResult CheckIfBrandModelExisted(string brandModel)
        {
            var result = _carRepository.GetAll(c => c.BrandModel == brandModel).Any();

            if (result)
            {
                return new ErrorResult(Messages.BrandModelAlreadyExisted);
            }

            return new SuccessResult();
        }

        private IResult CheckIfBrandLimitExceeded()
        {
            var result = _brandService.GetAll();

            if (result.Data.Count > 50)
            {
                return new ErrorResult(Messages.BrandLimitExceeded);
            }

            return new SuccessResult();
        }
    }
}   
