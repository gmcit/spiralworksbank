using SpiralWorks.Interfaces;
using SpiralWorks.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SpiralWorks.Data.Ado.Repositories
{
    public class UniqueNumberRepository : IRepository<UniqueNumber>
    {
        IDbContext _dbContext;
        public UniqueNumberRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(UniqueNumber entity)
        {

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Insert into UniqueNumber(AccountNumber) Values ('{entity.AccountNumber}'); Select @@Identity as [Identity];";
            entity.UniqueNumberId = _dbContext.ExecuteNonQuery();

        }

        public void AddRange(List<UniqueNumber> list)
        {

            list.ForEach(x =>
            {
                _dbContext.CommandType = CommandType.Text;
                _dbContext.CommandText = $"Insert into UniqueNumber(AccountNumber) Values ('{x.AccountNumber}'); Select @@Identity as [Identity];";
                x.UniqueNumberId = _dbContext.ExecuteNonQuery();

            });

        }

        public void Commit()
        {
            _dbContext.ExecuteNonQuery();
        }

        public void Delete(int id)
        {

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Delete From UniqueNumber Where UniqueNumber={id}";
            _dbContext.ExecuteNonQuery();

        }

        public void Delete(UniqueNumber entity)
        {

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Delete From UniqueNumber Where UniqueNumberId={entity.UniqueNumberId}";
            _dbContext.ExecuteNonQuery();

        }

        public List<UniqueNumber> Find(Func<UniqueNumber, bool> match)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UniqueNumber> FindAll()
        {
            IQueryable<UniqueNumber> result = null;

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Select * from UniqueNumber";
            result = _dbContext.ExecuteToEntity<UniqueNumber>().AsQueryable();

            return result;
        }

        public UniqueNumber FindById(int id)
        {
            UniqueNumber result = null;

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Select * from UniqueNumber Where UniqueNumberId={id}";
            result = _dbContext.ExecuteToEntity<UniqueNumber>().SingleOrDefault();

            return result;
        }

        public void Update(UniqueNumber entity)
        {

            _dbContext.CommandType = CommandType.Text;
            _dbContext.CommandText = $"Update UniqueNumber set AccountNumber='{entity.AccountNumber}' where UniqueNumberId={entity.UniqueNumberId}";
            _dbContext.ExecuteNonQuery();

        }
    }
}
