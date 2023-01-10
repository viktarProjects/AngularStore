using AngularStore.Core.Entities;
using AngularStore.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularStore.Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int Id);
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T> GetEntitySpec(ISpecification<T> specification);

        Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> specification);
    }
}
