using Core.Repository;
using Core.Model;
using Repository.EF.Configuration;
using Repository.EF.Repository.Base;

namespace Repository.EF.Repository
{
    public class UsuarioRepository : BaseRepository<Usuario, int>, IUsuarioRepository
    {
        public UsuarioRepository(EFContext context) : base(context)
        {
        }
    }
}
