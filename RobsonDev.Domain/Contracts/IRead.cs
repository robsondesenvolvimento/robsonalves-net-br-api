using System.Collections.Generic;
using System.Threading.Tasks;

namespace RobsonDev.Domain.Contracts
{
    public interface IRead<T>
    {
        /// <summary>
        /// Find data by <see cref="id"/>.
        /// </summary>
        /// <param name="id">Identifier number.</param>
        /// <returns><typeparamref name="T"/></returns>
        public Task<T> FindAsync(int id);

        /// <summary>
        /// Return a list of generics.
        /// </summary>
        /// <returns>IEnumerable&lt;<typeparamref name="T"/>&gt;</returns>
        public Task<IEnumerable<T>> AllAsync();
    }
}
