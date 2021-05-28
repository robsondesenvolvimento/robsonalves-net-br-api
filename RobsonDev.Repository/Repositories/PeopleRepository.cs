using RobsonDev.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobsonDev.Repository.Repositories
{
    public class PeopleRepository : IPeopleRepository
    {
        /// <summary>
        /// Return a list of generics.
        /// </summary>
        /// <returns><see cref="IEnumerable{People}"/></returns>
        public async Task<IEnumerable<People>> AllAsync()
        {
            var list = new List<People>()
            {
                new People { Id = 1, Name = "Robson", Birthday = new DateTime(1980, 8, 29), Active = true },
                new People { Id = 2, Name = "Henrique", Birthday = new DateTime(2019, 7, 21), Active = true }
            };

            return await Task<IEnumerable<People>>.Factory.StartNew(() => list);
        }

        /// <summary>
        /// Find data by <see cref="id"/>.
        /// </summary>
        /// <param name="id">Identifier number.</param>
        /// <returns><see cref="People"/></returns>
        public async Task<People> FindAsync(int id)
        {
            var list = new List<People>()
            {
                new People { Id = 1, Name = "Robson", Birthday = new DateTime(1980, 8, 29), Active = true },
                new People { Id = 2, Name = "Henrique", Birthday = new DateTime(2019, 7, 21), Active = true }
            };

            return await Task<People>.Factory.StartNew(() => list.Find(key => key.Id == 2));

        }
    }
}
