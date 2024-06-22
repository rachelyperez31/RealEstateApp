using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class FavPropertyRepository : GenericRepository<FavProperty>, IFavPropertyRepository
    {
        private readonly ApplicationContext _dbContext;

        public FavPropertyRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async override Task<List<FavProperty>> GetAllAsync()
        {
            return await _dbContext.Set<FavProperty>()
                            .Include(b => b.Property)
                            .Where(e => !e.IsDeleted)
                            .ToListAsync();
        }

        public async Task<List<FavProperty>> GetFavoriteProperties(string clientId)
        {
            var favProperties = await _dbContext.Set<FavProperty>()
                                                .Where(fp => fp.UserId == clientId && !fp.IsDeleted)
                                                .ToListAsync();
            return favProperties;
        }

        public async Task<FavProperty> GetFavoriteProperty(string clientId, string code)
        {
            var property = await _dbContext.Set<FavProperty>().FindAsync(clientId, code);
            return property;
        }

        public async Task<bool> IsPropertyFavorite(string clientId, string code)
        {
            var favoriteProperty = await _dbContext.Set<FavProperty>().FindAsync(clientId, code);
            return favoriteProperty != null && !favoriteProperty.IsDeleted;
        }

        public async Task UpdateAsync(FavProperty favProperty, string clientId, string propertyCode)
        {
            var entry = _dbContext.Set<FavProperty>().Find(clientId, propertyCode);
            _dbContext.Entry(entry).CurrentValues.SetValues(favProperty);
            await _dbContext.SaveChangesAsync();
        }
    }
}
