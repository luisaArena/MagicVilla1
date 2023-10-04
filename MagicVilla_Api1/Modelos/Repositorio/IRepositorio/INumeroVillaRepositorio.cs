using MagicVilla_Api1.Modelos;

namespace MagicVilla_Api1.Modelos.Repositorio.IRepositorio
{
    public interface INumeroVillaRepositorio : IRepositorio<NumeroVilla>
    {
        Task<NumeroVilla> Actualizar(NumeroVilla entidad);
    }
}
