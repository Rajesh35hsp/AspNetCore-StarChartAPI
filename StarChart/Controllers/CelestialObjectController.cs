using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            CelestialObject celestialObject = _context.CelestialObjects.FirstOrDefault<CelestialObject>(x => x.Id == id);

            if (celestialObject == null)
            {
                return NotFound();
            }
            else
            {
                celestialObject.Satellites = _context.CelestialObjects.Where<CelestialObject>(x => x.OrbitedObjectId == id).ToList();
            }
            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.Where<CelestialObject>(x => x.Name == name).ToList();

            if (celestialObjects.Count == 0)
            {

                return NotFound();

            }
            else
            {


                foreach (CelestialObject item in celestialObjects)
                {
                    item.Satellites = _context.CelestialObjects.Where<CelestialObject>(x => x.OrbitedObjectId == item.Id).ToList();
                }
                return Ok(celestialObjects);
            }

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.ToList();

            foreach (CelestialObject item in celestialObjects)
            {
                item.Satellites = _context.CelestialObjects.Where<CelestialObject>(x => x.OrbitedObjectId == item.Id).ToList();
            }

            return Ok(celestialObjects);

        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            CelestialObject celestialObjectExisting = _context.CelestialObjects.FirstOrDefault<CelestialObject>(x => x.Id == id);

            if (celestialObjectExisting == null)
            {
                return NotFound();
            }
            else
            {
                celestialObjectExisting.Name = celestialObject.Name;
                celestialObjectExisting.OrbitalPeriod = celestialObject.OrbitalPeriod;
                celestialObjectExisting.OrbitedObjectId = celestialObject.OrbitedObjectId;

                _context.CelestialObjects.Update(celestialObjectExisting);
                _context.SaveChanges();

                return NoContent();
            }
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            CelestialObject celestialObjectExisting = _context.CelestialObjects.FirstOrDefault<CelestialObject>(x => x.Id == id);
            if (celestialObjectExisting == null)
            {
                return NotFound();
            }
            else
            {
                celestialObjectExisting.Name = name;
                _context.CelestialObjects.Update(celestialObjectExisting);
                _context.SaveChanges();

                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.Where<CelestialObject>(x => x.Id == id || x.OrbitedObjectId ==id).ToList();

            if (celestialObjects.Count==0)
            {
                return NotFound();
            }
            else
            {
                _context.CelestialObjects.RemoveRange(celestialObjects);
                _context.SaveChanges();
                return NoContent();
            }

        }
    }
}
