using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class ImprovementRepository : GenericRepository<Improvement>, IImprovementRepository
    {
        private readonly ApplicationContext _dbContext;

        public ImprovementRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async override Task<List<Improvement>> GetAllAsync()
        {
            var entities = await _dbContext.Set<Improvement>()
                            .Include(b => b.Properties.Where(p => !p.IsDeleted))
                            .Where(e => !e.IsDeleted)
                            .ToListAsync();
            return entities;
        }
    }
}
