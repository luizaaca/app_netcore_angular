using System.Threading.Tasks;
using Core.Model;
using Core.Repository.Abstractions;

namespace Core.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario, int>
    {
        Task<Usuario> FindUsuarioAsync(int id);
    }
}
