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
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        Task<int> SaveAsync();
        IQueryable<T> GetReadOnly();
        
        Task<bool> AnyDerivedAsync<TDerived>(Expression<Func<TDerived, bool>> predicate)
            where TDerived : class, T;  //TÜRETİLMİŞ SINIFLAR İÇİN ANY METODU;  MUHATAP TABLOSUNDA GENERİC REPOSITORYDE HER TÜRÜNÜN AYRI BİR TABLOSU OLMADIĞI İÇİN TÜRETİLMİŞ SINIFLARIN VARLIĞINI KONTROL ETMEK İÇİN KULLANILIR.
        
    }
}
