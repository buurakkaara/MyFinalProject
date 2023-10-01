using Business.Abstract;
using Business.Constants;
using Core.Utilities.Business;
using Core.Utilities.Helpers.FileHelper;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CarImageManager : ICarImageService
    {
        ICarImageRepository _carImageRepository;
        IFileHelper _fileHelper;
        public CarImageManager(ICarImageRepository carImageRepository, IFileHelper fileHelper)
        {
            _carImageRepository = carImageRepository;
            _fileHelper = fileHelper;
        }
        public IResult Add(IFormFile file, CarImage carImage)
        {
            IResult result = BusinessRules.Run(CheckIfCarImageLimitExceeded(carImage.CarId));
            if (result != null)
            {
                return result;
            }
            carImage.ImagePath = _fileHelper.Upload(file, PathConstants.ImagesPath);
            carImage.UploadDate = DateTime.Now;
            _carImageRepository.Add(carImage);
            return new SuccessResult("Image is successfully added");
        }

        public IResult Delete(int id)
        {
            CarImage carImageToDelete = _carImageRepository.Get(c => c.Id == id);
            _fileHelper.Delete(PathConstants.ImagesPath + carImageToDelete.ImagePath);
            _carImageRepository.Delete(carImageToDelete);
            return new SuccessResult();
        }
        public IResult Update(IFormFile file, CarImage carImage)
        {
            carImage.ImagePath = _fileHelper.Update(file, PathConstants.ImagesPath + carImage.ImagePath, PathConstants.ImagesPath);
            _carImageRepository.Update(carImage);
            return new SuccessResult();
        }

        public IDataResult<List<CarImage>> GetByCarId(int carId)
        {
            var result = BusinessRules.Run(CheckCarImage(carId));
            if (result != null)
            {
                return new ErrorDataResult<List<CarImage>>(GetDefaultImage(carId).Data);
            }
            return new SuccessDataResult<List<CarImage>>(_carImageRepository.GetAll(c => c.CarId == carId));
        }

        public IDataResult<CarImage> GetByImageId(int imageId)
        {
            return new SuccessDataResult<CarImage>(_carImageRepository.Get(c => c.Id == imageId));
        }

        public IDataResult<List<CarImage>> GetAll()
        {
            return new SuccessDataResult<List<CarImage>>(_carImageRepository.GetAll());
        }
        
        private IDataResult<List<CarImage>> GetDefaultImage(int carId)
        {

            List<CarImage> carImage = new List<CarImage>();
            carImage.Add(new CarImage { CarId = carId, UploadDate = DateTime.Now, ImagePath = "DefaultImage.jpg" });
            return new SuccessDataResult<List<CarImage>>(carImage);
        }

        private IResult CheckCarImage(int carId)
        {
            var result = _carImageRepository.GetAll(c => c.CarId == carId).Count;
            if (result < 1)
            {
                return new ErrorResult();
            }
            return new SuccessResult();
        }

        private IResult CheckIfCarImageLimitExceeded(int carId)
        {
            var result = _carImageRepository.GetAll(c => c.CarId == carId).Count;
            if (result >= 5)
            {
                return new ErrorResult();
            }
            return new SuccessResult();
        }
    }
}
