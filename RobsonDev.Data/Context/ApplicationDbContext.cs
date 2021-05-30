using Microsoft.EntityFrameworkCore;
using RobsonDev.Authentication.Models;
using RobsonDev.Domain.Entities;

namespace RobsonDev.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<People> Peoples { get; set; }
    }
}
