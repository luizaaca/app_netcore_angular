using Core.Repository;
using Core.Model;
using Repository.EF.Configuration;
using Repository.EF.Repository.Base;

namespace Repository.EF.Repository
{
    public class AssuntoRepository : BaseRepository<Assunto, int>, IAssuntoRepository
    {
        public AssuntoRepository(EFContext context) : base(context)
        {
        }
    }
}
