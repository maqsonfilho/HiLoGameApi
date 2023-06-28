using AutoMapper;
using HiLoGame.Application.Interfaces;
using HiLoGame.Domain.MappingProfiles;
using HiLoGame.Infrastructure.Data.Context;
using HiLoGame.Infrastructure.Data.Interfaces;
using HiLoGame.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HiLoGame.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureMapping(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(map =>
            {
                map.AddProfile<GameMappingProfile>();
            });

            services.AddSingleton(mapperConfig.CreateMapper());
        }

        public static void RegisterDependencies(this IServiceCollection services)
        {
            #region Services
            services.AddScoped<IGameService, GameService>();
            #endregion

            #region Repositories
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            #endregion
        }

        public static void AddContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}
