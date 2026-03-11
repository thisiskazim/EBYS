using EBYS.Application.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace EBYS.Persistence.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly EBYSContext _context;
        public GenericRepository(EBYSContext context) => _context = context;
        public async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);
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
    }
}
