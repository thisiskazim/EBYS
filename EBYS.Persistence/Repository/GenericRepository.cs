using EBYS.Application.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace EBYS.Persistence.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly EBYSContext _context;
        public GenericRepository(EBYSContext context) => _context = context;
        public async Task<List<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);
        public void UpdateAsync(T entity) => _context.Set<T>().Update(entity);
        public void DeleteAsync(T entity) => _context.Set<T>().Remove(entity);
        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        public IQueryable<T> GetReadOnly()=> _context.Set<T>().AsNoTracking();

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsQueryable().AnyAsync(predicate);
        }

        public async Task<bool> AnyDerivedAsync<TDerived>(Expression<Func<TDerived, bool>> predicate)
            where TDerived : class, T
        {
            return await _context.Set<T>().OfType<TDerived>().AnyAsync(predicate);
        }

        public async Task<List<TDerived>> GetAllDerivedAsync<TDerived>() where TDerived : class, T
        {
            return await _context.Set<T>().OfType<TDerived>().ToListAsync();
        }

        //ilişkili tablolarun bulunduğu bir yerde güncelleme yapılacaksa bu uygun 
        public async Task<T> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _context.Set<T>();

            // Eğer include delegesi doluysa, sorguya uygula
            if (include != null)
            {
                query = include(query);
            }

            // ID'ye göre filtrele (Primary Key isminin 'Id' olduğunu varsayıyoruz)
            return await query.FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);
        }


    }
}
