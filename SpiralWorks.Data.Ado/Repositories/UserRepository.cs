using SpiralWorks.Interfaces;
using SpiralWorks.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SpiralWorks.Data.Ado.Repositories
{
    public class UserRepository : IRepository<User>
    {
        IDbContext _dbContext;
        public UserRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(User entity)
        {

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Insert into User(Email,Password,FirstName,LastName,Birtdate,DateCreated) Values ('{entity.Email}','{entity.Password}','{entity.FirstName}','{entity.LastName}',{entity.BirthDate},{entity.DateCreated}); Select @@Identity as Identity;";
            entity.UserId = _dbContext.ExecuteNonQuery();

        }

        public void AddRange(List<User> list)
        {

            list.ForEach(x =>
            {
                _dbContext.CommandType = CommandType.Text;
                _dbContext.CommandText = $"Insert into User(Email,Password,FirstName,LastName,Birtdate,DateCreated) Values ('{x.Email}','{x.Password}','{x.FirstName}','{x.LastName}',{x.BirthDate},{x.DateCreated}); Select @@Identity as Identity;";
                x.UserId = _dbContext.ExecuteNonQuery();

            });

        }

        public void Commit()
        {
            _dbContext.ExecuteNonQuery();
        }

        public void Delete(int id)
        {

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Delete From [User] Where UserId={id}";
            _dbContext.ExecuteNonQuery();

        }

        public void Delete(User entity)
        {

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Delete From [User] Where UserId={entity.UserId}";
            _dbContext.ExecuteNonQuery();

        }

        public List<User> Find(Func<User, bool> match)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> FindAll()
        {
            IQueryable<User> result = null;

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Select * from [User]";
            result = _dbContext.ExecuteToEntity<User>().AsQueryable();

            return result;
        }

        public User FindById(int id)
        {
            User result = null;

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Select * from [User] Where UserAccountId={id}";
            result = _dbContext.ExecuteToEntity<User>().SingleOrDefault();

            return result;
        }

        public void Update(User entity)
        {

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Update [User] set Email={entity.Email}, Password={entity.Password}, FirstName={entity.FirstName},LastName={entity.LastName}, BirthDate={entity.BirthDate} where UserAccountId={entity.UserId}";
            _dbContext.ExecuteNonQuery();

        }
    }
}
