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
    public class ColorManager : IColorService
    {
        IColorRepository _colorRepository;

        public ColorManager(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }

        [ValidationAspect(typeof(ColorValidator))]
        public IResult Add(Color color)
        {
            IResult result = BusinessRules.Run(CheckIfColorNameExisted(color.ColorName));

            if (result != null)
            {
                return result;
            }

            _colorRepository.Add(color);
            return new SuccessResult(Messages.DataAdded);
        }

        [ValidationAspect(typeof(ColorValidator))]
        public IResult Update(Color color)
        {
            _colorRepository.Update(color);
            return new SuccessResult(Messages.DataUpdated);
        }

        public IResult Delete(int id)
        {
            Color colorToDelete = _colorRepository.Get(c => c.ColorId == id);
            _colorRepository.Delete(colorToDelete);
            return new SuccessResult(Messages.DataDeleted);
        }

        public IDataResult<List<Color>> GetAll()
        {
            return new SuccessDataResult<List<Color>>(_colorRepository.GetAll());
        }

        public IDataResult<Color> GetById(int id)
        {
            return new SuccessDataResult<Color>(_colorRepository.Get(c => c.ColorId == id));
        }

        private IResult CheckIfColorNameExisted(string colorName)
        {
            var result = _colorRepository.GetAll(c => c.ColorName == colorName).Any();

            if (result)
            {
                return new ErrorResult(Messages.ColorNameAlreadyExisted);
            }
            return new SuccessResult();
        }
    }
}
