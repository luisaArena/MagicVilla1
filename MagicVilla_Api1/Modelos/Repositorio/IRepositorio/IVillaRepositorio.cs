using MagicVilla_Api1.Modelos;

namespace MagicVilla_Api1.Modelos.Repositorio.IRepositorio
{
    public interface IVillaRepositorio : IRepositorio<Villa>
    {
        Task<Villa> Actualizar(Villa entidad);
    }
}
