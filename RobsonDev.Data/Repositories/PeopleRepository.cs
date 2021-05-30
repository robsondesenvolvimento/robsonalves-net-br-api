using Microsoft.EntityFrameworkCore;
using RobsonDev.Data.Context;
using RobsonDev.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RobsonDev.Data.Repositories
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly ApplicationDbContext _context;

        public PeopleRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Return a list of generics.
        /// </summary>
        /// <returns><see cref="IEnumerable{People}"/></returns>
        public async Task<IEnumerable<People>> AllAsync()
        {
            var list = await _context.Peoples.ToListAsync().ConfigureAwait(false);
            return list;
        }

        /// <summary>
        /// Find data by <see cref="id"/>.
        /// </summary>
        /// <param name="id">Identifier number.</param>
        /// <returns><see cref="People"/></returns>
        public async Task<People> FindAsync(int id)
        {
            var people = await _context.Peoples.FindAsync(id).ConfigureAwait(false);
            return people;

        }
    }
}
