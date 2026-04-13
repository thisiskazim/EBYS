using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.Interfaces.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>> include = null);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        Task<int> SaveAsync();
        IQueryable<T> GetReadOnly();
        int GetContextUserId();

        //Derived olanları kalıtım olan sınıflarda kullanırız. GetAll() Base sınıfı tüm verileri getirir. bu ise alt sınıflarda hangisini istersek getirir
        Task<bool> AnyDerivedAsync<TDerived>(Expression<Func<TDerived, bool>> predicate)
            where TDerived : class, T;  
        Task<List<TDerived>> GetAllDerivedAsync<TDerived>() where TDerived : class, T;
    

    }
}
