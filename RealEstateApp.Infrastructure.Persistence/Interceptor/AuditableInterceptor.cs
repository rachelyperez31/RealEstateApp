using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Domain.Common;
using RealEstateApp.Core.Application.Helpers;

namespace RealEstateApp.Infrastructure.Persistence.Interceptor
{
    public sealed class AuditableInterceptor : SaveChangesInterceptor
    {
        public readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationResponse _user;

        public AuditableInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            DbContext? dbContext = eventData.Context;
            if (dbContext is null)
            {
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            IEnumerable<EntityEntry<AuditableBaseEntity>> entries = dbContext.ChangeTracker.Entries<AuditableBaseEntity>();

            _user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

            string? userName = null; 
            if (_user == null)
            {
                userName = _httpContextAccessor.HttpContext?.User?.Claims
                                    .FirstOrDefault(c => c.Issuer == "CodeIdentity")?.Value ?? "defaultUser";
            }

            foreach (EntityEntry<AuditableBaseEntity> entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = userName ?? _user.UserName;
                    entry.Entity.CreatedOn = DateTime.Now;
                }
                else if (entry.Entity.IsDeleted)
                {
                    entry.Entity.DeletedBy = userName ?? _user.UserName;
                    entry.Entity.DeletedOn = DateTime.Now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedBy = userName ?? _user.UserName;
                    entry.Entity.ModifiedOn = DateTime.Now;
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
