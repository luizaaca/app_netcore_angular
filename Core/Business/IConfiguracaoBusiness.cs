using Core.Business.DTOs;
using Core.Business.Wraps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Business
{
    public interface IConfiguracaoBusiness
    {
        Task<BaseResponse<IList<Condominio>>> ListarCondominiosAsync();
        Task<BaseResponse<IList<Usuario>>> ListarUsuariosAsync();
        Task<BaseResponse<IList<Administradora>>> ListarAdministradorasAsync();
        Task<BaseResponse<IList<Assunto>>> ListarAssuntosAsync();
        Task<BaseResponse<Condominio>> BuscarCondominioAsync(BaseRequest<int> request);
        Task<BaseResponse<Usuario>> BuscarUsuarioAsync(BaseRequest<int> request);
        Task<BaseResponse<int>> SalvarCondominioAsync(BaseRequest<Condominio> request);
        Task<BaseResponse<int>> SalvarUsuarioAsync(BaseRequest<Usuario> request);
    }
}
