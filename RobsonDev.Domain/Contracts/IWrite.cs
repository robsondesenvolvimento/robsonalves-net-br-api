using RobsonDev.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobsonDev.Domain.Contracts
{
    public interface IWrite<T>
    {
        public Task<User> Insert(T obj);
        public Task<bool> Update(T obj);
    }
}
