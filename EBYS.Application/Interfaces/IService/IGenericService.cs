using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.Interfaces.IService
{
    public interface IGenericService<TCreate, TUpdate, TList>
    {
        Task AddAsync(TCreate createDto);
        Task UpdateAsync(TUpdate updateDto);
        Task DeleteAsync(int id);
        Task<TUpdate> GetByIdAsync(int id);
        Task<List<TList>> GetAllAsync();
    }
}
