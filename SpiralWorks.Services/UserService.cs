using SpiralWorks.Interfaces;
using SpiralWorks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpiralWorks.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork _uow;
        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public User Authenticate(string email, string password)
        {
            try
            {
                return _uow.Users.FindAll().Where(x => x.Email.Equals(email) && x.Password.Equals(password)).SingleOrDefault();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public void Register(User user)
        {
            try
            {
                _uow.Users.Add(user);
                _uow.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
