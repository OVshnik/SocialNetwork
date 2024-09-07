using SocialNetwork.Data.Repository;
using SocialNetwork.Data.UOW;

namespace SocialNetwork.ViewModels.Extensions
{
    public static class ServiceExtentions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddSingleton<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddCustomRepository<TEntity, IRepository>(this IServiceCollection services)
                 where TEntity : class
                 where IRepository : class, IRepository<TEntity>
        {
            services.AddSingleton<IRepository<TEntity>, IRepository>();

            return services;
        }
    }
}
