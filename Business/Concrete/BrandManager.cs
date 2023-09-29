using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BrandManager : IBrandService
    {
        IBrandRepository _brandRepository;

        public BrandManager(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        [ValidationAspect(typeof(BrandValidator))]
        public IResult Add(Brand brand)
        {
            IResult result = BusinessRules.Run(CheckIfBrandNameExisted(brand.BrandName));

            if (result != null)
            {
                return result;
            }

            _brandRepository.Add(brand);
            return new SuccessResult(Messages.DataAdded);
        }

        [ValidationAspect(typeof(BrandValidator))]
        public IResult Update(Brand brand)
        {
            _brandRepository.Update(brand);
            return new SuccessResult(Messages.DataUpdated);
        }

        public IResult Delete(int id)
        {
            Brand brandToDelete = _brandRepository.Get(b => b.BrandId == id);
            _brandRepository.Delete(brandToDelete);
            return new SuccessResult(Messages.DataDeleted);
        }

        public IDataResult<List<Brand>> GetAll()
        {
            return new SuccessDataResult<List<Brand>>(_brandRepository.GetAll());
        }

        public IDataResult<Brand> GetById(int id)
        {
            return new SuccessDataResult<Brand>(_brandRepository.Get(b => b.BrandId == id));
        }

        private IResult CheckIfBrandNameExisted(string brandName)
        {
            var result = _brandRepository.GetAll(b => b.BrandName == brandName).Any();

            if (result)
            {
                return new ErrorResult(Messages.BrandNameAlreadyExisted);
            }
            return new SuccessResult();
        }

    }
}
