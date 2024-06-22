using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        private readonly ApplicationContext _dbContext;

        public ImageRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        //public async override Task<List<Image>> GetAllAsync()
        //{
        //    return await _dbContext.Set<Image>()
        //                    .Include(b => b.Properties)
        //                    .Where(e => !e.IsDeleted)
        //                    .ToListAsync();
        //}

        public async Task<List<Image>> GetImagesByPropertyId(int propertyId)
        {
            return await _dbContext.Set<Image>()
                .Where(i => i.PropertyId == propertyId)
                .ToListAsync();
        }
    }
}
