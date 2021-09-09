using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HospitalMgmtSystem.DAL.Repository
{
    public class GenericRepository<T> : ContextRepository where T : class
    {
        private static GenericRepository<T> _instance;

        public GenericRepository(ILogger logger)
        {
            this._logger = logger;
        }

        public GenericRepository()
        {
        }

        public static GenericRepository<T> Inst
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GenericRepository<T>();
                }
                return _instance;
            }
        }

        public DbSet<T> Set
        {
            get
            {
                return JoinContext.Set<T>();
            }
        }

        //private DbSet<T> table;
        private readonly ILogger _logger;

        public List<T> GetAll()
        {
            return Set.Include("Users").ToList();
        }

        public T GetById(object Id)
        {
            return Set.Find(Id);
        }

        public IEnumerable<T> GetByCondition(Func<T, bool> condition)
        {
            return Set.Where(condition);
        }

        public bool Create(T model)
        {
            try
            {
                Set.Add(model);
                Save();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Source} - {ex.Message}");
                return false;
            }
            return true;
        }

        public bool Update(T model)
        {
            try
            {
                Set.Update(model);
                Save();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Source} - {ex.Message}");
                return false;
            }
            return true;
        }

        public void Save()
        {
            try
            {
                JoinContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Source} - {ex.Message}");
            }
        }
    }
}
