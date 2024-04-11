using linux_prueba.DTO;
using linux_prueba.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace linux_prueba.Controllers
{
    [ApiController]
    [Route("api/Materias")]
    public class MateriaController : ControllerBase
    {
        private readonly DBContext DBContext;

        public MateriaController(DBContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [HttpGet("GetMaterias")]
        public async Task<ActionResult<List<MateriaDTO>>> Get()
        {
            var List = await DBContext.Materias.Select(
                s => new MateriaDTO
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

        [HttpGet("GetMateriasById")]
        public async Task<ActionResult<MateriaDTO>> GetMateriasById(int Id)
        {
            MateriaDTO materias = await DBContext.Materias.Select(
                    s => new MateriaDTO
                    {
                        Id = s.Id,
                        Nombre = s.Nombre
                    })
                .FirstOrDefaultAsync(s => s.Id == Id);

            if (materias == null)
            {
                return NotFound();
            }
            else
            {
                return materias;
            }
        }

        [HttpPost("InsertMaterias")]
        public async Task<HttpStatusCode> InsertMaterias(MateriaDTO materias)
        {
            var entity = new Materias()
            {
                Id = materias.Id,
                Nombre = materias.Nombre,
            };

            DBContext.Materias.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("UpdateMaterias")]
        public async Task<HttpStatusCode> UpdateMaterias(MateriaDTO materias)
        {
            var entity = await DBContext.Materias.FirstOrDefaultAsync(s => s.Id == materias.Id);

            entity.Nombre = materias.Nombre;

            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteMaterias/{Id}")]
        public async Task<HttpStatusCode> DeleteMaterias(int Id)
        {
            var entity = new Materias()
            {
                Id = Id
            };
            DBContext.Materias.Attach(entity);
            DBContext.Materias.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}