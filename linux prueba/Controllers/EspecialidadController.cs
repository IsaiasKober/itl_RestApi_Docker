using linux_prueba.DTO;
using linux_prueba.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace linux_prueba.Controllers
{
    [ApiController]
    [Route("api/Especialidad")]
    public class EspecialidadController : ControllerBase
    {
        private readonly DBContext DBContext;

        public EspecialidadController(DBContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [HttpGet("GetEspecialidades")]
        public async Task<ActionResult<List<EspecialidadDTO>>> Get()
        {
            var List = await DBContext.Especialidades.Select(
                s => new EspecialidadDTO
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

        [HttpGet("GetEspecialidadesById")]
        public async Task<ActionResult<EspecialidadDTO>> GetEspecialidadesById(int Id)
        {
            EspecialidadDTO especialidad = await DBContext.Especialidades.Select(
                    s => new EspecialidadDTO
                    {
                        Id = s.Id,
                        Nombre = s.Nombre
                    })
                .FirstOrDefaultAsync(s => s.Id == Id);

            if (especialidad == null)
            {
                return NotFound();
            }
            else
            {
                return especialidad;
            }
        }

        [HttpPost("InsertEspecialidades")]
        public async Task<HttpStatusCode> InsertEspecialidades(EspecialidadDTO especialidad)
        {
            var entity = new Especialidades()
            {
                Id = especialidad.Id,
                Nombre = especialidad.Nombre,
            };

            DBContext.Especialidades.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("UpdateEspecialidades")]
        public async Task<HttpStatusCode> UpdateEspecialidades(EspecialidadDTO especialidades)
        {
            var entity = await DBContext.Especialidades.FirstOrDefaultAsync(s => s.Id == especialidades.Id);

            entity.Nombre = especialidades.Nombre;

            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteEspecialidades/{Id}")]
        public async Task<HttpStatusCode> DeleteEspecialidades(int Id)
        {
            var entity = new Especialidades()
            {
                Id = Id
            };
            DBContext.Especialidades.Attach(entity);
            DBContext.Especialidades.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}