using MagicVilla_Api1.Modelos.Repositorio.IRepositorio;
using MagicVilla_Api1.Datos;
using MagicVilla_Api1.Modelos;

namespace MagicVilla_Api1.Modelos.Repositorio
{
    public class VillaRepositorio : Repositorio<Villa>, IVillaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public VillaRepositorio(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }

        public async Task<Villa> Actualizar(Villa entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _db.villas.Update(entidad);    
            await _db.SaveChangesAsync();
            return entidad; 
        }
    }
}
