using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TestProject.Domain.Models;
using TestProject.Domain.Repositories;
using TestProject.Entities;

namespace TestProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StoreManagerController : ControllerBase
    {
        private readonly IReadOnlyRepository _reader;
        private readonly IWriteOnlyRepository _writer;
        private readonly IMapper _mapper;

        public StoreManagerController(IReadOnlyRepository reader, IWriteOnlyRepository writer, IMapper mapper)
        {
            _reader = reader;
            _writer = writer;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerResponse(200, Type = typeof(List<StoreManagerGetModel>))]
        public async Task<IActionResult> GetAllAsync()
        {
            var entities = await _reader.GetAllAsync<StoreManager>();

            var models = _mapper.Map<List<StoreManagerGetModel>>(entities);

            return Ok(models);
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200, Type = typeof(StoreManagerGetModel))]
        [SwaggerResponse(400, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest($"Id is not a valid {nameof(Guid)}!");
            }

            var entity = await _reader.GetByIdAsync<StoreManager>(id);
            if (entity == null)
                return NotFound();

            var model = _mapper.Map<StoreManagerGetModel>(entity);

            return Ok(model);
        }

        [HttpPost]
        [SwaggerResponse(201, Type = typeof(Guid))]
        [SwaggerResponse(400, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Create([FromBody] StoreManagerPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _mapper.Map<StoreManager>(model);

            await _writer.SaveEntityAsync(entity);

            return StatusCode(201, entity.Id);
        }

        [HttpPut]
        [SwaggerResponse(204)]
        [SwaggerResponse(400, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Update([FromBody] StoreManagerPutModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _mapper.Map<StoreManager>(model);

            await _writer.SaveEntityAsync(entity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest($"Id is not a valid {nameof(Guid)}!");
            }

            var entity = await _reader.GetByIdAsync<StoreManager>(id);

            if (entity == null)
                throw new Exception($"Entity with id = {id} already deleted");

            await _writer.DeleteAsync<StoreManager>(entity);

            return NoContent();
        }
    }
}