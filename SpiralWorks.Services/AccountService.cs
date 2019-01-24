using SpiralWorks.Interfaces;
using SpiralWorks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpiralWorks.Services
{
    public class AccountService : IAccountService
    {
        IUnitOfWork _uow;
        public AccountService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void CreateAccount(Account model, int userId)
        {
            try
            {
                _uow.Accounts.Add(model);
                _uow.SaveChanges();

                var dto = new UserAccount()
                {
                    AccountId = model.AccountId,
                    UserId = userId
                };

                _uow.UserAccounts.Add(dto);
                _uow.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public UniqueNumber CreateAccountNumber()
        {
            var dto = new UniqueNumber();
            _uow.UniqueNumbers.Add(dto);
            _uow.SaveChanges();
            return dto;
        }

        public Account GetAccount(int accountId)
        {
            try
            {
                return _uow.Accounts.FindById(accountId);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Account> GetAccounts(int userId)
        {
            try
            {
                var userAccounts = _uow.UserAccounts.FindAll().Where(x => x.UserId.Equals(userId));

                var accounts = new List<Account>();

                userAccounts.ToList().ForEach(x =>
                {
                    var dto = _uow.Accounts.FindById(x.AccountId);
                    accounts.Add(dto);
                });

                return accounts;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<Account> GetOtherAccounts(int userId)
        {
            try
            {
                var userAccounts = _uow.UserAccounts.FindAll().Where(x => x.UserId != userId);

                var accounts = new List<Account>();

                userAccounts.ToList().ForEach(x =>
                {
                    var dto = _uow.Accounts.FindById(x.AccountId);
                    accounts.Add(dto);
                });

                return accounts;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public void UpdateAccount(Account model)
        {
            try
            {
                _uow.Accounts.Update(model);
                _uow.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
