using Microsoft.EntityFrameworkCore;
using Myshop.DataAccess.Data;
using Myshop.Entities.Repositres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.DataAccess.Implementaion
{
    public class GenericRepositry<T> : IGenericRepositry<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbset;
        public GenericRepositry(ApplicationDbContext context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate=null, string? includeword=null)
        {
            IQueryable<T> query = _dbset;
            if(predicate is not null)
            {
                query = query.Where(predicate);
            }
            if(includeword is not null)
            {
                foreach(var item in includeword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.ToList();
        }

        public T GetFirstorDefault(Expression<Func<T, bool>> ?predicate=null, string? includeword=null)
        {
            IQueryable<T> query = _dbset;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (includeword != null)
            {
                foreach (var item in includeword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.SingleOrDefault();
        }

        public void Remove(T entity)
        {
            _dbset.Remove(entity);
        }

        public void RemoveRang(IEnumerable<T> entites)
        {
            _dbset.RemoveRange(entites);
        }
    }
}
