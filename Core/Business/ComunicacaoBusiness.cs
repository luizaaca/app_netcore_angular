using Core.Business.DTOs;
using Core.Business.Wraps;
using Core.Repository;
using System;
using System.Threading.Tasks;

namespace Core.Business
{
    public class ComunicacaoBusiness : IComunicacaoBusiness
    {
        ICondominioRepository _condominioRepository;
        IAdministradoraRepository _administradoraRepository;
        IUsuarioRepository _usuarioRepository;
        IAssuntoRepository _assuntoRepository;

        public ComunicacaoBusiness(
            IAdministradoraRepository administradoraRepository,
            IAssuntoRepository assuntoRepository,
            ICondominioRepository condominioRepository,
            IUsuarioRepository usuarioRepository)
        {
            _administradoraRepository = administradoraRepository;
            _assuntoRepository = assuntoRepository;
            _condominioRepository = condominioRepository;
            _usuarioRepository = usuarioRepository;
        }

        public Task<BaseResponse> EnviarComunicadoAsync(BaseRequest<Comunicado> request)
        {
            throw new NotImplementedException();
        }
    }
}
