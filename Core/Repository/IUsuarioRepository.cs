using Core.Repository.Abstractions;
using Core.Model;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario, int>
    {
        Task<Usuario> FindUsuarioCompletoAsync(int id);
    }
}
