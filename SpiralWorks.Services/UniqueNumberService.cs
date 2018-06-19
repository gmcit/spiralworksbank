using SpiralWorks.Interfaces;
using SpiralWorks.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpiralWorks.Services
{
    public class UniqueNumberService : IUniqueNumberService
    {
        IUnitOfWork _uow;

        public UniqueNumberService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void Add(UniqueNumber number)
        {
            _uow.UniqueNumbers.Add(number);
            _uow.SaveChanges();
        }
    }
}
