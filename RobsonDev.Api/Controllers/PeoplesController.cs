using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobsonDev.Data.Repositories;
using RobsonDev.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobsonDev.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class PeoplesController : ControllerBase
    {
        private readonly IPeopleRepository _peopleRepository;

        public PeoplesController(IPeopleRepository peopleRepository)
        {
            _peopleRepository = peopleRepository;
        }

        /// <summary>
        /// Retorna uma lista de pessoas
        /// </summary>
        /// <returns><see cref="IEnumerable{People}"/></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<People>>> GetAsync()
        {
            var list = await _peopleRepository.AllAsync().ConfigureAwait(false);

            if (list == null) return NoContent();

            return Ok(list?.ToList());
        }

        /// <summary>
        /// Retorna uma pessoa pelo número de id informado.
        /// </summary>
        /// <returns><see cref="People"/></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<People>> GetAsync([FromRoute] int id)
        {
            var pessoa = await _peopleRepository.FindAsync(id).ConfigureAwait(false);

            if (pessoa == null) return NoContent();

            return Ok(pessoa);
        }
    }
}
