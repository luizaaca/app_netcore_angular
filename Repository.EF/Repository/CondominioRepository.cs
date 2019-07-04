using Core.Repository;
using Core.Model;
using Repository.EF.Configuration;
using Repository.EF.Repository.Base;

namespace Repository.EF.Repository
{
    public class CondominioRepository : BaseRepository<Condominio, int>, ICondominioRepository
    {
        public CondominioRepository(EFContext context) : base(context)
        {
        }
    }
}
