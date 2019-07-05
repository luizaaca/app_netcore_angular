using Core.Repository;
using Core.Model;
using Repository.EF.Configuration;
using Repository.EF.Repository.Base;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository.EF.Repository
{
    public class UsuarioRepository : BaseRepository<Usuario, int>, IUsuarioRepository
    {
        public UsuarioRepository(EFContext context) : base(context)
        {
        }

        public async Task<Usuario> FindUsuarioCompletoAsync(int id)
            => await context
                        .Usuarios
                        .Include(u => u.Condominio.Responsavel)
                        .Include(u => u.Condominio.Administradora)
                        .SingleOrDefaultAsync();
    }
}
