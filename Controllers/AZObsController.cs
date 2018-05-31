using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ObsApi.Models;
using System.Linq;
using obsApi.Database;


namespace AZObsApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class AZObsController : Controller
    {
        private readonly AzureDbContext _context;

        public AZObsController(AzureDbContext context)
        {
            _context = context;

            //create at least one item
            if (_context.AZObsItems.Count() == 0)
            {
                _context.AZObsItems.Add(new AZObsItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<AZObsItem> GetAll()
        {
            var all = _context.AZObsItems.ToList();
            return all.Select(x=> UpdateGeo(x));
        }

        private AZObsItem UpdateGeo(AZObsItem item)
        {
            item.Geo = GeneralDB.GetLocation(_context, "AZObsItems", item.Id);
            return item;
        }

        [HttpGet("{id}", Name = "GetAZObs")]
        public IActionResult GetById(long id)
        {
            var item = _context.AZObsItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            item.Geo = GeneralDB.GetLocation(_context, "AZObsItems", id);
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] AZObsItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.AZObsItems.Add(item);
            _context.SaveChanges();
            GeneralDB.UpdateLocation(_context, "AZObsItems", item.Geo, item.Id);
            return CreatedAtRoute("GetAZObs", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] AZObsItemDto item)
        {
            if (!ModelState.IsValid)
                 return BadRequest(ModelState);

            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var AZObs = _context.AZObsItems.FirstOrDefault(t => t.Id == id);
            if (AZObs == null)
            {
                return NotFound();
            }

            AZObs.IsComplete = item.IsComplete;
            AZObs.Name = item.Name;


            _context.AZObsItems.Update(AZObs);
            _context.SaveChanges();
            //GeneralDB.UpdateLocation(_context, "AZObsItems", item.Geo, item.Id);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var AZObs = _context.AZObsItems.FirstOrDefault(t => t.Id == id);
            if (AZObs == null)
            {
                return NotFound();
            }

            _context.AZObsItems.Remove(AZObs);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}