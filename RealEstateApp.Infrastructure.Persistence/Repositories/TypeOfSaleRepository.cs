using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class TypeOfSaleRepository : GenericRepository<TypeOfSale>, ITypeOfSaleRepository
    {
        private readonly ApplicationContext _dbContext;

        public TypeOfSaleRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async override Task<List<TypeOfSale>> GetAllAsync()
        {
            return await _dbContext.Set<TypeOfSale>()
                            .Include(b => b.Properties.Where(p => !p.IsDeleted))
                            .Where(e => !e.IsDeleted)
                            .ToListAsync();
        }
    }
}
