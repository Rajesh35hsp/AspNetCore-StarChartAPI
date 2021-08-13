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
    }
}
