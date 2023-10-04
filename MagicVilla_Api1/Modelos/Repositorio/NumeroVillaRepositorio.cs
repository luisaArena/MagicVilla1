using MagicVilla_Api1.Modelos.Repositorio.IRepositorio;
using MagicVilla_Api1.Datos;
using MagicVilla_Api1.Modelos;
using System.Linq.Expressions;

namespace MagicVilla_Api1.Modelos.Repositorio
{
    public class NumeroVillaRepositorio : Repositorio<NumeroVilla>, INumeroVillaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public NumeroVillaRepositorio(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }

        public async Task<NumeroVilla> Actualizar(NumeroVilla entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _db.NumeroVilla.Update(entidad);    
            await _db.SaveChangesAsync();
            return entidad; 
        }

    }
}
