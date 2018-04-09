using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ObsApi.Models;
using System.Linq;

namespace ObsApi.Controllers
{
    [Route("api/[controller]")]
    public class ObsController : Controller
    {
        private readonly ObsContext _context;

        public ObsController(ObsContext context)
        {
            _context = context;

            if (_context.ObsItems.Count() == 0)
            {
                _context.ObsItems.Add(new ObsItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<ObsItem> GetAll()
        {
            return _context.ObsItems.ToList();
        }

        [HttpGet("{id}", Name = "GetObs")]
        public IActionResult GetById(long id)
        {
            var item = _context.ObsItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }  

           [HttpPost]
            public IActionResult Create([FromBody] ObsItem item)
            {
                if (item == null)
                {
                    return BadRequest();
                }

                _context.ObsItems.Add(item);
                _context.SaveChanges();

                return CreatedAtRoute("GetObs", new { id = item.Id }, item);
            } 

            [HttpPut("{id}")]
            public IActionResult Update(long id, [FromBody] ObsItem item)
            {
                if (item == null || item.Id != id)
                {
                    return BadRequest();
                }

                var Obs = _context.ObsItems.FirstOrDefault(t => t.Id == id);
                if (Obs == null)
                {
                    return NotFound();
                }

                Obs.IsComplete = item.IsComplete;
                Obs.Name = item.Name;

                _context.ObsItems.Update(Obs);
                _context.SaveChanges();
                return new NoContentResult();
            }

            [HttpDelete("{id}")]
            public IActionResult Delete(long id)
            {
                var Obs = _context.ObsItems.FirstOrDefault(t => t.Id == id);
                if (Obs == null)
                {
                    return NotFound();
                }

                _context.ObsItems.Remove(Obs);
                _context.SaveChanges();
                return new NoContentResult();
            }
    }
}