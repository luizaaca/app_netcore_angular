using Core.Repository;
using Core.Model;
using Repository.EF.Configuration;
using Repository.EF.Repository.Base;

namespace Repository.EF.Repository
{
    public class AdministradoraRepository : BaseRepository<Administradora, int>, IAdministradoraRepository
    {
        public AdministradoraRepository(EFContext context) : base(context)
        {
        }
    }
}
