using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {

        IRentalRepository _rentalRepository;

        public RentalManager(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        [ValidationAspect(typeof(RentalValidator))]
        public IResult Add(Rental rental)
        {
            IResult result = BusinessRules.Run(CheckIfCarIsAvailable(rental.CarId));

            if (result != null)
            {
                return result;
            }

            _rentalRepository.Add(rental);
            return new SuccessResult(Messages.CarRentSuccessful);
        }

        public IResult Delete(int id)
        {
            Rental rentalToDelete = _rentalRepository.Get(r => r.RentalId == id);
            _rentalRepository.Delete(rentalToDelete);
            return new SuccessResult(Messages.DataDeleted);
        }

        [ValidationAspect(typeof(RentalValidator))]
        public IResult Update(Rental rental)
        {
            _rentalRepository.Update(rental);
            return new SuccessResult(Messages.DataUpdated);
        }

        public IDataResult<List<Rental>> GetAll()
        {
            return new SuccessDataResult<List<Rental>>(_rentalRepository.GetAll());
        }

        public IDataResult<Rental> GetById(int id)
        {
            return new SuccessDataResult<Rental>(_rentalRepository.Get(r => r.RentalId == id));
        }

        public IDataResult<List<RentalDetailDto>> GetRentalDetails()
        {
            return new SuccessDataResult<List<RentalDetailDto>>(_rentalRepository.GetRentalDetails());
        }

        public IDataResult<RentalDetailDto> GetRentalDetailsById(int id)
        {
            return new SuccessDataResult<RentalDetailDto>(_rentalRepository.GetRentalDetails().SingleOrDefault(r => r.RentalId == id));
        }

        public List<Rental> GetRentalsByCarId(int id)
        {
            return _rentalRepository.GetAll(r => r.CarId == id);
        }

        private IResult CheckIfCarIsAvailable(int carId)
        {
            List<Rental> oldRentals = GetRentalsByCarId(carId);
            bool isReturnDateNull = oldRentals.Any(r => r.ReturnDate == null);
            bool isCarAvailable = oldRentals.Any(r => r.RentDate <= DateTime.Now && r.ReturnDate > DateTime.Now);

            if (isReturnDateNull || isCarAvailable)
            {
                return new ErrorResult(Messages.CarNotAvailable);
            }

            return new SuccessResult();
        }
    }
}
