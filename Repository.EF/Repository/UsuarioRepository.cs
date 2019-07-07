using System.Threading.Tasks;
using Core.Model;
using Core.Repository;
using Repository.EF.Configuration;
using Repository.EF.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Repository.EF.Repository
{
    public class UsuarioRepository : BaseRepository<Usuario, int>, IUsuarioRepository
    {
        public UsuarioRepository(EFContext context) : base(context)
        {
        }

        public async Task<Usuario> FindUsuarioAsync(int id)
        {
            var usuario = await context.Usuarios
                                        .Include(u => u.Condominio)
                                        .SingleOrDefaultAsync(u => u.Id == id);

            return usuario;
        }
    }
}
