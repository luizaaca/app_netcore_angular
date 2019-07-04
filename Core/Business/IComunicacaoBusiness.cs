using Core.Business.DTOs;
using Core.Business.Wraps;
using System.Threading.Tasks;

namespace Core.Business
{
    public interface IComunicacaoBusiness
    {
        Task<BaseResponse> EnviarComunicadoAsync(BaseRequest<Comunicado> request);
    }
}
