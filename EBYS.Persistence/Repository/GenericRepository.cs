using EBYS.Application.Interface;
using Microsoft.EntityFrameworkCore;


namespace EBYS.Persistence.Repository
{
    public class GenericRepository : IGenericRepository<T> where T : class
    {
        protected readonly EBYSContext _context;
        public GenericRepository(EBYSContext context) => _context = context;
        public async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);
        public async Task<List<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);
        public void Update(T entity) => _context.Set<T>().Update(entity);
        public void Delete(T entity) => _context.Set<T>().Remove(entity);
        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

    }
}
