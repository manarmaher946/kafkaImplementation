using Microsoft.AspNetCore.Mvc;
using ProducerImplementation.ProducerService;
using System.Text.Json;
using ProducerImplementation.Models;
using backgroundImplementation.Data;
using Kafkaproducerimplemenation.DTO;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using Kafkaproducerimplemenation.GenaricPagination;
using static Confluent.Kafka.ConfigPropertyNames;
using Producer = ProducerImplementation.ProducerService.Producer;

namespace ProducerImplementation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly Producer _producer;
        private readonly ApplicationDbcontext  _application;

        public ProducerController(Producer producer, ApplicationDbcontext application)
        {
            _producer = producer;
            _application = application;
        }
        [HttpPost]
        public async Task<IActionResult> Addprince(Princesrequest request)
        {
            var prince = new prince()
            {
                Name = request.Name,
                Description = request.Description,
            };

            _application.Princes.Add(prince);
            await _application.SaveChangesAsync(); 

            var message = JsonSerializer.Serialize(request);

            await _producer.ProduceAsync("princesAdded", message);

            return Ok("Prince added successfully.");
        }

        [HttpGet]
        [Route("GetAllprinceUsingOffset")]
        public async Task<IActionResult> GetAllprince(int pagenumber,int pagesize)
        {
            //var pageelements = 5f;
            //var allpages = Math.Ceiling(_application.Princes.Count() / pageelements);
            //var princes=await _application.Princes.Skip((pagenumber-1)*(int)pageelements).Take((int)pageelements).ToListAsync();
            //return Ok(new
            //{
            //    currentpage=pagenumber,
            //    AllPages=allpages,
            //    princes=princes
            //});
            var Princes=  _application.Princes.AsQueryable();
            var paginatedList = await PaginatedList<prince>.paginatedList(Princes, pagenumber, pagesize);
            return Ok(paginatedList);
        }

        [HttpGet]
        [Route("GetAllPrinceUsingKeySet")]
        public async Task<IActionResult> GetAllPrinceUsingKeySet(int? lastId = null, int pageSize = 10)
        {
            IQueryable<prince> query = _application.Princes.OrderBy(p => p.Id);

            if (lastId.HasValue)
            {
                query = query.Where(p => p.Id > lastId.Value);
            }

            var items = await query.Take(pageSize).ToListAsync();
            var hasNextPage = items.Count == pageSize;

            var result = new PaginatedListKeyset<prince>(items, hasNextPage);

            return Ok(result);
        }

    }
}

