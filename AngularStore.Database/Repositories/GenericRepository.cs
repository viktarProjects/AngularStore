using AngularStore.Core.Entities;
using AngularStore.Core.Interfaces;
using AngularStore.Database.Data;
using AngularStore.Database.Evaluators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularStore.Database.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _storeContet;

        public GenericRepository(StoreContext storeContet)
        {
            _storeContet = storeContet;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _storeContet.Set<T>().ToListAsync();
        }

        //public async Task<T> GetByIdAsync(int id) => 
        //    await _storeContet.Set<T>().FindAsync(id);

        public async Task<T> GetByIdAsync(int id)
        {
            return await _storeContet.Set<T>().FindAsync(id);
        }

        public async Task<T> GetEntitySpec(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(_storeContet.Set<T>().AsQueryable(), specification);
        }
    }
}
