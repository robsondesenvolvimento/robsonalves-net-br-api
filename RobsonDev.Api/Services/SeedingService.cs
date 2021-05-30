using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RobsonDev.Authentication.Models;
using RobsonDev.Data.Context;
using RobsonDev.Domain.Entities;
using RobsonDev.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobsonDev.Api.Services
{
    /// <summary>
    /// Seeding service static class.
    /// </summary>
    public static class SeedingService
    {
        /// <summary>
        /// Seeding tables with data of peoples.
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <returns></returns>
        public static async Task PeoplesSeedingStart(this IApplicationBuilder appBuilder)
        {
            using (var serviceScope = appBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var databaseContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                await databaseContext.Database.EnsureCreatedAsync();

                if (!databaseContext.Peoples.Any())
                {
                    var list = new List<People>()
                    {
                        new People { Name = "Robson", Birthday = new DateTime(1980, 8, 29), Active = true },
                        new People { Name = "Henrique", Birthday = new DateTime(2019, 7, 21), Active = true }
                    };

                    await databaseContext.Peoples.AddRangeAsync(list);
                    await databaseContext.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Seeding tables with data of users.
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <returns></returns>
        public static async Task UsersSeedingStart(this IApplicationBuilder appBuilder) 
        {
            using (var serviceScope = appBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var databaseContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                await databaseContext.Database.EnsureCreatedAsync();

                if (!databaseContext.Users.Any())
                {
                    var user = new User { Username = "robson", Password = PasswordCryptoHelper.GeneratePasswordHash("H+123456"), Role = "Administrator" };

                    await databaseContext.Users.AddAsync(user);
                    await databaseContext.SaveChangesAsync();
                }
            }
        }
    }
}
