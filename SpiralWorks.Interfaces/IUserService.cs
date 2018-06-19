using SpiralWorks.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpiralWorks.Interfaces
{
    public interface IUserService
    {
        User Authenticate(string email, string password);
        void Register(User user);
    }
}
