using Core.Entities.Concrete;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Messages
    {
        internal static readonly string SuccessfulLogin;
        public static string DataAdded = "Data added";
        public static string DataUpdated = "Data updated";
        public static string DataDeleted = "Data deleted";
        public static string DataListed = "Data listed";
        public static string MaintenanceTime = "Maintenance time";
        public static string CarNotAvailable = "This car is busy !";
        public static string CarRentSuccessful = "Car successfully rented";
        public static string CarAddingLimitExceeded = "Car adding limit for a brand is exceeded !";
        public static string BrandModelAlreadyExisted = "Brand model already existed !";
        public static string BrandLimitExceeded = "Limit of the number of brands exceeded !";
        public static string BrandNameAlreadyExisted = "Brand name already existed !";
        public static string ColorNameAlreadyExisted = "Color name already existed !";
        public static string UserEmailAlreadyExisted = "User email already existed !";
        public static string AuthorizationDenied = "You have no authorization";
        public static string UserRegistered="User is sucessfully registered";
        public static string UserNotFound="User could not be found";
        public static string PasswordError="Wrong password !";
        public static string UserAlreadyExists = "User already existed";
        public static string AccessTokenCreated = "Access token is created";
    }
}
