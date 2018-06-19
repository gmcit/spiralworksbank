using SpiralWorks.Interfaces;
using SpiralWorks.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SpiralWorks.Data.Ado.Repositories
{
    public class UserAccountRepository : IRepository<UserAccount>
    {
        IDbContext _dbContext;
        public UserAccountRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(UserAccount entity)
        {

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Insert into UserAccount(UserId,AccountId) Values ({entity.UserId},{entity.AccountId}); Select @@Identity as Identity;";
                entity.UserAccountId = _dbContext.ExecuteNonQuery();

        }

        public void AddRange(List<UserAccount> list)
        {

            list.ForEach(x =>
            {
                _dbContext.CommandType = CommandType.Text;
                _dbContext.CommandText = $"Insert into UserAccount(UserId,AccountId) Values ({x.UserId},{x.AccountId}); Select @@Identity as Identity;";
                x.UserAccountId = _dbContext.ExecuteNonQuery();

            });

        }

        public void Commit()
        {
            _dbContext.ExecuteNonQuery();
        }

        public void Delete(int id)
        {

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Delete From UserAccount Where UserAccountId={id}";
            _dbContext.ExecuteNonQuery();

        }

        public void Delete(UserAccount entity)
        {

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Delete From UserAccount Where UserAccountId={entity.UserAccountId}";
            _dbContext.ExecuteNonQuery();

        }

        public List<UserAccount> Find(Func<UserAccount, bool> match)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserAccount> FindAll()
        {
            IQueryable<UserAccount> result = null;

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Select * from UserAccount";
            result = _dbContext.ExecuteToEntity<UserAccount>().AsQueryable();

            return result;
        }

        public UserAccount FindById(int id)
        {
            UserAccount result = null;

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Select * from UserAccount Where UserAccountId={id}";
            result = _dbContext.ExecuteToEntity<UserAccount>().SingleOrDefault();

            return result;
        }

        public void Update(UserAccount entity)
        {

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Update UserAccount set UserId={entity.UserId}, AccountId={entity.AccountId} where UserAccountId={entity.UserAccountId}";
            _dbContext.ExecuteNonQuery();

        }
    }
}
