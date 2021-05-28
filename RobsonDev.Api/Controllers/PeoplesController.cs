using Microsoft.AspNetCore.Mvc;
using RobsonDev.Domain.Contracts;
using RobsonDev.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobsonDev.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<People>>> GetAsync()
        {
            var list = await _peopleRepository.AllAsync().ConfigureAwait(false);
            return list?.ToList();
        }

        /// <summary>
        /// Retorna uma pessoa pelo número de id informado.
        /// </summary>
        /// <returns><see cref="People"/></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<People>> GetAsync([FromRoute] int id)
        {
            return await _peopleRepository.FindAsync(id).ConfigureAwait(false);
        }
    }
}
