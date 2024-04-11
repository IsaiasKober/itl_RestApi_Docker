using linux_prueba.DTO;
using linux_prueba.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace linux_prueba.Controllers
{
    [ApiController]
    [Route("api/Carreras")]
    public class CarreraController : ControllerBase
    {
        private readonly DBContext DBContext;

        public CarreraController(DBContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [HttpGet("GetCarreras")]
        public async Task<ActionResult<List<CarreraDTO>>> Get()
        {
            var List = await DBContext.Carreras.Select(
                s => new CarreraDTO
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                }
            ).ToListAsync();

            if (List.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return List;
            }
        }

        [HttpGet("GetCarrerasById")]
        public async Task<ActionResult<CarreraDTO>> GetCarreraById(int Id)
        {
            CarreraDTO carreras = await DBContext.Carreras.Select(
                    s => new CarreraDTO
                    {
                        Id = s.Id,
                        Nombre = s.Nombre
                    })
                .FirstOrDefaultAsync(s => s.Id == Id);

            if (carreras == null)
            {
                return NotFound();
            }
            else
            {
                return carreras;
            }
        }

        [HttpPost("InsertCarreras")]
        public async Task<HttpStatusCode> InsertCarreras(CarreraDTO carreras)
        {
            var entity = new Carreras()
            {
                Id = carreras.Id,
                Nombre = carreras.Nombre,
            };

            DBContext.Carreras.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("UpdateCarreras")]
        public async Task<HttpStatusCode> UpdateCarrera(CarreraDTO carreras)
        {
            var entity = await DBContext.Carreras.FirstOrDefaultAsync(s => s.Id == carreras.Id);

            entity.Nombre = carreras.Nombre;

            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteCarreras/{Id}")]
        public async Task<HttpStatusCode> DeleteCarrera(int Id)
        {
            var entity = new Carreras()
            {
                Id = Id
            };
            DBContext.Carreras.Attach(entity);
            DBContext.Carreras.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}