using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.Entities.Repositres
{
    public interface IGenericRepositry<T>where T:class
    {
        //_context.categories.where(c=>c.id ==id).tolist();
        //_context.categories.include("product").tolist();
        IEnumerable<T> GetAll(Expression<Func<T,bool>>?predicate = null,string? includeword = null);
        //_context.categories.where(c=>c.id ==id).tosingleordefault();
        //_context.categories.include("product").tosingleordefault();
        T GetFirstorDefault(Expression<Func<T, bool>>? predicate = null, string? includeword= null);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRang(IEnumerable<T> entites);
    }
}
