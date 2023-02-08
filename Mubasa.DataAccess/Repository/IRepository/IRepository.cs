using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProp = null);
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProp = null);
        void Update(T entity);
        void Remove(T entity);
    }
}
