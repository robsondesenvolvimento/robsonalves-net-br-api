using Microsoft.EntityFrameworkCore;
using RobsonDev.Authentication.Models;
using RobsonDev.Data.Context;
using RobsonDev.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobsonDev.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Return a list of generics.
        /// </summary>
        /// <returns><see cref="IEnumerable{People}"/></returns>
        public async Task<IEnumerable<User>> AllAsync()
        {
            var list = await _context.Users.ToListAsync().ConfigureAwait(false);
            return list;
        }

        /// <summary>
        /// Find data by <see cref="id"/>.
        /// </summary>
        /// <param name="id">Identifier number.</param>
        /// <returns><see cref="People"/></returns>
        public async Task<User> FindAsync(int id)
        {
            var user = await _context.Users.FindAsync(id).ConfigureAwait(false);
            return user;

        }

        /// <summary>
        /// Find data by <see cref="user"/>.
        /// </summary>
        /// <param name="user"></param>
        /// <returns><see cref="User"/></returns>
        public async Task<User> FindAsync(User user)
        {
            var userFind = await _context.Users.SingleOrDefaultAsync(u => u.Username == user.Username);           

            return userFind;
        }

        public async Task<User> Insert(User user)
        {
            var userFind = await _context.Users.FindAsync(user);
            if (userFind == null) return null;

            user.Password = PasswordCryptoHelper.GeneratePasswordHash(user.Password);

            var userState = await _context.Users.AddAsync(user);
            return userState.Entity;
        }

        public async Task<bool> Update(User user)
        {
            var userFind = await _context.Users.FindAsync(user.Id);
            if (userFind == null) return false;

            user.Password = PasswordCryptoHelper.GeneratePasswordHash(user.Password);

            _context.Entry(userFind).CurrentValues.SetValues(user);
            var modifications = await _context.SaveChangesAsync();
            return modifications > 0;
        }
    }
}
