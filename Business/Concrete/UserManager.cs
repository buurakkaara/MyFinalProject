using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
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
    public class UserManager : IUserService
    {

        IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [ValidationAspect(typeof(UserValidator))]
        public IResult Add(User user)
        {
            IResult result = BusinessRules.Run(CheckIfUserEmailExisted(user.Email));

            if (result != null)
            {
                return result;
            }

            _userRepository.Add(user);
            return new SuccessResult(Messages.DataAdded);
        }

        public IResult Delete(int id)
        {
            User userToDelete = _userRepository.Get(u => u.Id == id);
            _userRepository.Delete(userToDelete);
            return new SuccessResult(Messages.DataDeleted);
        }

        [ValidationAspect(typeof(UserValidator))]
        public IResult Update(User user)
        {
            _userRepository.Update(user);

            return new SuccessResult(Messages.DataUpdated);
        }

        public IDataResult<List<User>> GetAll()
        {
            return new SuccessDataResult<List<User>>(_userRepository.GetAll());
        }

        public IDataResult<User> GetById(int id)
        {
            return new SuccessDataResult<User>(_userRepository.Get(u => u.Id == id));
        }

        public List<OperationClaim> GetClaims(User user)
        {
            return _userRepository.GetClaims(user);
        }


        public User GetByMail(string email)
        {
            return _userRepository.Get(u => u.Email == email);
        }

        private IResult CheckIfUserEmailExisted(string Email)
        {
            var result = _userRepository.GetAll(c => c.Email == Email).Any();

            if (result)
            {
                return new ErrorResult(Messages.UserEmailAlreadyExisted);
            }

            return new SuccessResult();
        }

    }
}
