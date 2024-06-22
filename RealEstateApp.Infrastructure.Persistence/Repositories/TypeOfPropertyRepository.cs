using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Common;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;
using System.Runtime.CompilerServices;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class TypeOfPropertyRepository : GenericRepository<TypeOfProperty>, ITypeOfPropertyRepository
    {
        private readonly ApplicationContext _dbContext;

        public TypeOfPropertyRepository(ApplicationContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public async override Task<List<TypeOfProperty>> GetAllAsync()
        {
            return await _dbContext.Set<TypeOfProperty>()
                            .Include(b => b.Properties.Where(p => !p.IsDeleted))
                            .Where(e => !e.IsDeleted)
                            .ToListAsync();
        }
    }
}
