using RobsonDev.Authentication.Models;
using RobsonDev.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobsonDev.Data.Repositories
{
    public interface IUserRepository : IRead<User>, IWrite<User>
    {
        public Task<User> FindAsync(User user);
    }
}
