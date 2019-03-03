using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TestProject.Domain.Models;
using TestProject.Domain.Models.Metrics;
using TestProject.Domain.Repositories;
using TestProject.Entities;

namespace TestProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StoreController : ControllerBase
    {
        private readonly IReadOnlyRepository _reader;
        private readonly IWriteOnlyRepository _writer;
        private readonly IMapper _mapper;

        public StoreController(IReadOnlyRepository reader, IWriteOnlyRepository writer, IMapper mapper)
        {
            _reader = reader;
            _writer = writer;
            _mapper = mapper;
        }

        [HttpGet("metrics")]
        [SwaggerResponse(200, Type = typeof(StoreMetricsGetModel))]
        public async Task<IActionResult> GetMetrics()
        {
            var stores = await _reader.GetAllAsync<Store>(includeProperties: "Stock");
            var stocks = await _reader.GetAllAsync<Stock>();

            var model = new StoreMetricsGetModel();

            var avg_total_stock_total = stocks.Average(x => x.BackstoreAmount + x.FrontstoreAmount + x.ShoppingWindowAmount);
            var avg_stock_accuracy_total = stocks.Average(x => x.StockAccuracy);
            var avg_on_floor_availability_total = stocks.Average(x => x.OnFloorAvailability);
            var avg_stock_mean_age_in_days_total = stocks.Average(x => x.StockMeanAgeInDays);

            var max_total_stock = stocks.Select(x => new StockMetric { StockId = x.Id, MetricValue = (x.BackstoreAmount + x.FrontstoreAmount + x.ShoppingWindowAmount) }).OrderByDescending(o => o.MetricValue).FirstOrDefault();
            var max_stock_accuracy = stocks.Select(x => new StockMetric { StockId = x.Id, MetricValue = x.StockAccuracy }).OrderByDescending(o => o.MetricValue).FirstOrDefault();
            var max_on_floor_availability = stocks.Select(x => new StockMetric { StockId = x.Id, MetricValue = x.OnFloorAvailability }).OrderByDescending(o => o.MetricValue).FirstOrDefault();
            var max_stock_mean_age_in_days = stocks.Select(x => new StockMetric { StockId = x.Id, MetricValue = x.StockMeanAgeInDays }).OrderByDescending(o => o.MetricValue).FirstOrDefault();

            var min_total_stock = stocks.Select(x => new StockMetric { StockId = x.Id, MetricValue = (x.BackstoreAmount + x.FrontstoreAmount + x.ShoppingWindowAmount) }).OrderBy(o => o.MetricValue).FirstOrDefault();
            var min_stock_accuracy = stocks.Select(x => new StockMetric { StockId = x.Id, MetricValue = x.StockAccuracy }).OrderBy(o => o.MetricValue).FirstOrDefault();
            var min_on_floor_availability = stocks.Select(x => new StockMetric { StockId = x.Id, MetricValue = x.OnFloorAvailability }).OrderBy(o => o.MetricValue).FirstOrDefault();
            var min_stock_mean_age_in_days = stocks.Select(x => new StockMetric { StockId = x.Id, MetricValue = x.StockMeanAgeInDays }).OrderBy(o => o.MetricValue).FirstOrDefault();

            var avg_total_stock = stocks.Select(x => new StockMetric { StockId = x.Id, MetricValue = (x.BackstoreAmount + x.FrontstoreAmount + x.ShoppingWindowAmount), Delta = Math.Abs(x.BackstoreAmount + x.FrontstoreAmount + x.ShoppingWindowAmount - avg_total_stock_total) }).OrderBy(o => o.Delta).FirstOrDefault();
            var avg_stock_accuracy = stocks.Select(x => new StockMetric { StockId = x.Id, MetricValue = x.StockAccuracy, Delta = Math.Abs(x.StockAccuracy - avg_stock_accuracy_total) }).OrderBy(o => o.Delta).FirstOrDefault();
            var avg_on_floor_availability = stocks.Select(x => new StockMetric { StockId = x.Id, MetricValue = x.OnFloorAvailability, Delta = Math.Abs(x.OnFloorAvailability - avg_on_floor_availability_total) }).OrderBy(o => o.Delta).FirstOrDefault();
            var avg_stock_mean_age_in_days = stocks.Select(x => new StockMetric { StockId = x.Id, MetricValue = x.StockMeanAgeInDays, Delta = Math.Abs(x.StockMeanAgeInDays - avg_stock_mean_age_in_days_total) }).OrderBy(o => o.Delta).FirstOrDefault();

            var max_total_stock_store = stores.FirstOrDefault(x => x.StockId == max_total_stock.StockId);
            var max_stock_accuracy_store = stores.FirstOrDefault(x => x.StockId == max_stock_accuracy.StockId);
            var max_on_floor_availability_store = stores.FirstOrDefault(x => x.StockId == max_on_floor_availability.StockId);
            var max_stock_mean_age_in_days_store = stores.FirstOrDefault(x => x.StockId == max_stock_mean_age_in_days.StockId);

            var min_total_stock_store = stores.FirstOrDefault(x => x.StockId == min_total_stock.StockId);
            var min_stock_accuracy_store = stores.FirstOrDefault(x => x.StockId == min_stock_accuracy.StockId);
            var min_on_floor_availability_store = stores.FirstOrDefault(x => x.StockId == min_on_floor_availability.StockId);
            var min_stock_mean_age_in_days_store = stores.FirstOrDefault(x => x.StockId == min_stock_mean_age_in_days.StockId);

            var avg_total_stock_store = stores.FirstOrDefault(x => x.StockId == avg_total_stock.StockId);
            var avg_stock_accuracy_store = stores.FirstOrDefault(x => x.StockId == avg_stock_accuracy.StockId);
            var avg_on_floor_availability_store = stores.FirstOrDefault(x => x.StockId == avg_on_floor_availability.StockId);
            var avg_stock_mean_age_in_days_store = stores.FirstOrDefault(x => x.StockId == avg_stock_mean_age_in_days.StockId);

            model.MaxTotalStock.StoreId = max_total_stock_store.Id;
            model.MaxTotalStock.StoreName = max_total_stock_store.Name;
            model.MaxTotalStock.MetricValue = max_total_stock.MetricValue;

            model.MaxStockAccuracy.StoreId = max_stock_accuracy_store.Id;
            model.MaxStockAccuracy.StoreName = max_stock_accuracy_store.Name;
            model.MaxStockAccuracy.MetricValue = max_stock_accuracy.MetricValue;

            model.MaxOnFloorAvailability.StoreId = max_on_floor_availability_store.Id;
            model.MaxOnFloorAvailability.StoreName = max_on_floor_availability_store.Name;
            model.MaxOnFloorAvailability.MetricValue = max_on_floor_availability.MetricValue;

            model.MaxStockMeanAge.StoreId = max_stock_mean_age_in_days_store.Id;
            model.MaxStockMeanAge.StoreName = max_stock_mean_age_in_days_store.Name;
            model.MaxStockMeanAge.MetricValue = max_stock_mean_age_in_days.MetricValue;

            model.MinTotalStock.StoreId = min_total_stock_store.Id;
            model.MinTotalStock.StoreName = min_total_stock_store.Name;
            model.MinTotalStock.MetricValue = min_total_stock.MetricValue;

            model.MinStockAccuracy.StoreId = min_stock_accuracy_store.Id;
            model.MinStockAccuracy.StoreName = min_stock_accuracy_store.Name;
            model.MinStockAccuracy.MetricValue = min_stock_accuracy.MetricValue;

            model.MinOnFloorAvailability.StoreId = min_on_floor_availability_store.Id;
            model.MinOnFloorAvailability.StoreName = min_on_floor_availability_store.Name;
            model.MinOnFloorAvailability.MetricValue = min_on_floor_availability.MetricValue;

            model.MinStockMeanAge.StoreId = min_stock_mean_age_in_days_store.Id;
            model.MinStockMeanAge.StoreName = min_stock_mean_age_in_days_store.Name;
            model.MinStockMeanAge.MetricValue = min_stock_mean_age_in_days.MetricValue;

            model.AvgTotalStock.StoreId = avg_total_stock_store.Id;
            model.AvgTotalStock.StoreName = avg_total_stock_store.Name;
            model.AvgTotalStock.MetricValue = avg_total_stock.MetricValue;

            model.AvgStockAccuracy.StoreId = avg_stock_accuracy_store.Id;
            model.AvgStockAccuracy.StoreName = avg_stock_accuracy_store.Name;
            model.AvgStockAccuracy.MetricValue = avg_stock_accuracy.MetricValue;

            model.AvgOnFloorAvailability.StoreId = avg_on_floor_availability_store.Id;
            model.AvgOnFloorAvailability.StoreName = avg_on_floor_availability_store.Name;
            model.AvgOnFloorAvailability.MetricValue = avg_on_floor_availability.MetricValue;

            model.AvgStockMeanAge.StoreId = avg_stock_mean_age_in_days_store.Id;
            model.AvgStockMeanAge.StoreName = avg_stock_mean_age_in_days_store.Name;
            model.AvgStockMeanAge.MetricValue = avg_stock_mean_age_in_days.MetricValue;

            return Ok(model);
        }

        [HttpGet]
        [SwaggerResponse(200, Type = typeof(List<StoreGetModel>))]
        public async Task<IActionResult> GetAllAsync()
        {
            var entities = await _reader.GetAllAsync<Store>(includeProperties: "Stock,Country,StoreManager");

            var models = _mapper.Map<List<StoreGetModel>>(entities);

            return Ok(models);
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200, Type = typeof(StoreGetModel))]
        [SwaggerResponse(400, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest($"Id is not a valid {nameof(Guid)}!");
            }

            var entity = await _reader.GetFirstAsync<Store>(x => x.Id == id, includeProperties: "Stock,Country,StoreManager");
            if (entity == null)
                return NotFound();

            var model = _mapper.Map<StoreGetModel>(entity);

            return Ok(model);
        }

        [HttpPost]
        [SwaggerResponse(201, Type = typeof(Guid))]
        [SwaggerResponse(400, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Create([FromBody] StorePostModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _mapper.Map<Store>(model);

            await _writer.SaveEntityAsync(entity);

            return StatusCode(201, entity.Id);
        }

        [HttpPut]
        [SwaggerResponse(204)]
        [SwaggerResponse(400, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Update([FromBody] StorePutModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _mapper.Map<Store>(model);

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

            var entity = await _reader.GetByIdAsync<Store>(id);

            if (entity == null)
                throw new Exception($"Entity with id = {id} already deleted");

            await _writer.DeleteAsync<Store>(entity);

            return NoContent();
        }
    }
}